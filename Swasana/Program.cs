using System.Globalization;
using Npgsql;
using Swasana.Data;
using Swasana.Models;
using Swasana.Services;

namespace Swasana;

public class Program
{
    // Pakai:  dotnet run                          -> simpan lokasi default (Yogyakarta)
    //         dotnet run -- Sleman -7.7162 110.3555
    //         dotnet run -- risk                  -> evaluasi risiko saja (tanpa database)
    public static void Main(string[] args)
    {
        if (args.Length >= 1 && args[0].Equals("risk", StringComparison.OrdinalIgnoreCase))
        {
            DemoRisiko();
            return;
        }

        var db = new DatabaseContext(GetConnectionString());

        // ===== Soal 2: koneksi aplikasi ke PostgreSQL =====
        Console.WriteLine("== Tes koneksi ke PostgreSQL ==");
        try
        {
            using var conn = db.connect();
            Console.WriteLine($"Terhubung ke database '{conn.Database}' (PostgreSQL {conn.PostgreSqlVersion})");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Gagal konek: {ex.Message}");
            return;
        }

        // ===== Soal 1: proses simpan & ambil lokasi lewat repository (OOP) =====
        var users = new UserRepository(db);
        var locations = new LocationRepository(db);

        var user = users.FindByUsername("raka") ?? users.Insert(new User { Username = "raka" });
        Console.WriteLine($"\nUser aktif: {user.Username} (user_id = {user.UserId})");

        var city = args.Length >= 3 ? args[0] : "Yogyakarta";
        var lat = args.Length >= 3 ? double.Parse(args[1], CultureInfo.InvariantCulture) : -7.795580;
        var lon = args.Length >= 3 ? double.Parse(args[2], CultureInfo.InvariantCulture) : 110.369490;

        var location = new Location { UserId = user.UserId, Name = city };
        location.setLocation(lat, lon);
        locations.Insert(location);
        Console.WriteLine($"Lokasi '{location.Name}' tersimpan dengan location_id = {location.LocationId}");

        Console.WriteLine($"\n== Lokasi milik {user.Username} (dibaca ulang dari tabel locations) ==");
        foreach (var loc in locations.GetByUser(user.UserId))
        {
            var (la, lo) = loc.getCoordinates();
            Console.WriteLine($"[{loc.LocationId}] {loc.Name,-15} lat {la,10:F6}  lon {lo,11:F6}");
        }
    }

    // Menilai beberapa contoh kondisi cuaca memakai kumpulan aturan RiskRule.
    private static void DemoRisiko()
    {
        var evaluator = new RiskEvaluator();

        var contoh = new (string Keterangan, WeatherData Data)[]
        {
            ("Cerah",               new WeatherData { Temperature = 30, Rainfall = 0,  WindSpeed = 10 }),
            ("Panas terik",         new WeatherData { Temperature = 36, Rainfall = 0,  WindSpeed = 12 }),
            ("Hujan sedang",        new WeatherData { Temperature = 28, Rainfall = 7,  WindSpeed = 15 }),
            ("Hujan lebat + angin", new WeatherData { Temperature = 27, Rainfall = 15, WindSpeed = 50 })
        };

        Console.WriteLine("== Evaluasi risiko: satu List<RiskRule>, isi Evaluate() berbeda-beda ==\n");

        foreach (var (keterangan, data) in contoh)
        {
            Console.WriteLine($"{keterangan}");
            Console.WriteLine($"   suhu {data.Temperature:F0} C | hujan {data.Rainfall:F0} mm/jam | angin {data.WindSpeed:F0} km/jam");

            foreach (var (nama, level) in evaluator.rincian(data))
                Console.WriteLine($"   - {nama,-14} : {level}");

            Console.WriteLine($"   Hasil akhir      : {evaluator.evaluateRisk(data)}\n");
        }
    }

    // Connection string diambil dari environment variable SWASANA_DB.
    // Kalau tidak ada, pakai default lokal dan tanyakan password postgres (tidak ditulis di kode).
    private static string GetConnectionString()
    {
        var fromEnv = Environment.GetEnvironmentVariable("SWASANA_DB");
        if (!string.IsNullOrWhiteSpace(fromEnv)) return fromEnv;

        Console.Write("Password user postgres: ");
        var password = ReadHidden();
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = "localhost",
            Port = 5432,
            Database = "swasana_db",
            Username = "postgres",
            Password = password
        };
        return builder.ConnectionString;
    }

    private static string ReadHidden()
    {
        if (Console.IsInputRedirected) return Console.ReadLine() ?? "";

        var chars = new List<char>();
        while (true)
        {
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter) break;
            if (key.Key == ConsoleKey.Backspace) { if (chars.Count > 0) chars.RemoveAt(chars.Count - 1); }
            else chars.Add(key.KeyChar);
        }
        Console.WriteLine();
        return new string(chars.ToArray());
    }
}
