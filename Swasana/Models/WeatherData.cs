using System.Text.Json;
using Swasana.Services;

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

    // Hanya untuk tampilan (tidak disimpan ke database)
    public int WeatherCode { get; set; }      // kode cuaca WMO
    public bool IsDay { get; set; } = true;
    public double ApparentTemperature { get; set; }   // °C, suhu yang terasa
    public double UvIndex { get; set; }
    public List<PrakiraanJam> Hourly { get; set; } = new List<PrakiraanJam>();
    public List<PrakiraanHari> Daily { get; set; } = new List<PrakiraanHari>();
    public int? Aqi { get; set; }             // US AQI dari Open-Meteo Air Quality, null kalau tidak tersedia
    public double? Pm25 { get; set; }         // µg/m³

    // Ambil cuaca terkini lokasi dari Open-Meteo Web API
    public static async Task<WeatherData> fetchFromAPI(Location location)
    {
        var (lat, lon) = location.getCoordinates();
        var data = await new WeatherService().GetCurrentAsync(lat, lon);
        data.LocationId = location.LocationId;
        return data;
    }

    public string toJSON()
    {
        return JsonSerializer.Serialize(this);
    }
}
