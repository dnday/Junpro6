using System.ComponentModel;
using System.Text;
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
        private readonly System.Windows.Forms.Timer timerTren = new System.Windows.Forms.Timer { Interval = 300 };
        private bool gagalMuat;
        private int versiTren;

        public FormRiwayat(DatabaseContext db, User user)
        {
            InitializeComponent();
            Icon = Tema.IkonAplikasi;
            history = new WeatherHistoryManager(db);
            this.user = user;
            Animasi.FadeIn(this);
            Animasi.Hover(btnCari, Tema.HoverUtama);
            Animasi.Hover(btnKembali, Tema.HoverSekunder);
            Animasi.Hover(btnEkspor, Tema.HoverSekunder);
            tbCari.TextChanged += tbCari_TextChanged;
            timerTren.Tick += timerTren_Tick;
            FormClosed += (s, e) => { timerTren.Stop(); timerTren.Dispose(); };
        }

        private async void FormRiwayat_Load(object sender, EventArgs e)
        {
            try
            {
                dataRiwayat = await Task.Run(() => history.getHistory(user));
            }
            catch (Exception)
            {
                gagalMuat = true;
                lblKosong.Text = "Riwayat tidak bisa dibaca dari database. Periksa koneksi database, lalu buka ulang halaman ini.";
            }
            if (IsDisposed) return;
            TampilkanRiwayat();
            _ = TampilkanTrenAsync(tbCari.Text.Trim());
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            timerTren.Stop();
            TampilkanRiwayat();
            _ = TampilkanTrenAsync(tbCari.Text.Trim());
        }

        private void tbCari_TextChanged(object? sender, EventArgs e)
        {
            TampilkanRiwayat();
            timerTren.Stop();
            timerTren.Start();
        }

        private void timerTren_Tick(object? sender, EventArgs e)
        {
            timerTren.Stop();
            _ = TampilkanTrenAsync(tbCari.Text.Trim());
        }

        private void cbStatus_SelectedIndexChanged(object? sender, EventArgs e)
        {
            TampilkanRiwayat();
        }

        private IEnumerable<SearchHistory> Tersaring(string kataKunci)
        {
            var hasil = dataRiwayat.Where(h => h.CityName.Contains(kataKunci, StringComparison.OrdinalIgnoreCase));
            if (cbStatus.SelectedIndex > 0)
            {
                var status = Enum.Parse<RiskLevel>(cbStatus.Text);
                hasil = hasil.Where(h => h.RiskLevel == status);
            }
            return hasil;
        }

        private int jumlahTampil;
        private System.Windows.Forms.Timer? timerBaris;

        // Baris tabel muncul satu per satu dari atas (maksimal 25 baris pertama, sisanya langsung tampil)
        private void BarisBergiliran()
        {
            timerBaris?.Dispose();
            timerBaris = null;
            if (!Animasi.Aktif || dgvRiwayat.Rows.Count == 0) return;
            dgvRiwayat.CurrentCell = null;
            var jumlah = Math.Min(dgvRiwayat.Rows.Count, 25);
            for (int i = 1; i < jumlah; i++) dgvRiwayat.Rows[i].Visible = false;
            var berikut = 1;
            timerBaris = new System.Windows.Forms.Timer { Interval = 28 };
            timerBaris.Tick += (s, e) =>
            {
                if (berikut < jumlah && berikut < dgvRiwayat.Rows.Count) dgvRiwayat.Rows[berikut++].Visible = true;
                else { timerBaris?.Dispose(); timerBaris = null; }
            };
            timerBaris.Start();
        }

        private void TampilkanRiwayat()
        {
            timerBaris?.Dispose();
            timerBaris = null;
            var kataKunci = tbCari.Text.Trim();
            var kolomUrut = dgvRiwayat.SortedColumn;
            var arahUrut = dgvRiwayat.SortOrder;

            dgvRiwayat.Rows.Clear();
            foreach (var h in Tersaring(kataKunci))
            {
                var baris = dgvRiwayat.Rows[dgvRiwayat.Rows.Add(Ikon.Status(h.RiskLevel), h.RiskLevel.ToString(),
                    h.SearchedAt.ToString("dd/MM/yyyy HH:mm"), h.CityName,
                    h.Temperature.ToString("0.0"), h.Rainfall.ToString("0.0"), h.WindSpeed.ToString("0.0"))];
                baris.Tag = h;
                baris.Cells[colStatus.Index].Style.ForeColor = h.RiskLevel == RiskLevel.Waspada
                    ? Tema.WaspadaGelap
                    : Tema.WarnaStatus(h.RiskLevel);
                baris.Cells[colStatus.Index].Style.Font = Tema.HurufTebal;
            }
            if (kolomUrut != null && arahUrut != SortOrder.None)
                dgvRiwayat.Sort(kolomUrut, arahUrut == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);

            var jumlah = dgvRiwayat.Rows.Count;
            Animasi.Hitung(lblJumlah, jumlahTampil, jumlah, v => $"{v:0} data", $"{jumlah} data", 400);
            jumlahTampil = jumlah;
            BarisBergiliran();
            btnEkspor.Enabled = jumlah > 0;
            lblKosong.Visible = jumlah == 0;
            if (jumlah == 0 && !gagalMuat)
            {
                var sebab = kataKunci.Length > 0 ? $"untuk \"{kataKunci}\"" : "dengan status ini";
                lblKosong.Text = dataRiwayat.Count > 0
                    ? $"Tidak ada riwayat {sebab}. Ubah kata kunci atau filter status."
                    : "Belum ada riwayat. Buka Cek cuaca, hasilnya otomatis tersimpan di sini.";
            }
            lblKosong.BringToFront();
        }

        private void dgvRiwayat_SortCompare(object? sender, DataGridViewSortCompareEventArgs e)
        {
            if (dgvRiwayat.Rows[e.RowIndex1].Tag is not SearchHistory a || dgvRiwayat.Rows[e.RowIndex2].Tag is not SearchHistory b) return;
            var kolom = e.Column;
            e.SortResult = kolom == colStatus ? a.RiskLevel.CompareTo(b.RiskLevel)
                : kolom == colTanggal ? a.SearchedAt.CompareTo(b.SearchedAt)
                : kolom == colLokasi ? string.Compare(a.CityName, b.CityName, StringComparison.CurrentCultureIgnoreCase)
                : kolom == colSuhu ? a.Temperature.CompareTo(b.Temperature)
                : kolom == colHujan ? a.Rainfall.CompareTo(b.Rainfall)
                : kolom == colAngin ? a.WindSpeed.CompareTo(b.WindSpeed)
                : 0;
            if (e.SortResult == 0) e.SortResult = b.SearchedAt.CompareTo(a.SearchedAt);
            e.Handled = true;
        }

        private async Task TampilkanTrenAsync(string kataKunci)
        {
            var versi = ++versiTren;
            try
            {
                var judul = kataKunci.Length == 0 ? "Tren suhu harian, semua lokasi" : $"Tren suhu harian, lokasi \"{kataKunci}\"";
                var tren = await Task.Run(() => history.getTrenHarian(user, kataKunci));
                if (IsDisposed || versi != versiTren) return;
                grafikTren.Tampilkan(tren, judul);
            }
            catch (Exception)
            {
                if (IsDisposed || versi != versiTren) return;
                grafikTren.Tampilkan(new(), "Tren tidak bisa dimuat dari database.");
            }
        }

        private void btnEkspor_Click(object sender, EventArgs e)
        {
            using var dialog = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                DefaultExt = "csv",
                FileName = $"riwayat-cuaca-{DateTime.Now:yyyyMMdd}.csv"
            };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            var baris = dgvRiwayat.Rows.Cast<DataGridViewRow>().Select(r => (SearchHistory)r.Tag!);
            try
            {
                File.WriteAllText(dialog.FileName, EksporRiwayat.BuatCsv(baris), new UTF8Encoding(true));
                MessageBox.Show(this, "Riwayat disimpan ke:\n" + dialog.FileName, "Ekspor CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                MessageBox.Show(this, "File tidak bisa disimpan. Pastikan lokasinya bisa ditulis dan file tidak sedang terbuka di aplikasi lain.",
                    "Ekspor CSV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
