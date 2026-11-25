using System.Data;
using Npgsql;
namespace Swasana.Data;
public abstract class BaseRepository<T>
{
    protected DatabaseContext Context { get; }
    protected abstract string TableName { get; }
    protected abstract string IdColumn { get; }
    protected BaseRepository(DatabaseContext context)
    {
        Context = context;
    }
    public abstract T Insert(T entity);
    protected abstract T MapRow(DataRow row);
    public List<T> GetAll()
    {
        var ds = Context.executeQuery($"SELECT * FROM {TableName} ORDER BY {IdColumn}");
        return MapRows(ds);
    }
    public T? GetById(int id)
    {
        var ds = Context.executeQuery(
            $"SELECT * FROM {TableName} WHERE {IdColumn} = @id",
            new NpgsqlParameter("id", id));
        return MapRows(ds).FirstOrDefault();
    }
    protected List<T> MapRows(DataSet ds)
    {
        return ds.Tables[0].Rows.Cast<DataRow>().Select(MapRow).ToList();
    }
}
