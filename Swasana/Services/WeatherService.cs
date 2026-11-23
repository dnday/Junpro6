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

    // Nama lokasi yang diketik (tanpa memilih saran) -> satu lokasi.
    // "Nama, Provinsi" dicocokkan dengan provinsinya. Kalau Open-Meteo tidak menemukan (misalnya nama
    // kecamatan seperti "Selagan Raya"), dicari lewat OpenStreetMap Nominatim yang lebih lengkap untuk Indonesia.
    public async Task<SaranLokasi?> FindCityAsync(string city)
    {
        var bagian = city.Split(',', 2);
        var nama = bagian[0].Trim();
        var petunjuk = bagian.Length > 1 ? bagian[1].Trim() : "";

        var url = "https://geocoding-api.open-meteo.com/v1/search?count=10&language=id&format=json&name="
                  + Uri.EscapeDataString(nama);
        var cocok = PilihHasil(ParseHasil(await http.GetStringAsync(url).ConfigureAwait(false)), petunjuk);
        if (cocok != null) return cocok;

        try
        {
            // Kebijakan Nominatim: User-Agent yang jelas dan hanya dipanggil saat pengguna menekan Cek cuaca
            using var permintaan = new HttpRequestMessage(HttpMethod.Get,
                "https://nominatim.openstreetmap.org/search?format=jsonv2&limit=1&addressdetails=1&accept-language=id&q="
                + Uri.EscapeDataString(city.Trim()));
            permintaan.Headers.UserAgent.ParseAdd("SWASANA/1.3 (aplikasi cuaca desktop)");
            using var jawaban = await http.SendAsync(permintaan).ConfigureAwait(false);
            if (!jawaban.IsSuccessStatusCode) return null;
            return ParseNominatim(await jawaban.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException) { return null; }
    }

    // Hasil pertama (urutan relevansi API), atau yang provinsi/negaranya cocok dengan petunjuk
    public static SaranLokasi? PilihHasil(List<SaranLokasi> hasil, string petunjuk) =>
        petunjuk.Length == 0
            ? hasil.FirstOrDefault()
            : hasil.FirstOrDefault(s => $"{s.Wilayah} {s.Negara}".Contains(petunjuk, StringComparison.OrdinalIgnoreCase)
                                        || s.KodeNegara.Equals(petunjuk, StringComparison.OrdinalIgnoreCase));

    public static SaranLokasi? ParseNominatim(string json)
    {
        using var doc = JsonDocument.Parse(json);
        if (doc.RootElement.ValueKind != JsonValueKind.Array || doc.RootElement.GetArrayLength() == 0) return null;
        var r = doc.RootElement[0];
        string Ambil(JsonElement e, string kunci) => e.TryGetProperty(kunci, out var v) ? v.GetString() ?? "" : "";
        var alamat = r.TryGetProperty("address", out var a) ? a : default;
        string DariAlamat(string kunci) => alamat.ValueKind == JsonValueKind.Object ? Ambil(alamat, kunci) : "";
        var nama = Ambil(r, "name");
        if (nama.Length == 0) nama = Ambil(r, "display_name").Split(',')[0];
        return new SaranLokasi(nama, DariAlamat("state"), DariAlamat("country"), DariAlamat("country_code").ToUpperInvariant(),
            double.Parse(Ambil(r, "lat"), CultureInfo.InvariantCulture), double.Parse(Ambil(r, "lon"), CultureInfo.InvariantCulture));
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

    // Saran untuk daftar: lokasi Indonesia didahulukan
    public static List<SaranLokasi> ParseSaran(string json) =>
        ParseHasil(json).OrderBy(s => s.KodeNegara == "ID" ? 0 : 1).ToList();   // stabil: urutan relevansi tetap terjaga

    public static List<SaranLokasi> ParseHasil(string json)
    {
        using var doc = JsonDocument.Parse(json);
        if (!doc.RootElement.TryGetProperty("results", out var results)) return new List<SaranLokasi>();

        return results.EnumerateArray().Select(r =>
        {
            string Ambil(string kunci) => r.TryGetProperty(kunci, out var v) ? v.GetString() ?? "" : "";
            return new SaranLokasi(Ambil("name"), Ambil("admin1"), Ambil("country"), Ambil("country_code"),
                r.GetProperty("latitude").GetDouble(), r.GetProperty("longitude").GetDouble());
        }).ToList();
    }

    public async Task<WeatherData> GetCurrentAsync(double latitude, double longitude)
    {
        var url = string.Format(CultureInfo.InvariantCulture,
            "https://api.open-meteo.com/v1/forecast?latitude={0}&longitude={1}" +
            "&current=temperature_2m,apparent_temperature,relative_humidity_2m,precipitation,wind_speed_10m,weather_code,is_day,uv_index" +
            "&hourly=temperature_2m,precipitation_probability,weather_code,is_day&forecast_hours=8" +
            "&daily=weather_code,temperature_2m_max,temperature_2m_min,precipitation_probability_max&forecast_days=7&timezone=auto",
            latitude, longitude);
        var urlUdara = string.Format(CultureInfo.InvariantCulture,
            "https://air-quality-api.open-meteo.com/v1/air-quality?latitude={0}&longitude={1}&current=us_aqi,pm2_5",
            latitude, longitude);

        // Cuaca dan kualitas udara diambil bersamaan; kualitas udara boleh gagal tanpa menggagalkan cuaca
        // Peringatan BMKG juga boleh gagal: penilaian lain tetap berjalan
        var tugasUdara = http.GetStringAsync(urlUdara);
        var tugasBmkg = PeringatanBmkg.CariAsync(latitude, longitude);
        var data = ParseCurrent(await http.GetStringAsync(url).ConfigureAwait(false));
        try
        {
            (data.Aqi, data.Pm25) = ParseAirQuality(await tugasUdara.ConfigureAwait(false));
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException) { }
        try
        {
            data.PeringatanBmkg = await tugasBmkg.ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or System.Xml.XmlException or FormatException or IndexOutOfRangeException) { }
        return data;
    }

    public static (int? Aqi, double? Pm25) ParseAirQuality(string json)
    {
        using var doc = JsonDocument.Parse(json);
        if (!doc.RootElement.TryGetProperty("current", out var c)) return (null, null);
        int? aqi = c.TryGetProperty("us_aqi", out var a) && a.ValueKind == JsonValueKind.Number ? a.GetInt32() : null;
        double? pm = c.TryGetProperty("pm2_5", out var p) && p.ValueKind == JsonValueKind.Number ? p.GetDouble() : null;
        return (aqi, pm);
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

        if (doc.RootElement.TryGetProperty("daily", out var daily))
        {
            var hari = daily.GetProperty("time");
            for (int i = 0; i < hari.GetArrayLength(); i++)
            {
                var peluang = daily.GetProperty("precipitation_probability_max")[i];
                data.Daily.Add(new PrakiraanHari(
                    DateTime.Parse(hari[i].GetString()!, CultureInfo.InvariantCulture),
                    daily.GetProperty("temperature_2m_max")[i].GetDouble(),
                    daily.GetProperty("temperature_2m_min")[i].GetDouble(),
                    peluang.ValueKind == JsonValueKind.Number ? peluang.GetInt32() : 0,
                    daily.GetProperty("weather_code")[i].GetInt32()));
            }
        }
        return data;
    }
}
