namespace Swasana
{
    public partial class FormCekCuaca : Form
    {
        public FormCekCuaca()
        {
            InitializeComponent();
        }

        private void btnCek_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbLokasi.Text))
            {
                MessageBox.Show("Lokasi belum diisi.");
                return;
            }

            // ponytail: data contoh, nanti diganti hasil Open-Meteo API (WeatherData.fetchFromAPI)
            double suhu = 34.2, kelembapan = 70, angin = 12, hujan = 0.5;

            lblSuhu.Text = $"Suhu : {suhu} °C";
            lblKelembapan.Text = $"Kelembapan : {kelembapan} %";
            lblAngin.Text = $"Angin : {angin} km/jam";
            lblHujan.Text = $"Curah hujan : {hujan} mm/jam";

            // Aturan risiko dari W1 (sama dengan RiskEvaluator di classCode.cs)
            if (suhu > 35 || (hujan >= 10 && angin >= 45))
            {
                lblStatus.Text = "STATUS : BAHAYA";
                lblStatus.BackColor = Color.FromArgb(198, 40, 40);
                lblRekomendasi.Text = "Rekomendasi : Batasi aktivitas luar ruang pukul 11.00-14.00, " +
                    "tingkatkan konsumsi air minum, dan gunakan pelindung diri.";
            }
            else if (suhu > 33 || hujan >= 5)
            {
                lblStatus.Text = "STATUS : WASPADA";
                lblStatus.BackColor = Color.FromArgb(249, 168, 37);
                lblRekomendasi.Text = "Rekomendasi : Bawa air minum dan payung saat keluar rumah, " +
                    "kurangi aktivitas fisik berat di luar ruangan.";
            }
            else
            {
                lblStatus.Text = "STATUS : AMAN";
                lblStatus.BackColor = Color.FromArgb(46, 125, 50);
                lblRekomendasi.Text = "Rekomendasi : Cuaca aman untuk beraktivitas di luar ruangan.";
            }
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
