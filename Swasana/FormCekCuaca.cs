using System.Globalization;
using Swasana.Data;
using Swasana.Models;
using Swasana.Services;

namespace Swasana
{
    public partial class FormCekCuaca : Form
    {
        private readonly User user;
        private readonly WeatherService weatherService = new WeatherService();
        private readonly RiskEvaluator riskEvaluator = new RiskEvaluator();
        private readonly ActionRecommender recommender;
        private readonly WeatherHistoryManager history;
        private bool isiOtomatis;

        public FormCekCuaca(DatabaseContext db, User user)
        {
            InitializeComponent();
            this.user = user;
            recommender = new ActionRecommender(db);
            history = new WeatherHistoryManager(db);
        }

        private async void btnCariLokasi_Click(object sender, EventArgs e)
        {
            await Jalankan(CariKoordinat);
        }

        private async void btnCek_Click(object sender, EventArgs e)
        {
            await Jalankan(async () =>
            {
                // Koordinat kosong -> cari dulu dari nama lokasi
                if (tbLatitude.Text.Trim().Length == 0 || tbLongitude.Text.Trim().Length == 0)
                {
                    if (!await CariKoordinat()) return true;
                }

                if (!TryParseKoordinat(tbLatitude.Text, out var lat) || !TryParseKoordinat(tbLongitude.Text, out var lon)
                    || lat < -90 || lat > 90 || lon < -180 || lon > 180)
                {
                    MessageBox.Show("Latitude harus -90 s.d. 90 dan longitude -180 s.d. 180.", "Koordinat tidak valid",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }

                var lokasi = new Location { Name = tbLokasi.Text.Trim() };
                lokasi.setLocation(lat, lon);

                var data = await WeatherData.fetchFromAPI(lokasi);
                data.RiskLevel = riskEvaluator.evaluateRisk(data);
                TampilkanHasil(data);

                history.saveHistory(user, lokasi, data);
                return true;
            });
        }

        // Mengisi latitude & longitude dari nama lokasi lewat Open-Meteo Geocoding API
        private async Task<bool> CariKoordinat()
        {
            if (tbLokasi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Lokasi wajib diisi.", "Cek Cuaca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var hasil = await weatherService.FindCityAsync(tbLokasi.Text);
            if (hasil == null)
            {
                MessageBox.Show($"Lokasi \"{tbLokasi.Text.Trim()}\" tidak ditemukan.", "Cek Cuaca",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var (nama, lat, lon) = hasil.Value;
            isiOtomatis = true;
            tbLokasi.Text = nama;
            isiOtomatis = false;
            tbLatitude.Text = lat.ToString("0.####", CultureInfo.InvariantCulture);
            tbLongitude.Text = lon.ToString("0.####", CultureInfo.InvariantCulture);
            return true;
        }

        // Nama lokasi diganti pengguna -> koordinat lama tidak berlaku lagi
        private void tbLokasi_TextChanged(object sender, EventArgs e)
        {
            if (isiOtomatis) return;
            tbLatitude.Text = "";
            tbLongitude.Text = "";
        }

        // Terima koma maupun titik sebagai pemisah desimal
        private static bool TryParseKoordinat(string text, out double nilai)
        {
            return double.TryParse(text.Trim().Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out nilai);
        }

        private void TampilkanHasil(WeatherData data)
        {
            lblSuhu.Text = $"Suhu : {data.Temperature:0.0} °C";
            lblKelembapan.Text = $"Kelembapan : {data.Humidity:0} %";
            lblAngin.Text = $"Angin : {data.WindSpeed:0.0} km/jam";
            lblHujan.Text = $"Curah hujan : {data.Rainfall:0.0} mm/jam";

            lblStatus.Text = "STATUS : " + data.RiskLevel.ToString().ToUpper();
            lblStatus.BackColor = data.RiskLevel switch
            {
                RiskLevel.Bahaya => Color.FromArgb(198, 40, 40),
                RiskLevel.Waspada => Color.FromArgb(249, 168, 37),
                _ => Color.FromArgb(46, 125, 50)
            };

            lblRincian.Text = "Rincian aturan : " +
                string.Join(" | ", riskEvaluator.rincian(data).Select(r => $"{r.Nama} = {r.Level}"));
            lblRekomendasi.Text = "Rekomendasi :\n" +
                string.Join("\n", recommender.getRecommendation(data.RiskLevel).Select(s => "• " + s));
            lblSumber.Text = $"Sumber data : Open-Meteo, {data.Timestamp:dd/MM/yyyy HH:mm} waktu setempat";
        }

        // Kunci tombol selama proses berjalan dan tampilkan error jaringan/database dengan rapi
        private async Task Jalankan(Func<Task<bool>> aksi)
        {
            btnCek.Enabled = btnCariLokasi.Enabled = false;
            Cursor = Cursors.WaitCursor;
            try
            {
                await aksi();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show("Gagal mengambil data dari Open-Meteo. Periksa koneksi internet.\n" + ex.Message,
                    "Koneksi internet", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("Open-Meteo tidak merespons (timeout). Coba lagi.", "Koneksi internet",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCek.Enabled = btnCariLokasi.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
