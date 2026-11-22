using Npgsql;
using Swasana.Data;

namespace Swasana.Tests;

public class KoneksiTests
{
    [Fact]
    public void BacaUri_FormatSupabase_PlaceholderPasswordDiabaikan()
    {
        var b = DatabaseSetup.BacaUri(
            "postgresql://postgres.abcd:[YOUR-DATABASE-PASSWORD]@aws-1-ap-northeast-1.pooler.supabase.com:5432/postgres");
        Assert.NotNull(b);
        Assert.Equal("aws-1-ap-northeast-1.pooler.supabase.com", b.Host);
        Assert.Equal(5432, b.Port);
        Assert.Equal("postgres", b.Database);
        Assert.Equal("postgres.abcd", b.Username);
        Assert.True(string.IsNullOrEmpty(b.Password));
    }

    [Fact]
    public void BacaUri_PasswordDenganKarakterKhusus()
    {
        var b = DatabaseSetup.BacaUri("postgres://user:p%40ss%3Aw0rd@db.example.com/swasana_db");
        Assert.NotNull(b);
        Assert.Equal("p@ss:w0rd", b.Password);
        Assert.Equal(5432, b.Port);
        Assert.Equal("swasana_db", b.Database);
    }

    [Theory]
    [InlineData("localhost")]
    [InlineData("aws-1-ap-northeast-1.pooler.supabase.com:5432")]
    [InlineData("")]
    public void BacaUri_BukanUri_Null(string teks)
    {
        Assert.Null(DatabaseSetup.BacaUri(teks));
    }

    [Theory]
    [InlineData("localhost", SslMode.Prefer)]
    [InlineData("127.0.0.1", SslMode.Prefer)]
    [InlineData("aws-1-ap-northeast-1.pooler.supabase.com", SslMode.Require)]
    public void PilihSsl_OnlineWajibTerenkripsi(string host, SslMode harapan)
    {
        Assert.Equal(harapan, DatabaseSetup.PilihSsl(host));
    }
}
