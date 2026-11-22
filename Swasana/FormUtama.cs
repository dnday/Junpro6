using Swasana.Data;
using Swasana.Models;

namespace Swasana
{
    public partial class FormUtama : Form
    {
        private readonly DatabaseContext db;
        private readonly User user;

        public FormUtama(DatabaseContext db, User user)
        {
            InitializeComponent();
            this.db = db;
            this.user = user;
            lblUser.Text = "Halo, " + user.Username;
            picLogo.Image = Ikon.Muat("partly-cloudy-day");
            btnCekCuaca.Image = new Bitmap(Ikon.Muat("partly-cloudy-day"), 40, 40);
            btnRiwayat.Image = new Bitmap(Ikon.Muat("raindrop-measure"), 40, 40);
            Animasi.FadeIn(this);
            foreach (var b in new[] { btnCekCuaca, btnRiwayat, btnTentang, btnKeluar })
                Animasi.Hover(b, Color.FromArgb(231, 241, 244));
        }

        // Menu disembunyikan selama form tujuan terbuka, lalu tampil lagi saat ditutup
        private void BukaForm(Form form)
        {
            Hide();
            using (form) form.ShowDialog(this);
            Show();
        }

        private void btnCekCuaca_Click(object sender, EventArgs e)
        {
            BukaForm(new FormCekCuaca(db, user));
        }

        private void btnRiwayat_Click(object sender, EventArgs e)
        {
            BukaForm(new FormRiwayat(db, user));
        }

        private void btnTentang_Click(object sender, EventArgs e)
        {
            MessageBox.Show("SWASANA\nSistem Waspada Suasana & Adaptasi Iklim\n\n" +
                "Aplikasi pemantauan cuaca dan status risiko untuk membantu masyarakat beradaptasi dengan perubahan iklim.\n\n" +
                "Data cuaca: Open-Meteo Web API\nWindows Forms, .NET 10, PostgreSQL",
                "Tentang SWASANA", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnKeluar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
