using System.Data;
using Npgsql;
using Swasana.Data;
using Swasana.Models;

namespace Swasana.Services;

public class ActionRecommender
{
    private readonly DatabaseContext context;

    // Satu level risiko bisa punya banyak saran (relasi 1:N di tabel recommendations).
    // Diisi dari database saat pertama kali diminta, lalu disimpan di sini.
    private readonly Dictionary<RiskLevel, List<(string Kategori, string Teks)>> recommendationRules =
        new Dictionary<RiskLevel, List<(string Kategori, string Teks)>>();

    public ActionRecommender(DatabaseContext context)
    {
        this.context = context;
    }

    public List<string> getRecommendation(RiskLevel level)
    {
        return getRecommendationDetail(level).Select(r => r.Teks).ToList();
    }

    // Saran beserta kategorinya (aktivitas, kesehatan, perlindungan diri) untuk ditampilkan sebagai kartu
    public List<(string Kategori, string Teks)> getRecommendationDetail(RiskLevel level)
    {
        if (!recommendationRules.TryGetValue(level, out var saran))
        {
            var ds = context.executeQuery(
                "SELECT category, action_text FROM recommendations WHERE risk_id = @risk ORDER BY rec_id",
                new NpgsqlParameter("risk", (short)level));
            saran = ds.Tables[0].Rows.Cast<DataRow>()
                .Select(r => (Convert.ToString(r["category"]) ?? "", Convert.ToString(r["action_text"]) ?? ""))
                .ToList();
            recommendationRules[level] = saran;
        }
        return saran;
    }
}
