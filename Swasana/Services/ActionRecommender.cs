using Swasana.Models;

namespace Swasana.Services;

public class ActionRecommender
{
    // Satu level risiko bisa punya banyak saran (relasi 1:N di tabel recommendations)
    private Dictionary<RiskLevel, List<string>> recommendationRules = new Dictionary<RiskLevel, List<string>>();

    public List<string> getRecommendation(RiskLevel level)
    {
        throw new NotImplementedException();
    }
}
