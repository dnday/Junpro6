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
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var username = tbUsername.Text.Trim();
            if (username.Length == 0)
            {
                MessageBox.Show("Username wajib diisi.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // ponytail: login cukup username (tabel users belum punya kolom password)
                var users = new UserRepository(Db);
                User = users.FindByUsername(username) ?? users.Insert(new User { Username = username });
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tidak bisa mengakses database:\n" + ex.Message +
                    "\n\nPeriksa lewat \"Pengaturan Database\".", "Login gagal",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lnkPengaturan_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using var setup = new FormKoneksi();
            if (setup.ShowDialog(this) == DialogResult.OK)
            {
                Db = new DatabaseContext(setup.ConnectionString);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
