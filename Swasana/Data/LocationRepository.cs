using System.Data;
using Npgsql;
using Swasana.Models;

namespace Swasana.Data;

// Proses "simpan & ambil lokasi pantauan" milik user.
public class LocationRepository : BaseRepository<Location>
{
    public LocationRepository(DatabaseContext context) : base(context) { }

    protected override string TableName => "locations";
    protected override string IdColumn => "location_id";

    public override Location Insert(Location location)
    {
        var (lat, lon) = location.getCoordinates();

        var id = Context.executeScalar(
            @"INSERT INTO locations (user_id, city_name, latitude, longitude)
              VALUES (@userId, @cityName, @lat, @lon)
              RETURNING location_id",
            new NpgsqlParameter("userId", location.UserId),
            new NpgsqlParameter("cityName", location.Name),
            new NpgsqlParameter("lat", (decimal)lat),
            new NpgsqlParameter("lon", (decimal)lon));

        location.LocationId = Convert.ToInt32(id);
        return location;
    }

    public List<Location> GetByUser(int userId)
    {
        var ds = Context.executeQuery(
            "SELECT * FROM locations WHERE user_id = @userId ORDER BY location_id",
            new NpgsqlParameter("userId", userId));
        return MapRows(ds);
    }

    public Location? FindByUserAndCity(int userId, string cityName)
    {
        var ds = Context.executeQuery(
            "SELECT * FROM locations WHERE user_id = @userId AND city_name = @cityName LIMIT 1",
            new NpgsqlParameter("userId", userId),
            new NpgsqlParameter("cityName", cityName));
        return MapRows(ds).FirstOrDefault();
    }

    protected override Location MapRow(DataRow row)
    {
        var location = new Location
        {
            LocationId = Convert.ToInt32(row["location_id"]),
            UserId = Convert.ToInt32(row["user_id"]),
            Name = Convert.ToString(row["city_name"]) ?? ""
        };
        // Lewat setLocation() supaya validasi rentang koordinat tetap berlaku
        location.setLocation(Convert.ToDouble(row["latitude"]), Convert.ToDouble(row["longitude"]));
        return location;
    }
}
