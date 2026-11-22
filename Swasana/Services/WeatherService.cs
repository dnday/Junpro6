using System.Globalization;
using System.Text.Json;
using Swasana.Models;

namespace Swasana.Services;

// Layanan web API Open-Meteo (gratis, tanpa API key):
//  - Geocoding : nama kota -> koordinat
//  - Forecast  : koordinat -> cuaca saat ini
public class WeatherService
{
    private static readonly HttpClient http = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };

    public async Task<(string Name, double Latitude, double Longitude)?> FindCityAsync(string city)
    {
        var url = "https://geocoding-api.open-meteo.com/v1/search?count=1&language=id&format=json&name="
                  + Uri.EscapeDataString(city.Trim());
        return ParseGeocode(await http.GetStringAsync(url).ConfigureAwait(false));
    }

    public async Task<WeatherData> GetCurrentAsync(double latitude, double longitude)
    {
        var url = string.Format(CultureInfo.InvariantCulture,
            "https://api.open-meteo.com/v1/forecast?latitude={0}&longitude={1}" +
            "&current=temperature_2m,relative_humidity_2m,precipitation,wind_speed_10m&timezone=auto",
            latitude, longitude);
        return ParseCurrent(await http.GetStringAsync(url).ConfigureAwait(false));
    }

    // public static supaya bisa diuji tanpa internet (lihat Swasana.Tests)
    public static (string Name, double Latitude, double Longitude)? ParseGeocode(string json)
    {
        using var doc = JsonDocument.Parse(json);
        if (!doc.RootElement.TryGetProperty("results", out var results) || results.GetArrayLength() == 0)
            return null;

        var first = results[0];
        var name = first.GetProperty("name").GetString() ?? "";
        if (first.TryGetProperty("admin1", out var admin1) && admin1.GetString() is { Length: > 0 } province)
            name += ", " + province;
        return (name, first.GetProperty("latitude").GetDouble(), first.GetProperty("longitude").GetDouble());
    }

    public static WeatherData ParseCurrent(string json)
    {
        using var doc = JsonDocument.Parse(json);
        var current = doc.RootElement.GetProperty("current");
        return new WeatherData
        {
            Temperature = current.GetProperty("temperature_2m").GetDouble(),
            Humidity = current.GetProperty("relative_humidity_2m").GetDouble(),
            Rainfall = current.GetProperty("precipitation").GetDouble(),
            WindSpeed = current.GetProperty("wind_speed_10m").GetDouble(),
            Timestamp = DateTime.Parse(current.GetProperty("time").GetString()!, CultureInfo.InvariantCulture)
        };
    }
}
