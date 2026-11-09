using System.Data;
using Npgsql;
using Swasana.Models;

namespace Swasana.Data;

public class UserRepository : BaseRepository<User>
{
    public UserRepository(DatabaseContext context) : base(context) { }

    protected override string TableName => "users";
    protected override string IdColumn => "user_id";

    public override User Insert(User user)
    {
        var ds = Context.executeQuery(
            "INSERT INTO users (username) VALUES (@username) RETURNING user_id, created_at",
            new NpgsqlParameter("username", user.Username));

        var row = ds.Tables[0].Rows[0];
        user.UserId = Convert.ToInt32(row["user_id"]);
        user.CreatedAt = Convert.ToDateTime(row["created_at"]);
        return user;
    }

    public User? FindByUsername(string username)
    {
        var ds = Context.executeQuery(
            "SELECT * FROM users WHERE username = @username",
            new NpgsqlParameter("username", username));
        return MapRows(ds).FirstOrDefault();
    }

    protected override User MapRow(DataRow row)
    {
        return new User
        {
            UserId = Convert.ToInt32(row["user_id"]),
            Username = Convert.ToString(row["username"]) ?? "",
            CreatedAt = Convert.ToDateTime(row["created_at"])
        };
    }
}
