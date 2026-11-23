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
            Icon = Tema.IkonAplikasi;
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
            tbHost.TextChanged += tbHost_TextChanged;

            // Exe yang membawa database bawaan: sediakan jalan pintas untuk kembali memakainya
            if (DatabaseSetup.KoneksiBawaan != null)
            {
                var bawaan = new LinkLabel
                {
                    Text = "Pakai database online bawaan", AutoSize = true, Font = Tema.HurufBesar,
                    LinkColor = Tema.Langit, ActiveLinkColor = Tema.Langit, Location = new Point(22, btnSimpan.Top + 8)
                };
                bawaan.LinkClicked += (s, e) =>
                {
                    DatabaseSetup.HapusPengaturan();
                    ConnectionString = DatabaseSetup.KoneksiBawaan;
                    DialogResult = DialogResult.OK;
                };
                Controls.Add(bawaan);
            }
        }

        // URI koneksi (mis. dari Supabase) yang ditempel ke kolom Host langsung dipecah ke tiap kolom
        private void tbHost_TextChanged(object? sender, EventArgs e)
        {
            var uri = DatabaseSetup.BacaUri(tbHost.Text);
            if (uri == null) return;
            tbHost.Text = uri.Host;
            tbPort.Text = uri.Port.ToString();
            tbDatabase.Text = uri.Database;
            tbUsername.Text = uri.Username;
            if (!string.IsNullOrEmpty(uri.Password)) tbPassword.Text = uri.Password;
            Pesan(string.IsNullOrEmpty(uri.Password) ? "Data koneksi sudah diisi. Tinggal ketik password database." : "Data koneksi sudah diisi. Klik Sambungkan.", error: false);
            tbPassword.Focus();
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
                Password = tbPassword.Text,
                SslMode = DatabaseSetup.PilihSsl(tbHost.Text.Trim())
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
                Pesan(builder.SslMode == SslMode.Require
                    ? "Password database salah. Untuk Supabase, pakai Database password (bisa di-reset di Project Settings > Database)."
                    : "Password PostgreSQL salah. Pakai password yang dibuat saat menginstal PostgreSQL.");
            }
            catch (PostgresException ex) when (ex.MessageText.Contains("Tenant or user not found"))
            {
                Pesan("Username tidak dikenali server. Untuk Supabase, formatnya postgres.<id-project>, lihat menu Connect di dashboard.");
            }
            catch (Exception ex) when (ex is NpgsqlException { InnerException: System.Net.Sockets.SocketException } or System.Net.Sockets.SocketException)
            {
                Pesan(builder.SslMode == SslMode.Require
                    ? "Server tidak bisa dihubungi. Periksa host, port, dan koneksi internet."
                    : "Tidak bisa tersambung ke PostgreSQL. Pastikan service PostgreSQL berjalan.");
            }
            catch (Exception ex)
            {
                Pesan("Tidak bisa tersambung ke database. (" + ex.Message + ")");
            }
            finally
            {
                btnSimpan.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void Pesan(string teks, bool error = true) => Tema.Pesan(lblPesan, teks, error);

        private void btnBatal_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
