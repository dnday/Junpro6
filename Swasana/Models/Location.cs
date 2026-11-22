namespace Swasana.Models;

public class Location
{
    public int LocationId { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = "";          // kolom city_name

    // Encapsulation: koordinat hanya bisa diubah lewat setLocation() yang memvalidasi rentang
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public List<WeatherData> WeatherDataList { get; } = new List<WeatherData>();

    public (double, double) getCoordinates()
    {
        return (Latitude, Longitude);
    }

    public void setLocation(double lat, double lon)
    {
        if (lat < -90 || lat > 90) throw new ArgumentOutOfRangeException(nameof(lat));
        if (lon < -180 || lon > 180) throw new ArgumentOutOfRangeException(nameof(lon));
        Latitude = lat;
        Longitude = lon;
    }
}
