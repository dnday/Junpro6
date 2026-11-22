using System.Reflection;
using Npgsql;

namespace Swasana.Data;

// Menyiapkan database supaya pengguna tidak perlu menjalankan skrip SQL manual:
// buat database kalau belum ada, lalu jalankan skema (aman diulang).
public static class DatabaseSetup
{
    // Lokasi file koneksi: %APPDATA%\SWASANA\koneksi.txt (per user Windows)
    private static readonly string ConfigPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SWASANA", "koneksi.txt");

    // ponytail: password tersimpan polos di profil user sendiri; enkripsi (DPAPI) kalau aplikasi dipakai bersama
    public static string? LoadConnectionString()
    {
        var fromEnv = Environment.GetEnvironmentVariable("SWASANA_DB");
        if (!string.IsNullOrWhiteSpace(fromEnv)) return fromEnv;
        if (File.Exists(ConfigPath)) return File.ReadAllText(ConfigPath).Trim();
        return KoneksiBawaan;   // tanpa pengaturan sendiri -> database online bawaan (kalau ditanam saat build)
    }

    // Koneksi database online bawaan yang ditanam di exe (null kalau exe dibuat tanpa file koneksi-bawaan.txt)
    public static string? KoneksiBawaan { get; } = BacaResource("koneksi-bawaan.txt")?.Trim();

    // Kembali ke database bawaan: hapus pengaturan koneksi milik pengguna
    public static void HapusPengaturan()
    {
        if (File.Exists(ConfigPath)) File.Delete(ConfigPath);
    }

    public static void SaveConnectionString(string connectionString)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);
        File.WriteAllText(ConfigPath, connectionString);
    }

    // Server di komputer sendiri tidak wajib SSL; server online (mis. Supabase) wajib terenkripsi
    public static SslMode PilihSsl(string host) =>
        host is "localhost" or "127.0.0.1" or "::1" ? SslMode.Prefer : SslMode.Require;

    // Membaca URI koneksi seperti yang diberikan Supabase:
    //   postgresql://user:password@host:5432/postgres
    // Password berupa placeholder ([YOUR-PASSWORD]) diabaikan supaya diisi sendiri.
    public static NpgsqlConnectionStringBuilder? BacaUri(string teks)
    {
        teks = teks.Trim();
        if (!teks.StartsWith("postgres://") && !teks.StartsWith("postgresql://")) return null;
        if (!Uri.TryCreate(teks, UriKind.Absolute, out var uri) || uri.Host.Length == 0) return null;

        var info = uri.UserInfo.Split(':', 2);
        var password = info.Length > 1 ? Uri.UnescapeDataString(info[1]) : "";
        var database = Uri.UnescapeDataString(uri.AbsolutePath.Trim('/'));
        return new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.Port > 0 ? uri.Port : 5432,
            Database = database.Length > 0 ? database : "postgres",
            Username = Uri.UnescapeDataString(info[0]),
            Password = password.StartsWith("[") ? "" : password
        };
    }

    // Buat database (jika belum ada) lalu tabel + seed. Melempar exception kalau gagal.
    public static void EnsureDatabase(string connectionString)
    {
        var target = new NpgsqlConnectionStringBuilder(connectionString);
        var databaseName = target.Database ?? "swasana_db";

        var admin = new NpgsqlConnectionStringBuilder(connectionString) { Database = "postgres", Pooling = false };
        using (var conn = new NpgsqlConnection(admin.ConnectionString))
        {
            conn.Open();
            using var check = new NpgsqlCommand("SELECT 1 FROM pg_database WHERE datname = @name", conn);
            check.Parameters.AddWithValue("name", databaseName);
            if (check.ExecuteScalar() == null)
            {
                // Nama database tidak bisa dikirim sebagai parameter, jadi di-quote manual
                using var create = new NpgsqlCommand($"CREATE DATABASE \"{databaseName.Replace("\"", "\"\"")}\"", conn);
                create.ExecuteNonQuery();
            }
        }

        EnsureSchema(connectionString);
    }

    // Jalankan skema (tabel, kolom baru, seed). Aman diulang, dipanggil juga setiap aplikasi dibuka
    // supaya database dari versi lama otomatis mendapat kolom/tabel baru.
    public static void EnsureSchema(string connectionString)
    {
        using var db = new NpgsqlConnection(connectionString);
        db.Open();
        using var schema = new NpgsqlCommand(ReadSchema(), db);
        schema.ExecuteNonQuery();
    }

    private static string ReadSchema() =>
        BacaResource("swasana_schema.sql") ?? throw new InvalidOperationException("swasana_schema.sql tidak ditemukan di dalam aplikasi.");

    private static string? BacaResource(string nama)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(nama);
        if (stream == null) return null;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
