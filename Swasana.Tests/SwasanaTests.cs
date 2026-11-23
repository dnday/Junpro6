using Swasana.Models;
using Swasana.Services;

namespace Swasana.Tests;

public class RiskEvaluatorTests
{
    private readonly RiskEvaluator evaluator = new RiskEvaluator();

    [Theory]
    [InlineData(30, 0, 10, RiskLevel.Aman)]       // cerah, kering
    [InlineData(28, 9.9, 10, RiskLevel.Aman)]     // hujan sedang (BMKG 5-10 mm/jam) belum Waspada
    [InlineData(28, 10, 10, RiskLevel.Waspada)]   // hujan lebat (BMKG 10-20 mm/jam)
    [InlineData(28, 19.9, 20, RiskLevel.Waspada)] // masih hujan lebat
    [InlineData(28, 20, 20, RiskLevel.Bahaya)]    // mulai 20 mm/jam: cuaca ekstrem BMKG
    [InlineData(27, 15, 45, RiskLevel.Waspada)]   // angin tepat 45 km/jam belum "di atas 25 knot"
    [InlineData(27, 15, 46, RiskLevel.Bahaya)]    // hujan lebat + angin kencang
    public void EvaluateRisk_HujanDanAnginSesuaiBmkg(double suhu, double hujan, double angin, RiskLevel harapan)
    {
        var data = new WeatherData { Temperature = suhu, Humidity = 0, Rainfall = hujan, WindSpeed = angin };
        Assert.Equal(harapan, evaluator.evaluateRisk(data));
    }

    [Fact]
    public void Rincian_BerisiSemuaAturan()
    {
        var rincian = evaluator.rincian(new WeatherData { Temperature = 34, Humidity = 75 });
        Assert.Equal(6, rincian.Count);
        Assert.Contains(rincian, r => r.Nama == "Heat index" && r.Level == RiskLevel.Bahaya);
    }
}

public class WeatherServiceTests
{
    [Fact]
    public void ParseCurrent_MembacaJsonOpenMeteo()
    {
        const string json = """
            {"latitude":-7.75,"longitude":110.375,"current":{"time":"2026-11-22T13:00",
             "temperature_2m":31.4,"relative_humidity_2m":68,"precipitation":2.5,"wind_speed_10m":11.2,"weather_code":61,"is_day":0}}
            """;
        var data = WeatherService.ParseCurrent(json);
        Assert.Equal(31.4, data.Temperature);
        Assert.Equal(68, data.Humidity);
        Assert.Equal(2.5, data.Rainfall);
        Assert.Equal(11.2, data.WindSpeed);
        Assert.Equal(new DateTime(2026, 11, 22, 13, 0, 0), data.Timestamp);
        Assert.Equal(61, data.WeatherCode);
        Assert.False(data.IsDay);
    }

    [Fact]
    public void ParseGeocode_AmbilHasilPertama()
    {
        const string json = """
            {"results":[{"name":"Sleman","latitude":-7.7162,"longitude":110.3555,"admin1":"Daerah Istimewa Yogyakarta"}]}
            """;
        var hasil = WeatherService.ParseGeocode(json);
        Assert.NotNull(hasil);
        Assert.Equal("Sleman, Daerah Istimewa Yogyakarta", hasil.Value.Name);
        Assert.Equal(-7.7162, hasil.Value.Latitude);
    }

    [Fact]
    public void ParseSaran_IndonesiaDidahulukan()
    {
        const string json = """
            {"results":[
              {"name":"Yoga","latitude":-3.1,"longitude":29.9,"admin1":"Ngozi","country":"Burundi","country_code":"BI"},
              {"name":"DI Yogyakarta","latitude":-7.8,"longitude":110.4,"admin1":"Yogyakarta","country":"Indonesia","country_code":"ID"}]}
            """;
        var saran = WeatherService.ParseSaran(json);
        Assert.Equal(2, saran.Count);
        Assert.Equal("DI Yogyakarta, Yogyakarta", saran[0].NamaLengkap);
        Assert.Equal("Yogyakarta, Indonesia", saran[0].Keterangan);
        Assert.Equal("Yoga, Ngozi", saran[1].NamaLengkap);
    }

    [Fact]
    public void ParseSaran_TanpaHasil_ListKosong()
    {
        Assert.Empty(WeatherService.ParseSaran("""{"generationtime_ms":0.5}"""));
    }

    [Fact]
    public void ParseGeocode_TidakDitemukan_Null()
    {
        Assert.Null(WeatherService.ParseGeocode("""{"generationtime_ms":0.5}"""));
    }
}

public class IkonTests
{
    [Theory]
    [InlineData(0, true, "clear-day")]
    [InlineData(0, false, "clear-night")]
    [InlineData(2, true, "partly-cloudy-day")]
    [InlineData(61, true, "rain")]
    [InlineData(65, true, "extreme-rain")]
    [InlineData(95, true, "thunderstorms-rain")]
    [InlineData(-1, true, "not-available")]
    public void NamaCuaca_SesuaiKodeWmo(int kode, bool siang, string harapan)
    {
        Assert.Equal(harapan, Ikon.NamaCuaca(kode, siang));
    }

    [Fact]
    public void SemuaIkonTertanamDiAplikasi()
    {
        foreach (var nama in new[] { "clear-day", "rain", "humidity", "wind", "raindrop", "code-red" })
            Assert.NotNull(Ikon.Muat(nama));
    }
}

public class LocationTests
{
    [Fact]
    public void SetLocation_TolakKoordinatDiLuarRentang()
    {
        var loc = new Location();
        Assert.Throws<ArgumentOutOfRangeException>(() => loc.setLocation(91, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => loc.setLocation(0, 181));
    }
}
