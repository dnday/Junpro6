using System;
using System.Collections.Generic;
using System.Data;

public enum RiskLevel
{
    Aman,
    Waspada,
    Bahaya
}

public class Location
{
    private int locationId;
    private string name;
    private double latitude;
    private double longitude;

    private List<WeatherData> weatherDataList;

    public Location()
    {
        weatherDataList = new List<WeatherData>();
    }

    public (double, double) getCoordinates()
    {
        throw new NotImplementedException();
    }

    public void setLocation(double lat, double lon)
    {
        throw new NotImplementedException();
    }
}

public class WeatherData
{
    private double temperature;
    private double humidity;
    private double windSpeed;
    private double rainfall;
    private DateTime timestamp;

    public WeatherData fetchFromAPI(Location location)
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
    private double thresholdWaspada;
    private double thresholdBahaya;

    public RiskLevel evaluateRisk(WeatherData data)
    {
        throw new NotImplementedException();
    }
}

public class ActionRecommender
{
    private Dictionary<RiskLevel, string> recommendationRules;

    public string getRecommendation(RiskLevel level)
    {
        throw new NotImplementedException();
    }
}

public class WeatherHistoryManager
{
    private DatabaseContext context;
    private List<WeatherData> weatherDataList;

    public WeatherHistoryManager(DatabaseContext context)
    {
        this.context = context;
        this.weatherDataList = new List<WeatherData>();
    }

    public void saveHistory(WeatherData data)
    {
        throw new NotImplementedException();
    }

    public List<WeatherData> getHistory(Location loc)
    {
        throw new NotImplementedException();
    }
}

public class DatabaseContext
{
    private string connectionString;

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
    }
}