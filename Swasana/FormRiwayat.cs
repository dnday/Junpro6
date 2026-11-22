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
        }

        private void FormRiwayat_Load(object sender, EventArgs e)
        {
            try
            {
                dataRiwayat = history.getHistory(user);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal membaca riwayat dari database:\n" + ex.Message, "Riwayat",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                dgvRiwayat.Rows.Add(h.SearchedAt.ToString("dd/MM/yyyy HH:mm"), h.CityName,
                    h.Temperature.ToString("0.0"), h.Rainfall.ToString("0.0"), h.RiskLevel.ToString());
            }
            lblJumlah.Text = $"{dgvRiwayat.Rows.Count} data";
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
