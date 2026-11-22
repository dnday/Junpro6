using Swasana.Models;

namespace Swasana.Services;

// Induk seluruh aturan risiko. Tiap aturan menilai data cuaca dari sudut pandangnya
// sendiri (suhu saja, hujan saja, atau gabungan hujan dan angin).
public abstract class RiskRule
{
    // public: nama aturan boleh dibaca class lain untuk ditampilkan ke layar
    public string Nama { get; }

    // protected: constructor hanya bisa dipanggil oleh class turunannya
    protected RiskRule(string nama)
    {
        Nama = nama;
    }

    // Wajib ditulis ulang oleh setiap turunan -> inti polymorphism
    public abstract RiskLevel Evaluate(WeatherData data);

    // protected: pembanding bersama, dipakai turunan tapi tertutup dari luar
    protected static RiskLevel Klasifikasi(double nilai, double batasWaspada, double batasBahaya)
    {
        if (nilai > batasBahaya) return RiskLevel.Bahaya;
        if (nilai > batasWaspada) return RiskLevel.Waspada;
        return RiskLevel.Aman;
    }
}

// Suhu di atas 33 C -> Waspada, di atas 35 C -> Bahaya
public class SuhuRule : RiskRule
{
    private const double BatasWaspada = 33;   // C
    private const double BatasBahaya = 35;    // C

    public SuhuRule() : base("Suhu udara") { }

    public override RiskLevel Evaluate(WeatherData data)
    {
        return Klasifikasi(data.Temperature, BatasWaspada, BatasBahaya);
    }
}

// Hujan sedang (mulai 5 mm/jam) -> Waspada.
// Hujan saja tidak pernah langsung Bahaya, sesuai aturan pada Modul 1.
public class HujanRule : RiskRule
{
    private const double HujanSedang = 5;     // mm/jam

    public HujanRule() : base("Curah hujan") { }

    public override RiskLevel Evaluate(WeatherData data)
    {
        return data.Rainfall >= HujanSedang ? RiskLevel.Waspada : RiskLevel.Aman;
    }
}

// Hujan lebat yang disertai angin kencang -> Bahaya.
// Aturan ini membaca dua parameter sekaligus, jadi isi Evaluate()-nya berbeda
// dari dua aturan di atas walaupun dipanggil lewat method yang sama.
public class HujanAnginRule : RiskRule
{
    private const double HujanLebat = 10;     // mm/jam
    private const double AnginKencang = 45;   // km/jam

    public HujanAnginRule() : base("Hujan + angin") { }

    public override RiskLevel Evaluate(WeatherData data)
    {
        return data.Rainfall >= HujanLebat && data.WindSpeed >= AnginKencang
            ? RiskLevel.Bahaya
            : RiskLevel.Aman;
    }
}

// Indeks UV dari Open-Meteo: 8-10 "sangat tinggi" -> Waspada, 11+ "ekstrem" -> Bahaya (skala WHO)
public class UvRule : RiskRule
{
    private const double BatasWaspada = 8;
    private const double BatasBahaya = 11;

    public UvRule() : base("Indeks UV") { }

    public override RiskLevel Evaluate(WeatherData data)
    {
        if (data.UvIndex >= BatasBahaya) return RiskLevel.Bahaya;
        if (data.UvIndex >= BatasWaspada) return RiskLevel.Waspada;
        return RiskLevel.Aman;
    }
}

// Heat index: suhu yang dirasakan tubuh dari gabungan suhu dan kelembapan (rumus NOAA).
// 32-41 C "extreme caution" -> Waspada, 41 C ke atas "danger" -> Bahaya
public class HeatIndexRule : RiskRule
{
    private const double BatasWaspada = 32;   // C
    private const double BatasBahaya = 41;    // C

    public HeatIndexRule() : base("Heat index") { }

    public override RiskLevel Evaluate(WeatherData data)
    {
        return Klasifikasi(Hitung(data.Temperature, data.Humidity), BatasWaspada - 0.001, BatasBahaya - 0.001);
    }

    // Rumus NOAA (Rothfusz + penyesuaian), dihitung dalam Fahrenheit lalu dikembalikan ke Celsius
    public static double Hitung(double suhuC, double kelembapan)
    {
        var t = suhuC * 9 / 5 + 32;
        var rh = kelembapan;
        var hi = 0.5 * (t + 61 + (t - 68) * 1.2 + rh * 0.094);
        if ((hi + t) / 2 >= 80)
        {
            hi = -42.379 + 2.04901523 * t + 10.14333127 * rh - 0.22475541 * t * rh
                 - 0.00683783 * t * t - 0.05481717 * rh * rh + 0.00122874 * t * t * rh
                 + 0.00085282 * t * rh * rh - 0.00000199 * t * t * rh * rh;
            if (rh < 13 && t is >= 80 and <= 112)
                hi -= (13 - rh) / 4 * Math.Sqrt((17 - Math.Abs(t - 95)) / 17);
            else if (rh > 85 && t is >= 80 and <= 87)
                hi += (rh - 85) / 10 * ((87 - t) / 5);
        }
        return (hi - 32) * 5 / 9;
    }
}

// Kualitas udara (US AQI): 101-150 tidak sehat bagi kelompok sensitif -> Waspada, 151+ tidak sehat -> Bahaya
public class KualitasUdaraRule : RiskRule
{
    public KualitasUdaraRule() : base("Kualitas udara") { }

    public override RiskLevel Evaluate(WeatherData data)
    {
        if (data.Aqi is not int aqi) return RiskLevel.Aman;   // data tidak tersedia -> tidak menaikkan risiko
        return Klasifikasi(aqi, 100, 150);
    }
}
