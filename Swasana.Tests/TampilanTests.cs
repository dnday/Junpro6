using Swasana.Models;
using Swasana.Services;

namespace Swasana.Tests;

public class TampilanTests
{
    [Theory]
    [InlineData(50, "baik")]
    [InlineData(51, "sedang")]
    [InlineData(150, "sensitif")]
    [InlineData(200, "buruk")]
    [InlineData(201, "bahaya")]
    public void KategoriAqi_BatasUsAqi(int aqi, string harapan)
    {
        Assert.Equal(harapan, FormatCuaca.KategoriAqi(aqi));
    }

    [Theory]
    [InlineData(2.9, "rendah")]
    [InlineData(3, "sedang")]
    [InlineData(6, "tinggi")]
    [InlineData(8, "sangat tinggi")]
    [InlineData(11, "ekstrem")]
    public void KategoriUv_SkalaWho(double uv, string harapan)
    {
        Assert.Equal(harapan, FormatCuaca.KategoriUv(uv));
    }

    [Fact]
    public void NilaiAturan_MenampilkanNilaiYangDinilai()
    {
        var data = new WeatherData { Temperature = 31.5, Rainfall = 2, WindSpeed = 12, UvIndex = 7.2, Aqi = null };
        Assert.Equal("tidak tersedia", FormatCuaca.NilaiAturan("Kualitas udara", data));
        Assert.Equal("-", FormatCuaca.NilaiAturan("Aturan lain", data));
    }

    [Fact]
    public void WarnaLangit_MalamTidakDipengaruhiKodeCuaca()
    {
        Assert.Equal(Tema.WarnaLangit(0, false), Tema.WarnaLangit(95, false));
        Assert.NotEqual(Tema.WarnaLangit(0, true), Tema.WarnaLangit(61, true));
    }

    [Fact]
    public void BuatCsv_MemberiHeaderDanSatuBarisPerRiwayat()
    {
        var csv = EksporRiwayat.BuatCsv(new[]
        {
            new SearchHistory { RiskLevel = RiskLevel.Waspada, SearchedAt = new DateTime(2026, 11, 22, 8, 5, 0), CityName = "Sleman", Temperature = 31.25, Rainfall = 0, WindSpeed = 12.5 }
        });
        var baris = csv.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(2, baris.Length);
        Assert.Equal("Waspada,2026-11-22 08:05,Sleman,31.3,0.0,12.5", baris[1]);
    }

    [Theory]
    [InlineData("Yogyakarta, DIY", "\"Yogyakarta, DIY\"")]
    [InlineData("Kota \"A\"", "\"Kota \"\"A\"\"\"")]
    [InlineData("=HYPERLINK(1)", "'=HYPERLINK(1)")]
    public void BuatCsv_MengamankanNamaLokasi(string nama, string harapan)
    {
        var csv = EksporRiwayat.BuatCsv(new[] { new SearchHistory { CityName = nama, SearchedAt = DateTime.Now } });
        Assert.Contains("," + harapan + ",", csv);
    }
}
