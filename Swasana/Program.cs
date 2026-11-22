using Swasana.Data;

namespace Swasana;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Belum ada pengaturan database -> minta dulu (sekali saja, lalu disimpan)
        var connectionString = DatabaseSetup.LoadConnectionString();
        if (connectionString == null)
        {
            using var setup = new FormKoneksi();
            if (setup.ShowDialog() != DialogResult.OK) return;
            connectionString = setup.ConnectionString;
        }

        var db = new DatabaseContext(connectionString);
        using var login = new FormLogin(db);
        if (login.ShowDialog() != DialogResult.OK || login.User == null) return;

        Application.Run(new FormUtama(login.Db, login.User));
    }
}
