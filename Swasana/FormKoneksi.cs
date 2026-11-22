using Npgsql;
using Swasana.Data;

namespace Swasana
{
    public partial class FormKoneksi : Form
    {
        public string ConnectionString { get; private set; } = "";

        public FormKoneksi()
        {
            InitializeComponent();
            Animasi.FadeIn(this);
            Animasi.Hover(btnSimpan, Tema.HoverUtama);
            Animasi.Hover(btnBatal, Tema.HoverSekunder);

            // Tampilkan pengaturan lama (kecuali password) kalau sudah pernah disimpan
            var lama = DatabaseSetup.LoadConnectionString();
            if (lama != null)
            {
                var b = new NpgsqlConnectionStringBuilder(lama);
                tbHost.Text = b.Host;
                tbPort.Text = b.Port.ToString();
                tbDatabase.Text = b.Database;
                tbUsername.Text = b.Username;
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(tbPort.Text, out var port) || string.IsNullOrWhiteSpace(tbHost.Text)
                || string.IsNullOrWhiteSpace(tbDatabase.Text) || string.IsNullOrWhiteSpace(tbUsername.Text))
            {
                Pesan("Host, database, dan username wajib diisi, dan port harus berupa angka (biasanya 5432).");
                return;
            }

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = tbHost.Text.Trim(),
                Port = port,
                Database = tbDatabase.Text.Trim(),
                Username = tbUsername.Text.Trim(),
                Password = tbPassword.Text
            };

            Pesan("Menyambungkan dan menyiapkan tabel...", error: false);
            btnSimpan.Enabled = false;
            Cursor = Cursors.WaitCursor;
            Refresh();
            try
            {
                DatabaseSetup.EnsureDatabase(builder.ConnectionString);
                DatabaseSetup.SaveConnectionString(builder.ConnectionString);
                ConnectionString = builder.ConnectionString;
                DialogResult = DialogResult.OK;
            }
            catch (PostgresException ex) when (ex.SqlState == "28P01")
            {
                Pesan("Password PostgreSQL salah. Pakai password yang dibuat saat menginstal PostgreSQL.");
            }
            catch (Exception ex)
            {
                Pesan("Tidak bisa tersambung ke PostgreSQL. Pastikan service PostgreSQL berjalan. (" + ex.Message + ")");
            }
            finally
            {
                btnSimpan.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void Pesan(string teks, bool error = true)
        {
            lblPesan.ForeColor = error ? Tema.Galat : Tema.Redup;
            lblPesan.Text = teks;
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
