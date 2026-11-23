using Swasana.Models;

namespace Swasana.Services;

public static class FormatCuaca
{
    public static string KategoriAqi(int aqi) => aqi switch
    {
        <= 50 => "baik",
        <= 100 => "sedang",
        <= 150 => "sensitif",
        <= 200 => "buruk",
        _ => "bahaya"
    };

    public static string KategoriUv(double uv) => uv switch
    {
        < 3 => "rendah",
        < 6 => "sedang",
        < 8 => "tinggi",
        < 11 => "sangat tinggi",
        _ => "ekstrem"
    };

    // Nilai yang dinilai tiap aturan, supaya alasan status terbaca jelas
    public static string NilaiAturan(string nama, WeatherData d) => nama switch
    {
        "Curah hujan" => $"{d.Rainfall:0.#} mm/j",
        "Hujan + angin" => $"{d.Rainfall:0.#} mm/j, {d.WindSpeed:0.#} km/j",
        "Indeks UV" => $"{d.UvIndex:0.#}",
        "Heat index" => $"{HeatIndexRule.Hitung(d.Temperature, d.Humidity):0.#}°C",
        "Kualitas udara" => d.Aqi is int a ? $"AQI {a}" : "tidak tersedia",
        "Peringatan BMKG" => d.PeringatanBmkg is { } p ? $"{p.Kejadian} s.d. {p.Berakhir.ToLocalTime():HH:mm}" : "tidak ada",
        _ => "-"
    };
}
