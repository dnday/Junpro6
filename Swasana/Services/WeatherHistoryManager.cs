using System.Data;
using Npgsql;
using Swasana.Data;
using Swasana.Models;

namespace Swasana.Services;

public class WeatherHistoryManager
{
    private readonly DatabaseContext context;
    private readonly LocationRepository locations;

    public WeatherHistoryManager(DatabaseContext context)
    {
        this.context = context;
        locations = new LocationRepository(context);
    }

    // Simpan satu hasil cek cuaca: lokasi (dipakai ulang kalau sudah ada) -> weather_data -> search_history.
    public void saveHistory(User user, Location location, WeatherData data, string? note = null)
    {
        var saved = locations.FindByUserAndCity(user.UserId, location.Name);
        if (saved == null)
        {
            location.UserId = user.UserId;
            saved = locations.Insert(location);
        }

        var weatherId = context.executeScalar(
            @"INSERT INTO weather_data (location_id, risk_id, temperature, humidity, wind_speed, rainfall, recorded_at)
              VALUES (@loc, @risk, @temp, @hum, @wind, @rain, @time)
              RETURNING weather_id",
            new NpgsqlParameter("loc", saved.LocationId),
            new NpgsqlParameter("risk", (short)data.RiskLevel),
            new NpgsqlParameter("temp", (decimal)data.Temperature),
            new NpgsqlParameter("hum", (decimal)data.Humidity),
            new NpgsqlParameter("wind", (decimal)data.WindSpeed),
            new NpgsqlParameter("rain", (decimal)data.Rainfall),
            new NpgsqlParameter("time", data.Timestamp == default ? DateTime.Now : data.Timestamp));

        data.WeatherId = Convert.ToInt64(weatherId);
        data.LocationId = saved.LocationId;

        context.executeNonQuery(
            "INSERT INTO search_history (user_id, weather_id, note) VALUES (@user, @weather, @note)",
            new NpgsqlParameter("user", user.UserId),
            new NpgsqlParameter("weather", data.WeatherId),
            new NpgsqlParameter("note", (object?)note ?? DBNull.Value));
    }

    public List<SearchHistory> getHistory(User user)
    {
        var ds = context.executeQuery(
            @"SELECT h.history_id, h.user_id, h.weather_id, h.searched_at, h.note,
                     l.city_name, w.temperature, w.rainfall, w.wind_speed, w.risk_id
              FROM search_history h
              JOIN weather_data w ON w.weather_id = h.weather_id
              JOIN locations l    ON l.location_id = w.location_id
              WHERE h.user_id = @user
              ORDER BY h.searched_at DESC",
            new NpgsqlParameter("user", user.UserId));

        return ds.Tables[0].Rows.Cast<DataRow>().Select(row => new SearchHistory
        {
            HistoryId = Convert.ToInt64(row["history_id"]),
            UserId = Convert.ToInt32(row["user_id"]),
            WeatherId = Convert.ToInt64(row["weather_id"]),
            SearchedAt = Convert.ToDateTime(row["searched_at"]),
            Note = row["note"] as string,
            CityName = Convert.ToString(row["city_name"]) ?? "",
            Temperature = row["temperature"] is DBNull ? 0 : Convert.ToDouble(row["temperature"]),
            Rainfall = row["rainfall"] is DBNull ? 0 : Convert.ToDouble(row["rainfall"]),
            WindSpeed = row["wind_speed"] is DBNull ? 0 : Convert.ToDouble(row["wind_speed"]),
            RiskLevel = (RiskLevel)Convert.ToInt32(row["risk_id"])
        }).ToList();
    }
}
