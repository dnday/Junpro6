namespace Swasana.Models;

public class SearchHistory
{
    public long HistoryId { get; set; }
    public int UserId { get; set; }
    public long WeatherId { get; set; }
    public DateTime SearchedAt { get; set; }
    public string? Note { get; set; }

    // Diisi dari JOIN weather_data + locations, untuk ditampilkan di tabel riwayat
    public string CityName { get; set; } = "";
    public double Temperature { get; set; }
    public double Rainfall { get; set; }
    public double WindSpeed { get; set; }
    public RiskLevel RiskLevel { get; set; }
}
