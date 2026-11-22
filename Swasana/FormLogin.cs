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
            Animasi.Hover(btnLogin, Color.FromArgb(28, 104, 124));
            Animasi.Hover(btnClose, Color.FromArgb(231, 241, 244));
        }

        private void btnLogin_Click(object sender, EventArgs e)
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
            try
            {
                // ponytail: login cukup username (tabel users belum punya kolom password)
                var users = new UserRepository(Db);
                User = users.FindByUsername(username) ?? users.Insert(new User { Username = username });
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                lblPesan.Text = "Database tidak bisa diakses. Periksa lewat \"Atur koneksi database\". (" + ex.Message + ")";
            }
            finally
            {
                Cursor = Cursors.Default;
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
