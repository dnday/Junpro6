using Swasana.Data;
using Swasana.Models;

namespace Swasana
{
    public partial class FormLogin : Form
    {
        public DatabaseContext Db { get; private set; }
        public User? User { get; private set; }

        public FormLogin(DatabaseContext db)
        {
            InitializeComponent();
            Db = db;
            picLogo.Image = Ikon.Muat("partly-cloudy-day");
            Animasi.FadeIn(this);
            Animasi.Hover(btnLogin, Tema.HoverUtama);
            Animasi.Hover(btnClose, Tema.HoverSekunder);
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var username = tbUsername.Text.Trim();
            if (username.Length == 0)
            {
                lblPesan.Text = "Isi username dulu untuk masuk.";
                tbUsername.Focus();
                return;
            }

            lblPesan.Text = "";
            Cursor = Cursors.WaitCursor;
            btnLogin.Enabled = false;
            try
            {
                // ponytail: login cukup username (tabel users belum punya kolom password)
                var users = new UserRepository(Db);
                var ditemukan = await Task.Run(() => users.FindByUsername(username));
                if (ditemukan == null)
                {
                    var jawaban = MessageBox.Show(this,
                        $"Username \"{username}\" belum terdaftar. Buat akun baru dengan username ini?",
                        "Akun baru", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (jawaban != DialogResult.Yes)
                    {
                        tbUsername.Focus();
                        tbUsername.SelectAll();
                        return;
                    }
                    ditemukan = await Task.Run(() => users.Insert(new User { Username = username }));
                }
                User = ditemukan;
                DialogResult = DialogResult.OK;
            }
            catch (Exception)
            {
                lblPesan.Text = "Database tidak bisa diakses. Periksa lewat \"Atur koneksi database\".";
            }
            finally
            {
                Cursor = Cursors.Default;
                btnLogin.Enabled = true;
            }
        }

        private void lnkPengaturan_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using var setup = new FormKoneksi();
            if (setup.ShowDialog(this) == DialogResult.OK)
            {
                Db = new DatabaseContext(setup.ConnectionString);
                lblPesan.Text = "";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
