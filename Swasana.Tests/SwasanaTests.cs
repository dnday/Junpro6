using Swasana.Models;
using Swasana.Services;

namespace Swasana.Tests;

public class RiskEvaluatorTests
{
    private readonly RiskEvaluator evaluator = new RiskEvaluator();

    [Theory]
    [InlineData(30, 0, 10, RiskLevel.Aman)]       // cerah
    [InlineData(33, 0, 10, RiskLevel.Aman)]       // tepat 33 C belum Waspada
    [InlineData(34, 0, 10, RiskLevel.Waspada)]    // panas > 33 C
    [InlineData(28, 5, 10, RiskLevel.Waspada)]    // hujan sedang
    [InlineData(28, 12, 20, RiskLevel.Waspada)]   // hujan lebat tanpa angin kencang
    [InlineData(36, 0, 10, RiskLevel.Bahaya)]     // panas > 35 C
    [InlineData(27, 15, 50, RiskLevel.Bahaya)]    // hujan lebat + angin kencang
    public void EvaluateRisk_MengikutiAturanModul1(double suhu, double hujan, double angin, RiskLevel harapan)
    {
        var data = new WeatherData { Temperature = suhu, Rainfall = hujan, WindSpeed = angin };
        Assert.Equal(harapan, evaluator.evaluateRisk(data));
    }

    [Fact]
    public void Rincian_BerisiSemuaAturan()
    {
        var rincian = evaluator.rincian(new WeatherData { Temperature = 36 });
        Assert.Equal(3, rincian.Count);
        Assert.Contains(rincian, r => r.Level == RiskLevel.Bahaya);
    }
}

public class WeatherServiceTests
{
    [Fact]
    public void ParseCurrent_MembacaJsonOpenMeteo()
    {
        const string json = """
            {"latitude":-7.75,"longitude":110.375,"current":{"time":"2026-11-22T13:00",
             "temperature_2m":31.4,"relative_humidity_2m":68,"precipitation":2.5,"wind_speed_10m":11.2}}
            """;
        var data = WeatherService.ParseCurrent(json);
        Assert.Equal(31.4, data.Temperature);
        Assert.Equal(68, data.Humidity);
        Assert.Equal(2.5, data.Rainfall);
        Assert.Equal(11.2, data.WindSpeed);
        Assert.Equal(new DateTime(2026, 11, 22, 13, 0, 0), data.Timestamp);
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
    public void ParseGeocode_TidakDitemukan_Null()
    {
        Assert.Null(WeatherService.ParseGeocode("""{"generationtime_ms":0.5}"""));
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
