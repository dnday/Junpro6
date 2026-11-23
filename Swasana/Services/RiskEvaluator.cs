using Swasana.Models;

namespace Swasana.Services;

public class RiskEvaluator
{
    // private + readonly: daftar aturan tidak bisa dibaca maupun diganti dari luar class
    private readonly List<RiskRule> rules;

    public RiskEvaluator()
    {
        rules = new List<RiskRule>
        {
            new HujanRule(),
            new HujanAnginRule(),
            new UvRule(),
            new HeatIndexRule(),
            new KualitasUdaraRule()
        };
    }

    // Risiko akhir = tingkat tertinggi di antara seluruh aturan.
    public RiskLevel evaluateRisk(WeatherData data)
    {
        var hasil = RiskLevel.Aman;

        foreach (var rule in rules)
        {
            // Baris ini menjalankan Evaluate() milik HujanRule, UvRule,
            // HeatIndexRule, dan seterusnya sesuai objek yang sedang diproses (polymorphism).
            var level = rule.Evaluate(data);
            if (level > hasil) hasil = level;
        }

        return hasil;
    }

    // Hasil penilaian tiap aturan, dipakai untuk menampilkan alasan di layar.
    public List<(string Nama, RiskLevel Level)> rincian(WeatherData data)
    {
        return rules.Select(rule => (rule.Nama, rule.Evaluate(data))).ToList();
    }
}
