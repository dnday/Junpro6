namespace Swasana.Models;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = "";
    public DateTime CreatedAt { get; set; }

    public List<Location> Locations { get; } = new List<Location>();
}
