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
