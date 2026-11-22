using System.Data;
using Npgsql;

namespace Swasana.Data;

// Induk semua repository (satu repository = satu tabel).
//  - Inheritance  : GetAll() & GetById() ditulis sekali di sini, diwarisi semua turunan.
//  - Polymorphism : Insert() & MapRow() abstract, tiap turunan meng-override sesuai tabelnya.
//  - Encapsulation: DatabaseContext disimpan protected, tidak bisa disentuh dari luar repository.
public abstract class BaseRepository<T>
{
    protected DatabaseContext Context { get; }

    protected abstract string TableName { get; }
    protected abstract string IdColumn { get; }

    protected BaseRepository(DatabaseContext context)
    {
        Context = context;
    }

    // Simpan entity ke tabel, kembalikan entity yang sudah berisi id dari database.
    public abstract T Insert(T entity);

    // Ubah satu baris hasil SELECT menjadi object.
    protected abstract T MapRow(DataRow row);

    public List<T> GetAll()
    {
        // TableName & IdColumn berasal dari kode (konstanta class turunan), bukan input user.
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
