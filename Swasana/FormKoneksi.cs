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
                MessageBox.Show("Host, port (angka), database, dan username wajib diisi.", "Data belum lengkap",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            Cursor = Cursors.WaitCursor;
            try
            {
                DatabaseSetup.EnsureDatabase(builder.ConnectionString);
                DatabaseSetup.SaveConnectionString(builder.ConnectionString);
                ConnectionString = builder.ConnectionString;
                MessageBox.Show($"Terhubung ke database '{builder.Database}'. Tabel sudah siap.", "Berhasil",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal terhubung ke PostgreSQL:\n" + ex.Message, "Koneksi gagal",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
