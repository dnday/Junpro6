using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

// Nilai enum = risk_id di tabel risk_levels (lihat database/swasana_db.sql)
public enum RiskLevel
{
    Aman = 1,
    Waspada = 2,
    Bahaya = 3
}

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = "";
    public DateTime CreatedAt { get; set; }

    public List<Location> Locations { get; } = new List<Location>();
}

public class Location
{
    public int LocationId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = "";
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public List<WeatherData> WeatherDataList { get; } = new List<WeatherData>();

    public (double, double) getCoordinates()
    {
        return (Latitude, Longitude);
    }

    public void setLocation(double lat, double lon)
    {
        if (lat < -90 || lat > 90) throw new ArgumentOutOfRangeException(nameof(lat));
        if (lon < -180 || lon > 180) throw new ArgumentOutOfRangeException(nameof(lon));
        Latitude = lat;
        Longitude = lon;
    }
}

public class WeatherData
{
    public long WeatherId { get; set; }
    public int LocationId { get; set; }
    public RiskLevel RiskLevel { get; set; }
    public double Temperature { get; set; }   // °C
    public double Humidity { get; set; }      // %
    public double WindSpeed { get; set; }     // km/jam
    public double Rainfall { get; set; }      // mm/jam
    public DateTime Timestamp { get; set; }

    public static WeatherData fetchFromAPI(Location location)
    {
        throw new NotImplementedException();
    }

    public string toJSON()
    {
        throw new NotImplementedException();
    }
}

public class RiskEvaluator
{
    // Sama dengan isi seed risk_levels. Hujan & angin mengikuti kategori BMKG per jam.
    private double suhuWaspada = 33;
    private double suhuBahaya = 35;
    private double hujanSedang = 5;    // mm/jam
    private double hujanLebat = 10;    // mm/jam
    private double anginKencang = 45;  // km/jam (~25 knot)

    public RiskLevel evaluateRisk(WeatherData data)
    {
        if (data.Temperature > suhuBahaya ||
            (data.Rainfall >= hujanLebat && data.WindSpeed >= anginKencang))
            return RiskLevel.Bahaya;

        if (data.Temperature > suhuWaspada || data.Rainfall >= hujanSedang)
            return RiskLevel.Waspada;

        return RiskLevel.Aman;
    }
}

public class ActionRecommender
{
    // Satu level risiko bisa punya banyak saran (relasi 1:N di tabel recommendations)
    private Dictionary<RiskLevel, List<string>> recommendationRules = new Dictionary<RiskLevel, List<string>>();

    public List<string> getRecommendation(RiskLevel level)
    {
        throw new NotImplementedException();
    }
}

public class SearchHistory
{
    public long HistoryId { get; set; }
    public int UserId { get; set; }
    public long WeatherId { get; set; }
    public DateTime SearchedAt { get; set; }
    public string? Note { get; set; }
}

public class WeatherHistoryManager
{
    private DatabaseContext context;

    public WeatherHistoryManager(DatabaseContext context)
    {
        this.context = context;
    }

    public void saveHistory(User user, WeatherData data, string? note = null)
    {
        throw new NotImplementedException();
    }

    public List<SearchHistory> getHistory(User user)
    {
        throw new NotImplementedException();
    }
}

public class DatabaseContext
{
    private string connectionString;

    public DatabaseContext(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void connect()
    {
        throw new NotImplementedException();
    }

    public DataSet executeQuery(string sql)
    {
        throw new NotImplementedException();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // Cek cepat aturan risiko dari W1
        var r = new RiskEvaluator();
        Debug.Assert(r.evaluateRisk(new WeatherData { Temperature = 30 }) == RiskLevel.Aman);
        Debug.Assert(r.evaluateRisk(new WeatherData { Temperature = 34 }) == RiskLevel.Waspada);
        Debug.Assert(r.evaluateRisk(new WeatherData { Temperature = 30, Rainfall = 6 }) == RiskLevel.Waspada);
        Debug.Assert(r.evaluateRisk(new WeatherData { Temperature = 36 }) == RiskLevel.Bahaya);
        Debug.Assert(r.evaluateRisk(new WeatherData { Temperature = 30, Rainfall = 12 }) == RiskLevel.Waspada);
        Debug.Assert(r.evaluateRisk(new WeatherData { Temperature = 30, Rainfall = 12, WindSpeed = 50 }) == RiskLevel.Bahaya);
        Console.WriteLine("RiskEvaluator OK");
    }
}
