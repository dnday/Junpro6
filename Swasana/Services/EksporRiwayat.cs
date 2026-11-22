using System.Globalization;
using System.Text;
using Swasana.Models;

namespace Swasana.Services;

public static class EksporRiwayat
{
    public static string BuatCsv(IEnumerable<SearchHistory> riwayat)
    {
        var csv = new StringBuilder("Status,Waktu,Lokasi,Suhu (°C),Hujan (mm/jam),Angin (km/jam)\r\n");
        foreach (var h in riwayat)
        {
            csv.Append(string.Join(',',
                h.RiskLevel,
                h.SearchedAt.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                Sel(h.CityName),
                h.Temperature.ToString("0.0", CultureInfo.InvariantCulture),
                h.Rainfall.ToString("0.0", CultureInfo.InvariantCulture),
                h.WindSpeed.ToString("0.0", CultureInfo.InvariantCulture)));
            csv.Append("\r\n");
        }
        return csv.ToString();
    }

    // Awalan = + - @ diberi tanda kutip tunggal supaya Excel tidak membacanya sebagai rumus
    private static string Sel(string teks)
    {
        if (teks.Length > 0 && "=+-@".Contains(teks[0])) teks = "'" + teks;
        return teks.IndexOfAny(new[] { ',', '"', '\r', '\n' }) >= 0 ? "\"" + teks.Replace("\"", "\"\"") + "\"" : teks;
    }
}
