namespace Swasana.Models;

public class SearchHistory
{
    public long HistoryId { get; set; }
    public int UserId { get; set; }
    public long WeatherId { get; set; }
    public DateTime SearchedAt { get; set; }
    public string? Note { get; set; }
}
