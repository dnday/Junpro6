using Swasana.Data;
using Swasana.Models;

namespace Swasana.Services;

public class WeatherHistoryManager
{
    private DatabaseContext context;

    public WeatherHistoryManager(DatabaseContext context)
    {
        this.context = context;
    }

    public void saveHistory(User user, WeatherData data, string? note = null)
    {
        throw new NotImplementedException();
    }

    public List<SearchHistory> getHistory(User user)
    {
        throw new NotImplementedException();
    }
}
