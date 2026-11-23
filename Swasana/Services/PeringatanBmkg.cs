using System.Globalization;
using System.Xml.Linq;
using Swasana.Models;

namespace Swasana.Services;

// Satu peringatan dini cuaca BMKG yang berlaku di lokasi yang dicek
public record PeringatanCuaca(string Kejadian, string Judul, string Keparahan, DateTimeOffset Berakhir);

// Peringatan dini cuaca BMKG (nowcast) dalam format CAP (Common Alerting Protocol), tanpa API key.
// RSS berisi daftar peringatan aktif; tiap peringatan punya polygon wilayah terdampak.
public static class PeringatanBmkg
{
    private const string UrlRss = "https://www.bmkg.go.id/alerts/nowcast/id";
    private static readonly HttpClient http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
    private static readonly XNamespace Cap = "urn:oasis:names:tc:emergency:cap:1.2";

    // ponytail: cache global 5 menit supaya cek beruntun tidak mengunduh ulang semua peringatan
    private static (DateTime Waktu, List<(PeringatanCuaca Info, List<List<(double Lat, double Lon)>> Wilayah)> Isi)? cache;

    // Peringatan BMKG paling parah yang berlaku di koordinat ini, null kalau tidak ada.
    // Di luar Indonesia langsung null tanpa mengakses internet.
    public static async Task<PeringatanCuaca?> CariAsync(double lat, double lon)
    {
        if (lat is < -11.5 or > 6.5 || lon is < 94 or > 141.5) return null;

        if (cache is not { } c || DateTime.UtcNow - c.Waktu > TimeSpan.FromMinutes(5))
        {
            var rss = XDocument.Parse(await http.GetStringAsync(UrlRss).ConfigureAwait(false));
            var tautan = rss.Descendants("item").Select(i => (string?)i.Element("link")).OfType<string>()
                            .Where(u => u.StartsWith("https://www.bmkg.go.id/alerts/", StringComparison.Ordinal));
            var isi = await Task.WhenAll(tautan.Select(u => http.GetStringAsync(u))).ConfigureAwait(false);
            cache = c = (DateTime.UtcNow, isi.Select(BacaCap).OfType<(PeringatanCuaca, List<List<(double, double)>>)>().ToList());
        }

        return Pilih(c.Isi, lat, lon, DateTimeOffset.Now);
    }

    public static PeringatanCuaca? Pilih(IEnumerable<(PeringatanCuaca Info, List<List<(double Lat, double Lon)>> Wilayah)> semua,
                                         double lat, double lon, DateTimeOffset sekarang)
    {
        return semua.Where(p => p.Info.Berakhir > sekarang && p.Wilayah.Any(w => DiDalam(w, lat, lon)))
                    .Select(p => p.Info)
                    .OrderByDescending(p => TingkatRisiko(p.Keparahan))
                    .FirstOrDefault();
    }

    // Satu dokumen CAP -> info peringatan + polygon wilayahnya
    public static (PeringatanCuaca Info, List<List<(double Lat, double Lon)>> Wilayah)? BacaCap(string xml)
    {
        var info = XDocument.Parse(xml).Descendants(Cap + "info").FirstOrDefault();
        if (info == null || !DateTimeOffset.TryParse((string?)info.Element(Cap + "expires"), CultureInfo.InvariantCulture, DateTimeStyles.None, out var berakhir))
            return null;

        var wilayah = info.Descendants(Cap + "polygon").Select(p => p.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Split(','))
            .Select(t => (double.Parse(t[0], CultureInfo.InvariantCulture), double.Parse(t[1], CultureInfo.InvariantCulture)))
            .ToList()).ToList();

        return (new PeringatanCuaca((string?)info.Element(Cap + "event") ?? "Peringatan dini",
                                    (string?)info.Element(Cap + "headline") ?? "",
                                    (string?)info.Element(Cap + "severity") ?? "Unknown",
                                    berakhir), wilayah);
    }

    // Tingkat keparahan CAP -> status SWASANA: Severe/Extreme -> Bahaya, lainnya -> Waspada
    public static RiskLevel TingkatRisiko(string keparahan) =>
        keparahan is "Severe" or "Extreme" ? RiskLevel.Bahaya : RiskLevel.Waspada;

    // Ray casting: titik di dalam polygon jika garis ke kanan memotong sisi polygon sebanyak ganjil kali
    public static bool DiDalam(List<(double Lat, double Lon)> poly, double lat, double lon)
    {
        var dalam = false;
        for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
        {
            var (yi, xi) = poly[i]; var (yj, xj) = poly[j];
            if ((yi > lat) != (yj > lat) && lon < (xj - xi) * (lat - yi) / (yj - yi) + xi)
                dalam = !dalam;
        }
        return dalam;
    }
}
