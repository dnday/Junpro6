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

    // Saran lokasi saat pengguna mengetik (autocomplete), lokasi Indonesia diutamakan
    public async Task<List<SaranLokasi>> CariSaranAsync(string kata, CancellationToken batal)
    {
        var url = "https://geocoding-api.open-meteo.com/v1/search?count=8&language=id&format=json&name="
                  + Uri.EscapeDataString(kata.Trim());
        return ParseSaran(await http.GetStringAsync(url, batal).ConfigureAwait(false));
    }

    // Nama tempat dari koordinat (untuk tombol "Lokasi saya"), memakai BigDataCloud tanpa API key
    public async Task<string?> NamaDariKoordinatAsync(double latitude, double longitude)
    {
        var url = string.Format(CultureInfo.InvariantCulture,
            "https://api.bigdatacloud.net/data/reverse-geocode-client?latitude={0}&longitude={1}&localityLanguage=id",
            latitude, longitude);
        using var doc = JsonDocument.Parse(await http.GetStringAsync(url).ConfigureAwait(false));
        string Ambil(string kunci) => doc.RootElement.TryGetProperty(kunci, out var v) ? v.GetString() ?? "" : "";
        var tempat = Ambil("locality").Length > 0 ? Ambil("locality") : Ambil("city");
        var wilayah = Ambil("city").Length > 0 && Ambil("city") != tempat ? Ambil("city") : Ambil("principalSubdivision");
        return tempat.Length == 0 ? null : wilayah.Length > 0 ? $"{tempat}, {wilayah}" : tempat;
    }

    public static List<SaranLokasi> ParseSaran(string json)
    {
        using var doc = JsonDocument.Parse(json);
        if (!doc.RootElement.TryGetProperty("results", out var results)) return new List<SaranLokasi>();

        return results.EnumerateArray().Select(r =>
        {
            string Ambil(string kunci) => r.TryGetProperty(kunci, out var v) ? v.GetString() ?? "" : "";
            return new SaranLokasi(Ambil("name"), Ambil("admin1"), Ambil("country"), Ambil("country_code"),
                r.GetProperty("latitude").GetDouble(), r.GetProperty("longitude").GetDouble());
        })
        .OrderBy(s => s.KodeNegara == "ID" ? 0 : 1)   // stabil: urutan relevansi dari API tetap terjaga
        .ToList();
    }

    public async Task<WeatherData> GetCurrentAsync(double latitude, double longitude)
    {
        var url = string.Format(CultureInfo.InvariantCulture,
            "https://api.open-meteo.com/v1/forecast?latitude={0}&longitude={1}" +
            "&current=temperature_2m,apparent_temperature,relative_humidity_2m,precipitation,wind_speed_10m,weather_code,is_day,uv_index" +
            "&hourly=temperature_2m,precipitation_probability,weather_code,is_day&forecast_hours=8&timezone=auto",
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
        var data = new WeatherData
        {
            Temperature = current.GetProperty("temperature_2m").GetDouble(),
            Humidity = current.GetProperty("relative_humidity_2m").GetDouble(),
            Rainfall = current.GetProperty("precipitation").GetDouble(),
            WindSpeed = current.GetProperty("wind_speed_10m").GetDouble(),
            Timestamp = DateTime.Parse(current.GetProperty("time").GetString()!, CultureInfo.InvariantCulture),
            WeatherCode = current.TryGetProperty("weather_code", out var kode) ? kode.GetInt32() : -1,
            IsDay = !current.TryGetProperty("is_day", out var siang) || siang.GetInt32() == 1,
            ApparentTemperature = current.TryGetProperty("apparent_temperature", out var terasa) ? terasa.GetDouble() : 0,
            UvIndex = current.TryGetProperty("uv_index", out var uv) && uv.ValueKind == JsonValueKind.Number ? uv.GetDouble() : 0
        };

        if (doc.RootElement.TryGetProperty("hourly", out var hourly))
        {
            var waktu = hourly.GetProperty("time");
            for (int i = 0; i < waktu.GetArrayLength(); i++)
            {
                data.Hourly.Add(new PrakiraanJam(
                    DateTime.Parse(waktu[i].GetString()!, CultureInfo.InvariantCulture),
                    hourly.GetProperty("temperature_2m")[i].GetDouble(),
                    hourly.GetProperty("precipitation_probability")[i].ValueKind == JsonValueKind.Number
                        ? hourly.GetProperty("precipitation_probability")[i].GetInt32() : 0,
                    hourly.GetProperty("weather_code")[i].GetInt32(),
                    hourly.GetProperty("is_day")[i].GetInt32() == 1));
            }
        }
        return data;
    }
}
