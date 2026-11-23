using Swasana.Models;
using Swasana.Services;

namespace Swasana.Tests;

public class PeringatanBmkgTests
{
    // Potongan dokumen CAP BMKG dengan satu polygon kotak di sekitar Bandar Lampung
    private const string Cap = """
        <?xml version="1.0" ?>
        <alert xmlns="urn:oasis:names:tc:emergency:cap:1.2">
          <info>
            <event>Hujan Lebat dan Petir</event>
            <severity>Moderate</severity>
            <expires>2026-11-23T11:30:00+07:00</expires>
            <headline>Hujan Lebat disertai Petir di Lampung</headline>
            <area><areaDesc>Lampung</areaDesc>
              <polygon>-5.50,105.20 -5.50,105.40 -5.30,105.40 -5.30,105.20 -5.50,105.20</polygon>
            </area>
          </info>
        </alert>
        """;

    [Fact]
    public void BacaCap_MembacaInfoDanPolygon()
    {
        var (info, wilayah) = PeringatanBmkg.BacaCap(Cap)!.Value;
        Assert.Equal("Hujan Lebat dan Petir", info.Kejadian);
        Assert.Equal("Moderate", info.Keparahan);
        Assert.Equal(new DateTimeOffset(2026, 11, 23, 11, 30, 0, TimeSpan.FromHours(7)), info.Berakhir);
        Assert.Single(wilayah);
        Assert.Equal(5, wilayah[0].Count);
    }

    [Theory]
    [InlineData(-5.40, 105.30, true)]    // di dalam kotak
    [InlineData(-5.40, 105.50, false)]   // di timur kotak
    [InlineData(-7.75, 110.37, false)]   // Yogyakarta
    public void DiDalam_RayCasting(double lat, double lon, bool harapan)
    {
        var wilayah = PeringatanBmkg.BacaCap(Cap)!.Value.Wilayah[0];
        Assert.Equal(harapan, PeringatanBmkg.DiDalam(wilayah, lat, lon));
    }

    [Fact]
    public void Pilih_HanyaPeringatanYangMasihBerlaku()
    {
        var semua = new[] { PeringatanBmkg.BacaCap(Cap)!.Value };
        var sebelum = new DateTimeOffset(2026, 11, 23, 10, 0, 0, TimeSpan.FromHours(7));
        Assert.NotNull(PeringatanBmkg.Pilih(semua, -5.40, 105.30, sebelum));
        Assert.Null(PeringatanBmkg.Pilih(semua, -5.40, 105.30, sebelum.AddHours(2)));   // sudah kedaluwarsa
        Assert.Null(PeringatanBmkg.Pilih(semua, -7.75, 110.37, sebelum));               // di luar wilayah
    }

    [Theory]
    [InlineData(null, RiskLevel.Aman)]
    [InlineData("Minor", RiskLevel.Waspada)]
    [InlineData("Moderate", RiskLevel.Waspada)]
    [InlineData("Severe", RiskLevel.Bahaya)]
    [InlineData("Extreme", RiskLevel.Bahaya)]
    public void PeringatanBmkgRule_KeparahanCap(string? keparahan, RiskLevel harapan)
    {
        var data = new WeatherData
        {
            PeringatanBmkg = keparahan == null ? null : new PeringatanCuaca("Hujan Lebat", "Hujan Lebat di Lampung", keparahan, DateTimeOffset.Now.AddHours(1))
        };
        Assert.Equal(harapan, new PeringatanBmkgRule().Evaluate(data));
    }

    [Fact]
    public async Task CariAsync_LuarIndonesiaTanpaInternet()
    {
        Assert.Null(await PeringatanBmkg.CariAsync(-23.7, 133.88));   // Alice Springs
    }
}
