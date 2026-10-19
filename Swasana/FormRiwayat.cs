namespace Swasana
{
    public partial class FormRiwayat : Form
    {
        // ponytail: data contoh, nanti diambil dari tabel search_history + weather_data
        private readonly string[][] dataRiwayat =
        {
            new[] { "19/10/2026 13:00", "Yogyakarta", "34.2", "0.5", "Waspada" },
            new[] { "18/10/2026 12:00", "Sleman", "31.0", "0.0", "Aman" },
            new[] { "17/10/2026 14:00", "Yogyakarta", "36.1", "0.0", "Bahaya" },
            new[] { "16/10/2026 16:00", "Bantul", "28.4", "12.3", "Waspada" },
            new[] { "15/10/2026 10:00", "Sleman", "29.8", "1.2", "Aman" },
        };

        public FormRiwayat()
        {
            InitializeComponent();
        }

        private void FormRiwayat_Load(object sender, EventArgs e)
        {
            TampilkanRiwayat("");
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            TampilkanRiwayat(tbCari.Text);
        }

        private void TampilkanRiwayat(string kataKunci)
        {
            dgvRiwayat.Rows.Clear();
            foreach (string[] baris in dataRiwayat)
            {
                if (baris[1].Contains(kataKunci, StringComparison.OrdinalIgnoreCase))
                {
                    dgvRiwayat.Rows.Add(baris);
                }
            }
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
