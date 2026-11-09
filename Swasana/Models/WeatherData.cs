namespace Swasana.Models;

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
