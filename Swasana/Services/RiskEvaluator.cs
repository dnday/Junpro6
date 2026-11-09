using Swasana.Models;

namespace Swasana.Services;

public class RiskEvaluator
{
    // Sama dengan isi seed risk_levels. Hujan & angin mengikuti kategori BMKG per jam.
    private double suhuWaspada = 33;
    private double suhuBahaya = 35;
    private double hujanSedang = 5;    // mm/jam
    private double hujanLebat = 10;    // mm/jam
    private double anginKencang = 45;  // km/jam (~25 knot)

    public RiskLevel evaluateRisk(WeatherData data)
    {
        if (data.Temperature > suhuBahaya ||
            (data.Rainfall >= hujanLebat && data.WindSpeed >= anginKencang))
            return RiskLevel.Bahaya;

        if (data.Temperature > suhuWaspada || data.Rainfall >= hujanSedang)
            return RiskLevel.Waspada;

        return RiskLevel.Aman;
    }
}
