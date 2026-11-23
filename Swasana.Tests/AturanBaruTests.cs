using Swasana.Models;
using Swasana.Services;

namespace Swasana.Tests;

public class AturanBaruTests
{
    [Theory]
    [InlineData(5, RiskLevel.Aman)]
    [InlineData(8, RiskLevel.Waspada)]
    [InlineData(11, RiskLevel.Bahaya)]
    public void UvRule_SkalaWho(double uv, RiskLevel harapan)
    {
        Assert.Equal(harapan, new UvRule().Evaluate(new WeatherData { UvIndex = uv }));
    }

    [Theory]
    [InlineData(32.0, 70, 40.0)]   // tabel NOAA: 90 F / 70% -> 106 F (41 C)
    [InlineData(26.7, 40, 26.7)]   // udara sejuk-kering: kira-kira sama dengan suhu
    public void HeatIndex_SesuaiTabelNoaa(double suhu, double rh, double harapan)
    {
        Assert.InRange(HeatIndexRule.Hitung(suhu, rh), harapan - 1.2, harapan + 1.2);
    }

    [Theory]
    [InlineData(28, 50, RiskLevel.Aman)]      // HI sekitar 28 C
    [InlineData(31, 70, RiskLevel.Waspada)]   // HI sekitar 37 C
    [InlineData(34, 75, RiskLevel.Bahaya)]    // HI di atas 41 C
    public void HeatIndexRule_Klasifikasi(double suhu, double rh, RiskLevel harapan)
    {
        Assert.Equal(harapan, new HeatIndexRule().Evaluate(new WeatherData { Temperature = suhu, Humidity = rh }));
    }

    [Theory]
    [InlineData(null, RiskLevel.Aman)]   // data tidak tersedia tidak menaikkan risiko
    [InlineData(80, RiskLevel.Aman)]
    [InlineData(125, RiskLevel.Waspada)]
    [InlineData(180, RiskLevel.Bahaya)]
    public void KualitasUdaraRule_UsAqi(int? aqi, RiskLevel harapan)
    {
        Assert.Equal(harapan, new KualitasUdaraRule().Evaluate(new WeatherData { Aqi = aqi }));
    }

    [Fact]
    public void RiskEvaluator_MemakaiLimaAturan()
    {
        var data = new WeatherData { Temperature = 28, Humidity = 50, Aqi = 125 };
        var evaluator = new RiskEvaluator();
        Assert.Equal(5, evaluator.rincian(data).Count);
        Assert.Equal(RiskLevel.Waspada, evaluator.evaluateRisk(data));   // naik karena kualitas udara saja
    }

    [Fact]
    public void ParseAirQuality_MembacaAqiDanPm25()
    {
        var (aqi, pm) = WeatherService.ParseAirQuality("""{"current":{"time":"2026-11-22T08:00","us_aqi":125,"pm2_5":16.0}}""");
        Assert.Equal(125, aqi);
        Assert.Equal(16.0, pm);
    }

    [Fact]
    public void ParseCurrent_MembacaPrakiraanHarian()
    {
        const string json = """
            {"current":{"time":"2026-11-22T13:00","temperature_2m":30,"relative_humidity_2m":60,"precipitation":0,"wind_speed_10m":5},
             "daily":{"time":["2026-11-22","2026-11-23"],"weather_code":[81,95],
                      "temperature_2m_max":[29.3,29.6],"temperature_2m_min":[23.7,23.6],"precipitation_probability_max":[100,null]}}
            """;
        var data = WeatherService.ParseCurrent(json);
        Assert.Equal(2, data.Daily.Count);
        Assert.Equal(29.6, data.Daily[1].SuhuMaks);
        Assert.Equal(95, data.Daily[1].KodeCuaca);
        Assert.Equal(0, data.Daily[1].PeluangHujan);   // null dari API dianggap 0
    }
}
