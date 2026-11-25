using System.Data;
using Npgsql;
namespace Swasana.Data;

public class DatabaseContext
{
    private readonly string connectionString;
    public DatabaseContext(string connectionString)
    {
        this.connectionString = connectionString;
    }
    public NpgsqlConnection connect()
    {
        var conn = new NpgsqlConnection(connectionString);
        conn.Open();
        return conn;
    }
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
    public object? executeScalar(string sql, params NpgsqlParameter[] parameters)
    {
        using var conn = connect();
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddRange(parameters);
        return cmd.ExecuteScalar();
    }
    public int executeNonQuery(string sql, params NpgsqlParameter[] parameters)
    {
        using var conn = connect();
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddRange(parameters);
        return cmd.ExecuteNonQuery();
    }
}
