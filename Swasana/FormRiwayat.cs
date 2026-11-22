using Swasana.Data;
using Swasana.Models;
using Swasana.Services;

namespace Swasana
{
    public partial class FormRiwayat : Form
    {
        private readonly WeatherHistoryManager history;
        private readonly User user;
        private List<SearchHistory> dataRiwayat = new List<SearchHistory>();

        public FormRiwayat(DatabaseContext db, User user)
        {
            InitializeComponent();
            history = new WeatherHistoryManager(db);
            this.user = user;
            Animasi.FadeIn(this);
            Animasi.Hover(btnCari, Color.FromArgb(28, 104, 124));
            Animasi.Hover(btnKembali, Color.FromArgb(231, 241, 244));
        }

        private void FormRiwayat_Load(object sender, EventArgs e)
        {
            try
            {
                dataRiwayat = history.getHistory(user);
            }
            catch (Exception ex)
            {
                lblKosong.Text = "Riwayat tidak bisa dibaca dari database: " + ex.Message;
            }
            TampilkanRiwayat("");
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            TampilkanRiwayat(tbCari.Text.Trim());
        }

        private void TampilkanRiwayat(string kataKunci)
        {
            dgvRiwayat.Rows.Clear();
            foreach (var h in dataRiwayat.Where(h => h.CityName.Contains(kataKunci, StringComparison.OrdinalIgnoreCase)))
            {
                var baris = dgvRiwayat.Rows[dgvRiwayat.Rows.Add(Ikon.Status(h.RiskLevel), h.RiskLevel.ToString(),
                    h.SearchedAt.ToString("dd/MM/yyyy HH:mm"), h.CityName,
                    h.Temperature.ToString("0.0"), h.Rainfall.ToString("0.0"), h.WindSpeed.ToString("0.0"))];
                baris.Cells[colStatus.Index].Style.ForeColor = h.RiskLevel == RiskLevel.Waspada
                    ? Color.FromArgb(150, 100, 0)   // kuning gelap supaya terbaca di latar putih
                    : Tema.WarnaStatus(h.RiskLevel);
                baris.Cells[colStatus.Index].Style.Font = new Font("Segoe UI Semibold", 9.75F);
            }

            var jumlah = dgvRiwayat.Rows.Count;
            lblJumlah.Text = $"{jumlah} data";
            lblKosong.Visible = jumlah == 0;
            if (jumlah == 0 && dataRiwayat.Count > 0)
                lblKosong.Text = $"Tidak ada riwayat untuk \"{kataKunci}\". Coba nama lokasi lain.";
            else if (jumlah == 0 && !lblKosong.Text.StartsWith("Riwayat tidak bisa"))
                lblKosong.Text = "Belum ada riwayat. Buka Cek cuaca, hasilnya otomatis tersimpan di sini.";
            lblKosong.BringToFront();
            TampilkanTren(kataKunci);
        }

        private void TampilkanTren(string kataKunci)
        {
            try
            {
                var judul = kataKunci.Length == 0 ? "Tren suhu harian, semua lokasi" : $"Tren suhu harian, lokasi \"{kataKunci}\"";
                grafikTren.Tampilkan(history.getTrenHarian(user, kataKunci), judul);
            }
            catch (Exception ex)
            {
                grafikTren.Tampilkan(new(), "Tren tidak bisa dibaca: " + ex.Message);
            }
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
