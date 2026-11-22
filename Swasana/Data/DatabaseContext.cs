using System.Data;
using Npgsql;

namespace Swasana.Data;

// Satu-satunya class yang tahu cara bicara ke PostgreSQL.
// Encapsulation: connectionString private, class lain hanya memakai method publiknya.
public class DatabaseContext
{
    private readonly string connectionString;

    public DatabaseContext(string connectionString)
    {
        this.connectionString = connectionString;
    }

    // Membuka koneksi baru ke swasana_db. Pemanggil wajib menutupnya (pakai `using`).
    public NpgsqlConnection connect()
    {
        var conn = new NpgsqlConnection(connectionString);
        conn.Open();
        return conn;
    }

    // SELECT -> DataSet. Nilai dari luar selalu lewat parameter (@nama), bukan digabung ke string SQL.
    public DataSet executeQuery(string sql, params NpgsqlParameter[] parameters)
    {
        using var conn = connect();
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddRange(parameters);

        var result = new DataSet();
        using var adapter = new NpgsqlDataAdapter(cmd);
        adapter.Fill(result);
        return result;
    }

    // INSERT ... RETURNING / SELECT satu nilai -> nilai kolom pertama baris pertama.
    public object? executeScalar(string sql, params NpgsqlParameter[] parameters)
    {
        using var conn = connect();
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddRange(parameters);
        return cmd.ExecuteScalar();
    }

    // UPDATE / DELETE -> jumlah baris yang terpengaruh.
    public int executeNonQuery(string sql, params NpgsqlParameter[] parameters)
    {
        using var conn = connect();
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddRange(parameters);
        return cmd.ExecuteNonQuery();
    }
}
