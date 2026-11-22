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
        return File.Exists(ConfigPath) ? File.ReadAllText(ConfigPath).Trim() : null;
    }

    public static void SaveConnectionString(string connectionString)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);
        File.WriteAllText(ConfigPath, connectionString);
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

    private static string ReadSchema()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("swasana_schema.sql")
            ?? throw new InvalidOperationException("swasana_schema.sql tidak ditemukan di dalam aplikasi.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
