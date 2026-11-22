using System.ComponentModel;
using System.Drawing.Drawing2D;
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
        private readonly LocationRepository lokasiRepo;
        private List<Location> favorit = new List<Location>();
        private WeatherData? dataTerakhir;
        private bool tampilHarian;
        private bool isiOtomatis;
        private bool adeganBerjalan;
        private bool jendelaAktif = true;
        private readonly ToolTip tip = new ToolTip();

        private static readonly Font FontKecil = new Font("Segoe UI", 8.25F);
        private static readonly Font FontSedang = new Font("Segoe UI", 9F);
        private static readonly Font FontBesar = new Font("Segoe UI", 9.75F);
        private static readonly Font FontKategori = new Font("Segoe UI Semibold", 8.25F);
        private static readonly Font FontSaran = new Font("Segoe UI Semibold", 9.75F);
        private static readonly Font FontSuhuHarian = new Font("Bahnschrift SemiBold", 9F);
        private static readonly Font FontSuhuJam = new Font("Bahnschrift SemiBold", 10F);
        private static readonly Font FontBintang = new Font("Segoe UI Symbol", 12F);

        // Warna langit di latar hero, mengikuti kondisi cuaca (seperti aplikasi cuaca pada umumnya).
        // Public supaya bisa dianimasikan FluentTransitions.
        private Color langitAtas = Color.FromArgb(15, 76, 92), langitBawah = Color.FromArgb(40, 100, 116);
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color LangitAtas { get => langitAtas; set { langitAtas = value; pnlHero.Invalidate(); } }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color LangitBawah { get => langitBawah; set { langitBawah = value; pnlHero.Invalidate(); } }

        // Angka suhu yang tampil; dianimasikan naik/turun ke nilai sebenarnya
        private double suhuTampil;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double SuhuTampil { get => suhuTampil; set { suhuTampil = value; lblSuhu.Text = $"{value:0.#}°"; } }

        private readonly GarisLoading loading;
        private readonly int suhuTop;
        private readonly AdeganCuaca adegan = new AdeganCuaca();
        private readonly System.Windows.Forms.Timer timerAdegan = new System.Windows.Forms.Timer { Interval = 33 };   // ~30 fps
        private readonly System.Diagnostics.Stopwatch jamAdegan = System.Diagnostics.Stopwatch.StartNew();
        private double detikTerakhir;

        // Animasi kartu rekomendasi: muncul bergantian sambil bergeser dari kanan
        private readonly System.Windows.Forms.Timer timerKartu = new System.Windows.Forms.Timer { Interval = 15 };
        private readonly List<Panel> kartu = new List<Panel>();
        private int langkahAnimasi;

        public FormCekCuaca(DatabaseContext db, User user)
        {
            InitializeComponent();
            this.user = user;
            recommender = new ActionRecommender(db);
            history = new WeatherHistoryManager(db);
            lokasiRepo = new LocationRepository(db);

            picCuaca.Image = Ikon.Muat("partly-cloudy-day");
            picKelembapan.Image = Ikon.Muat("humidity");
            picAngin.Image = Ikon.Muat("wind");
            picHujan.Image = Ikon.Muat("raindrop");
            picUv.Image = Ikon.Muat("uv-index");
            picUdara.Image = Ikon.Muat("smoke-particles");
            btnFavorit.Font = FontBintang;
            foreach (var b in new[] { btnPerJam, btnHarian }) b.Font = FontKecil;
            tip.SetToolTip(btnFavorit, "Simpan atau hapus lokasi ini dari favorit");
            picStatus.Visible = false;
            timerKartu.Tick += timerKartu_Tick;
            Shown += async (s, e) => await MuatFavoritAsync();
            TampilkanKosong();

            loading = new GarisLoading(pnlHero, Color.White);
            // Panel cuaca digambar ulang ~30 kali per detik untuk adegan animasi -> pakai double buffering
            typeof(Panel).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(pnlHero, true);
            timerAdegan.Tick += (s, e) =>
            {
                var sekarang = jamAdegan.Elapsed.TotalSeconds;
                adegan.Tick(pnlHero.Size, Math.Min(sekarang - detikTerakhir, 0.1));
                detikTerakhir = sekarang;
                pnlHero.Invalidate();
            };
            FormClosed += (s, e) => { timerAdegan.Stop(); timerKartu.Stop(); timerSaran.Stop(); batalSaran?.Cancel(); batalSaran?.Dispose(); tip.Dispose(); };
            Activated += (s, e) => { jendelaAktif = true; AturTimerAdegan(); };
            Deactivate += (s, e) => { jendelaAktif = false; AturTimerAdegan(); };
            SizeChanged += (s, e) => AturTimerAdegan();
            suhuTop = lblSuhu.Top;
            Animasi.FadeIn(this);
            Animasi.Hover(btnCek, Color.FromArgb(28, 104, 124));
            Animasi.Hover(btnLokasiSaya, Color.FromArgb(231, 241, 244));
            timerSaran.Tick += timerSaran_Tick;
            lstSaran.BringToFront();
            Animasi.Hover(btnKembali, Color.FromArgb(231, 241, 244));
        }

        // Lokasi perangkat (GPS / Wi-Fi Windows), lalu nama tempatnya dicari dari koordinat
        private async void btnLokasiSaya_Click(object sender, EventArgs e)
        {
            await Jalankan("Mencari lokasi perangkat...", async () =>
            {
                var (lat, lon) = await LokasiPerangkat.AmbilAsync();
                var nama = await weatherService.NamaDariKoordinatAsync(lat, lon) ?? "Lokasi saya";
                IsiLokasi(nama, lat, lon);
                Pesan("Mengambil cuaca terkini...", error: false);
                return await AmbilCuaca();
            });
        }

        private async void btnCek_Click(object sender, EventArgs e)
        {
            await Jalankan("Mengambil cuaca terkini...", AmbilCuaca);
        }

        private async Task<bool> AmbilCuaca()
        {
            {
                // Koordinat kosong -> cari dulu dari nama lokasi
                if (tbLatitude.Text.Trim().Length == 0 || tbLongitude.Text.Trim().Length == 0)
                {
                    if (!await CariKoordinat()) return false;
                }

                if (!TryParseKoordinat(tbLatitude.Text, out var lat) || !TryParseKoordinat(tbLongitude.Text, out var lon)
                    || lat < -90 || lat > 90 || lon < -180 || lon > 180)
                {
                    Pesan("Latitude harus -90 s.d. 90 dan longitude -180 s.d. 180.");
                    return false;
                }

                var lokasi = new Location { Name = tbLokasi.Text.Trim() };
                lokasi.setLocation(lat, lon);

                var data = await WeatherData.fetchFromAPI(lokasi);
                data.RiskLevel = riskEvaluator.evaluateRisk(data);
                TampilkanHasil(lokasi.Name, data);

                await Task.Run(() => history.saveHistory(user, lokasi, data));
                return true;
            }
        }

        // Mengisi latitude & longitude dari nama lokasi lewat Open-Meteo Geocoding API
        private async Task<bool> CariKoordinat()
        {
            if (tbLokasi.Text.Trim().Length == 0)
            {
                Pesan("Isi nama lokasi dulu, misalnya Sleman.");
                tbLokasi.Focus();
                return false;
            }

            var hasil = await weatherService.FindCityAsync(tbLokasi.Text);
            if (hasil == null)
            {
                Pesan($"Lokasi \"{tbLokasi.Text.Trim()}\" tidak ditemukan. Periksa ejaannya atau isi koordinat sendiri.");
                return false;
            }

            var (nama, lat, lon) = hasil.Value;
            IsiLokasi(nama, lat, lon);
            return true;
        }

        private void IsiLokasi(string nama, double lat, double lon)
        {
            SembunyikanSaran();
            isiOtomatis = true;
            tbLokasi.Text = nama;
            tbLokasi.SelectionStart = tbLokasi.TextLength;
            isiOtomatis = false;
            tbLatitude.Text = lat.ToString("0.####", CultureInfo.InvariantCulture);
            tbLongitude.Text = lon.ToString("0.####", CultureInfo.InvariantCulture);
            PerbaruiBintang();
        }

        // ===== Saran lokasi saat mengetik (autocomplete) =====
        private readonly System.Windows.Forms.Timer timerSaran = new System.Windows.Forms.Timer { Interval = 300 };
        private CancellationTokenSource? batalSaran;

        // Nama lokasi diganti pengguna -> koordinat lama tidak berlaku lagi, dan saran dicari ulang
        private void tbLokasi_TextChanged(object sender, EventArgs e)
        {
            if (isiOtomatis) return;
            tbLatitude.Text = "";
            tbLongitude.Text = "";
            PerbaruiBintang();
            // Tunggu jeda mengetik 300 ms supaya tidak memanggil API di setiap huruf
            timerSaran.Stop();
            if (tbLokasi.Text.Trim().Length >= 2) timerSaran.Start(); else SembunyikanSaran();
        }

        private async void timerSaran_Tick(object? sender, EventArgs e)
        {
            timerSaran.Stop();
            batalSaran?.Cancel();
            batalSaran?.Dispose();
            batalSaran = new CancellationTokenSource();
            var kata = tbLokasi.Text;
            try
            {
                var saran = await weatherService.CariSaranAsync(kata, batalSaran.Token);
                // Abaikan hasil kalau pengguna sudah mengetik lagi, pindah ke kontrol lain, atau sedang mengecek cuaca
                if (kata != tbLokasi.Text || !tbLokasi.Focused || !btnCek.Enabled) return;
                lstSaran.BeginUpdate();
                lstSaran.Items.Clear();
                foreach (var s in saran) lstSaran.Items.Add(s);
                lstSaran.EndUpdate();
                lstSaran.Height = Math.Min(saran.Count, 5) * lstSaran.ItemHeight + 2;
                lstSaran.Visible = saran.Count > 0;
                if (saran.Count > 0) lstSaran.SelectedIndex = 0;
                AcceptButton = saran.Count > 0 ? null : btnCek;   // selama daftar terbuka, Enter = pilih saran
            }
            catch (OperationCanceledException) { }
            catch (HttpRequestException) { SembunyikanSaran(); }   // tanpa internet: diam saja, Cek cuaca tetap memberi pesan
        }

        // Memilih saran langsung menampilkan cuaca lokasi itu (seperti aplikasi cuaca pada umumnya)
        private void PilihSaran()
        {
            if (lstSaran.SelectedItem is not SaranLokasi s) return;
            IsiLokasi(s.NamaLengkap, s.Latitude, s.Longitude);
            tbLokasi.Focus();
            btnCek.PerformClick();
        }

        private void SembunyikanSaran()
        {
            timerSaran.Stop();
            lstSaran.Visible = false;
            AcceptButton = btnCek;
        }

        private void tbLokasi_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSaran.Visible) return;
            switch (e.KeyCode)
            {
                case Keys.Down: lstSaran.SelectedIndex = Math.Min(lstSaran.SelectedIndex + 1, lstSaran.Items.Count - 1); break;
                case Keys.Up: lstSaran.SelectedIndex = Math.Max(lstSaran.SelectedIndex - 1, 0); break;
                case Keys.Enter: PilihSaran(); break;
                case Keys.Escape: SembunyikanSaran(); break;
                default: return;
            }
            e.Handled = e.SuppressKeyPress = true;   // Enter tidak ikut menekan tombol Cek cuaca
        }

        private void lstSaran_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { PilihSaran(); e.Handled = e.SuppressKeyPress = true; }
            if (e.KeyCode == Keys.Escape) { SembunyikanSaran(); tbLokasi.Focus(); }
        }

        private void lstSaran_MouseClick(object sender, MouseEventArgs e)
        {
            PilihSaran();
        }

        // Dua baris per saran: nama kota tebal, lalu provinsi & negara
        private void lstSaran_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || lstSaran.Items[e.Index] is not SaranLokasi s) return;
            var dipilih = (e.State & DrawItemState.Selected) != 0;
            using (var latar = new SolidBrush(dipilih ? Color.FromArgb(214, 232, 238) : Color.White))
                e.Graphics.FillRectangle(latar, e.Bounds);
            TextRenderer.DrawText(e.Graphics, s.Nama, FontSaran, new Point(e.Bounds.X + 8, e.Bounds.Y + 2), Tema.Tinta);
            TextRenderer.DrawText(e.Graphics, s.Keterangan, FontKecil, new Point(e.Bounds.X + 8, e.Bounds.Y + 19), Tema.Redup);
        }

        // Terima koma maupun titik sebagai pemisah desimal
        private static bool TryParseKoordinat(string text, out double nilai)
        {
            return double.TryParse(text.Trim().Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out nilai);
        }

        private void TampilkanKosong()
        {
            var kosong = new Label
            {
                Text = "Saran tindakan muncul di sini setelah cuaca dicek.",
                ForeColor = Tema.Redup,
                Font = FontBesar,
                AutoSize = false,
                Size = new Size(296, 40)
            };
            flowRekomendasi.Controls.Add(kosong);
            TampilkanPrakiraan();
        }

        // ===== Lokasi favorit =====
        private async Task MuatFavoritAsync()
        {
            try { favorit = await Task.Run(() => lokasiRepo.GetFavorites(user.UserId)); }
            catch (Exception) { favorit = new List<Location>(); }   // database lama tanpa kolom favorit: fitur diam saja
            if (IsDisposed) return;

            flowFavorit.SuspendLayout();
            Kosongkan(flowFavorit);
            if (favorit.Count == 0)
            {
                flowFavorit.Controls.Add(new Label
                {
                    Text = "Tekan ☆ untuk menyimpan lokasi favorit, lalu cek cuacanya sekali klik dari sini.",
                    ForeColor = Tema.Redup, Font = FontSedang, AutoSize = true, Margin = new Padding(0, 7, 0, 0)
                });
            }
            foreach (var lok in favorit)
            {
                var chip = new Button
                {
                    Text = "★ " + lok.Name.Split(',')[0], AutoSize = true, Height = 28, Tag = lok,
                    FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(231, 241, 244), ForeColor = Tema.Langit,
                    Font = FontSedang, Cursor = Cursors.Hand, Margin = new Padding(0, 0, 6, 0)
                };
                chip.FlatAppearance.BorderSize = 0;
                chip.Click += (s, e) =>
                {
                    var (lat, lon) = lok.getCoordinates();
                    IsiLokasi(lok.Name, lat, lon);
                    btnCek.PerformClick();
                };
                var menu = new ContextMenuStrip();
                menu.Items.Add("Hapus dari favorit", null, async (s, e) => await UbahFavoritAsync(lok, false));
                chip.ContextMenuStrip = menu;
                chip.Disposed += (s, e) => menu.Dispose();
                tip.SetToolTip(chip, lok.Name + "\nKlik kanan untuk menghapus dari favorit");
                flowFavorit.Controls.Add(chip);
            }
            flowFavorit.ResumeLayout();
            PerbaruiBintang();
        }

        private bool LokasiIniFavorit() =>
            favorit.Any(f => f.Name.Equals(tbLokasi.Text.Trim(), StringComparison.OrdinalIgnoreCase));

        private void PerbaruiBintang()
        {
            btnFavorit.Text = LokasiIniFavorit() ? "★" : "☆";
        }

        private async void btnFavorit_Click(object sender, EventArgs e)
        {
            if (tbLokasi.Text.Trim().Length == 0 || !TryParseKoordinat(tbLatitude.Text, out var lat) || !TryParseKoordinat(tbLongitude.Text, out var lon)
                || lat < -90 || lat > 90 || lon < -180 || lon > 180)
            {
                Pesan("Pilih lokasi dulu (dari saran, Lokasi saya, atau isi koordinat), baru simpan sebagai favorit.");
                return;
            }
            var lokasi = new Location { Name = tbLokasi.Text.Trim() };
            lokasi.setLocation(lat, lon);
            await UbahFavoritAsync(lokasi, !LokasiIniFavorit());
        }

        private async Task UbahFavoritAsync(Location lokasi, bool jadikanFavorit)
        {
            btnFavorit.Enabled = false;
            try
            {
                await Task.Run(() => lokasiRepo.SetFavorite(user.UserId, lokasi, jadikanFavorit));
                if (IsDisposed) return;
                Pesan(jadikanFavorit ? $"{lokasi.Name} disimpan ke favorit." : $"{lokasi.Name} dihapus dari favorit.", error: false);
                await MuatFavoritAsync();
            }
            catch (Exception ex)
            {
                if (!IsDisposed) Pesan("Favorit tidak bisa disimpan: " + ex.Message);
            }
            finally
            {
                if (!IsDisposed) btnFavorit.Enabled = true;
            }
        }

        private void TampilkanHasil(string namaLokasi, WeatherData data)
        {
            // Hero: lokasi, suhu, kondisi, latar langit
            var (atas, bawah) = WarnaLangit(data.WeatherCode, data.IsDay);
            Animasi.Ubah(this, nameof(LangitAtas), atas, 600);
            Animasi.Ubah(this, nameof(LangitBawah), bawah, 600);
            picCuaca.Image = Ikon.Muat(Ikon.NamaCuaca(data.WeatherCode, data.IsDay));
            adegan.Atur(data.WeatherCode, data.IsDay, data.Temperature, data.WindSpeed, pnlHero.Size);
            adeganBerjalan = Animasi.Aktif;
            AturTimerAdegan();
            pnlHero.Invalidate();
            lblLokasiHasil.Text = namaLokasi;
            // Suhu berjalan ke nilai baru sambil naik sedikit dari bawah
            lblSuhu.Top = suhuTop + (Animasi.Aktif ? 14 : 0);
            Animasi.Ubah(lblSuhu, nameof(Control.Top), suhuTop, 450);
            Animasi.Ubah(this, nameof(SuhuTampil), data.Temperature, 800);
            lblKondisi.Text = Ikon.KeteranganCuaca(data.WeatherCode);
            lblTerasa.Text = $"Terasa seperti {data.ApparentTemperature:0}°";

            // Kotak detail
            lblKelembapan.Text = $"{data.Humidity:0} %";
            lblAngin.Text = $"{data.WindSpeed:0.#} km/j";
            lblHujan.Text = $"{data.Rainfall:0.#} mm/j";
            lblUv.Text = $"{data.UvIndex:0.#} {KategoriUv(data.UvIndex)}";
            lblUdara.Text = data.Aqi is int aqi ? $"{aqi} {KategoriAqi(aqi)}" : "-";

            dataTerakhir = data;
            TampilkanPrakiraan();

            // Kartu risiko ala peringatan dini, berputar 3D ke status baru
            Flip3D.Putar(pnlRisiko, () =>
            {
                pnlRisiko.BackColor = Tema.WarnaStatus(data.RiskLevel);
                foreach (var l in new[] { lblRisikoJudul, lblStatus, lblRincian }) l.ForeColor = Tema.TeksStatus(data.RiskLevel);
                picStatus.Image = Ikon.Status(data.RiskLevel);
                picStatus.Visible = true;
                lblStatus.Text = data.RiskLevel.ToString();
                lblRincian.Text = string.Join("\n", riskEvaluator.rincian(data).Select(r => $"{r.Nama} ({NilaiAturan(r.Nama, data)}): {r.Level}"));
            });

            TampilkanRekomendasi(data.RiskLevel);
            lblSumber.Text = $"Data cuaca Open-Meteo pukul {data.Timestamp:HH:mm} waktu setempat. Hasil ini sudah tersimpan ke riwayat.";
        }

        // Nilai yang dinilai tiap aturan, supaya alasan status terbaca jelas
        private static string NilaiAturan(string nama, WeatherData d) => nama switch
        {
            "Suhu udara" => $"{d.Temperature:0.#}°C",
            "Curah hujan" => $"{d.Rainfall:0.#} mm/j",
            "Hujan + angin" => $"{d.Rainfall:0.#} mm/j, {d.WindSpeed:0.#} km/j",
            "Indeks UV" => $"{d.UvIndex:0.#}",
            "Heat index" => $"{HeatIndexRule.Hitung(d.Temperature, d.Humidity):0.#}°C",
            "Kualitas udara" => d.Aqi is int a ? $"AQI {a}" : "tidak tersedia",
            _ => "-"
        };

        private static string KategoriAqi(int aqi) => aqi switch
        {
            <= 50 => "baik",
            <= 100 => "sedang",
            <= 150 => "sensitif",
            <= 200 => "buruk",
            _ => "bahaya"
        };

        // ===== Prakiraan: per jam atau 7 hari =====
        private void btnPerJam_Click(object sender, EventArgs e) { tampilHarian = false; TampilkanPrakiraan(); }
        private void btnHarian_Click(object sender, EventArgs e) { tampilHarian = true; TampilkanPrakiraan(); }

        private void TampilkanPrakiraan()
        {
            // Tombol yang aktif bergaya tombol utama
            foreach (var (b, aktif) in new[] { (btnPerJam, !tampilHarian), (btnHarian, tampilHarian) })
            {
                b.BackColor = aktif ? Tema.Langit : Color.White;
                b.ForeColor = aktif ? Color.White : Tema.Langit;
                b.FlatAppearance.BorderSize = aktif ? 0 : 1;
            }
            if (dataTerakhir == null)
            {
                Kosongkan(flowJam);
                flowJam.Controls.Add(new Label
                {
                    Text = tampilHarian ? "Prakiraan 7 hari muncul setelah cuaca dicek." : "Prakiraan 8 jam ke depan muncul setelah cuaca dicek.",
                    ForeColor = Tema.Redup, Font = FontBesar, AutoSize = false, Size = new Size(470, 40)
                });
                return;
            }
            if (tampilHarian) TampilkanHarian(dataTerakhir.Daily); else TampilkanPerJam(dataTerakhir.Hourly);
        }

        private void TampilkanHarian(List<PrakiraanHari> hari)
        {
            var id = new CultureInfo("id-ID");
            flowJam.SuspendLayout();
            Kosongkan(flowJam);
            foreach (var (h, i) in hari.Take(7).Select((h, i) => (h, i)))
            {
                var item = new Panel { Size = new Size(67, 78), Margin = new Padding(1, 0, 1, 0) };
                item.Controls.Add(new Label
                {
                    Text = i == 0 ? "Hari ini" : h.Tanggal.ToString("ddd d", id), Font = FontKecil, ForeColor = Tema.Redup,
                    AutoSize = false, Size = new Size(67, 15), Location = new Point(0, 0), TextAlign = ContentAlignment.MiddleCenter
                });
                item.Controls.Add(new PictureBox
                {
                    Image = Ikon.Muat(Ikon.NamaCuaca(h.KodeCuaca, true)), SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(34, 34), Location = new Point(16, 15)
                });
                item.Controls.Add(new Label
                {
                    Text = $"{h.SuhuMaks:0}° / {h.SuhuMin:0}°", Font = FontSuhuHarian, ForeColor = Tema.Tinta,
                    AutoSize = false, Size = new Size(67, 16), Location = new Point(0, 48), TextAlign = ContentAlignment.MiddleCenter
                });
                item.Controls.Add(new Label
                {
                    Text = $"{h.PeluangHujan}%", Font = FontKecil, ForeColor = Color.FromArgb(30, 100, 200),
                    AutoSize = false, Size = new Size(67, 14), Location = new Point(0, 64), TextAlign = ContentAlignment.MiddleCenter
                });
                flowJam.Controls.Add(item);
            }
            flowJam.ResumeLayout();
        }

        private void TampilkanPerJam(List<PrakiraanJam> jam)
        {
            flowJam.SuspendLayout();
            Kosongkan(flowJam);
            foreach (var j in jam.Take(8))
            {
                var item = new Panel { Size = new Size(58, 78), Margin = new Padding(1, 0, 1, 0) };
                item.Controls.Add(new Label
                {
                    Text = j.Waktu.ToString("HH:mm"), Font = FontKecil, ForeColor = Tema.Redup,
                    AutoSize = false, Size = new Size(58, 15), Location = new Point(0, 0), TextAlign = ContentAlignment.MiddleCenter
                });
                item.Controls.Add(new PictureBox
                {
                    Image = Ikon.Muat(Ikon.NamaCuaca(j.KodeCuaca, j.Siang)), SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(34, 34), Location = new Point(12, 15)
                });
                item.Controls.Add(new Label
                {
                    Text = $"{j.Suhu:0}°", Font = FontSuhuJam, ForeColor = Tema.Tinta,
                    AutoSize = false, Size = new Size(58, 16), Location = new Point(0, 48), TextAlign = ContentAlignment.MiddleCenter
                });
                item.Controls.Add(new Label
                {
                    Text = $"{j.PeluangHujan}%", Font = FontKecil, ForeColor = Color.FromArgb(30, 100, 200),
                    AutoSize = false, Size = new Size(58, 14), Location = new Point(0, 64), TextAlign = ContentAlignment.MiddleCenter
                });
                flowJam.Controls.Add(item);
            }
            flowJam.ResumeLayout();
        }

        private void TampilkanRekomendasi(RiskLevel level)
        {
            timerKartu.Stop();
            flowRekomendasi.SuspendLayout();
            Kosongkan(flowRekomendasi);
            kartu.Clear();

            var warna = Tema.WarnaStatus(level);
            foreach (var (kategori, teks) in recommender.getRecommendationDetail(level))
            {
                var k = BuatKartu(kategori, teks, warna);
                kartu.Add(k);
                flowRekomendasi.Controls.Add(k);
            }
            flowRekomendasi.ResumeLayout();

            // Hormati pengaturan Windows "Animation effects" (reduced motion)
            if (!SystemInformation.UIEffectsEnabled) return;
            foreach (var k in kartu) { k.Visible = false; k.Margin = new Padding(40, 0, 0, 8); }
            langkahAnimasi = 0;
            timerKartu.Start();
        }

        private static Panel BuatKartu(string kategori, string teks, Color warna)
        {
            var isi = new Label
            {
                Text = teks, Font = FontBesar, ForeColor = Tema.Tinta,
                AutoSize = false, Location = new Point(58, 24), MaximumSize = new Size(226, 0)
            };
            var ukuran = TextRenderer.MeasureText(teks, isi.Font, new Size(226, 0), TextFormatFlags.WordBreak);
            isi.Size = new Size(226, ukuran.Height + 2);

            var k = new Panel
            {
                BackColor = Color.White,
                Size = new Size(294, Math.Max(64, isi.Bottom + 10)),
                Margin = new Padding(0, 0, 0, 8)
            };
            // Garis warna status di sisi kiri kartu
            k.Paint += (s, e) => { using var kuas = new SolidBrush(warna); e.Graphics.FillRectangle(kuas, 0, 0, 5, k.Height); };
            k.Controls.Add(new PictureBox
            {
                Image = Ikon.Muat(IkonKategori(kategori)), SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(38, 38), Location = new Point(13, (k.Height - 38) / 2)
            });
            k.Controls.Add(new Label
            {
                Text = char.ToUpper(kategori[0]) + kategori[1..], Font = FontKategori,
                ForeColor = Tema.Redup, AutoSize = true, Location = new Point(57, 6)
            });
            k.Controls.Add(isi);
            return k;
        }

        private static string IkonKategori(string kategori) => kategori switch
        {
            "aktivitas" => "sun-hot",
            "kesehatan" => "thermometer-water",
            "perlindungan diri" => "umbrella",
            _ => "not-available"
        };

        private static void Kosongkan(Control induk)
        {
            while (induk.Controls.Count > 0) induk.Controls[0].Dispose();
        }

        private void AturTimerAdegan()
        {
            var perlu = adeganBerjalan && jendelaAktif && WindowState != FormWindowState.Minimized;
            if (perlu == timerAdegan.Enabled) return;
            if (perlu) { detikTerakhir = jamAdegan.Elapsed.TotalSeconds; timerAdegan.Start(); }
            else timerAdegan.Stop();
        }

        private void timerKartu_Tick(object? sender, EventArgs e)
        {
            langkahAnimasi++;
            const int jeda = 5, durasi = 10;   // dalam langkah timer (~15 ms)
            var selesai = true;
            for (int i = 0; i < kartu.Count; i++)
            {
                var t = Math.Clamp((langkahAnimasi - i * jeda) / (double)durasi, 0, 1);
                if (t <= 0) { selesai = false; continue; }
                var ease = 1 - Math.Pow(1 - t, 3);   // ease-out cubic
                kartu[i].Visible = true;
                kartu[i].Margin = new Padding((int)(40 * (1 - ease)), 0, 0, 8);
                if (t < 1) selesai = false;
            }
            if (selesai) timerKartu.Stop();
        }

        private void pnlHero_Paint(object? sender, PaintEventArgs e)
        {
            using var kuas = new LinearGradientBrush(pnlHero.ClientRectangle, langitAtas, langitBawah, LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(kuas, pnlHero.ClientRectangle);
            adegan.Gambar(e.Graphics, pnlHero.ClientSize);
        }

        private static (Color, Color) WarnaLangit(int kode, bool siang)
        {
            // Semua pasangan warna diuji kontrasnya terhadap teks putih: minimal 4,5:1 (WCAG AA)
            if (!siang) return (Color.FromArgb(18, 30, 62), Color.FromArgb(44, 62, 104));                 // malam   10,5:1
            if (kode >= 51) return (Color.FromArgb(48, 62, 80), Color.FromArgb(72, 90, 110));             // hujan    7,1:1
            if (kode == 3 || kode == 45 || kode == 48) return (Color.FromArgb(62, 82, 102), Color.FromArgb(84, 104, 124)); // mendung 5,8:1
            return (Color.FromArgb(21, 96, 170), Color.FromArgb(36, 114, 186));                           // cerah    5,0:1
        }

        private static string KategoriUv(double uv) => uv switch
        {
            < 3 => "rendah",
            < 6 => "sedang",
            < 8 => "tinggi",
            < 11 => "sangat tinggi",
            _ => "ekstrem"
        };

        private void Pesan(string teks, bool error = true)
        {
            lblPesan.ForeColor = error ? Color.FromArgb(198, 40, 40) : Tema.Redup;
            lblPesan.Text = teks;
        }

        // Kunci tombol selama proses berjalan; kesalahan ditampilkan di form, bukan di kotak dialog
        private async Task Jalankan(string pesanProses, Func<Task<bool>> aksi)
        {
            btnCek.Enabled = btnLokasiSaya.Enabled = false;
            SembunyikanSaran();
            Cursor = Cursors.WaitCursor;
            Pesan(pesanProses, error: false);
            loading.Mulai();
            try
            {
                if (await aksi()) Pesan("");
            }
            catch (HttpRequestException)
            {
                Pesan("Open-Meteo tidak bisa dihubungi. Periksa koneksi internet, lalu coba lagi.");
            }
            catch (TaskCanceledException)
            {
                Pesan("Open-Meteo terlalu lama merespons. Coba lagi sebentar lagi.");
            }
            catch (InvalidOperationException ex)
            {
                Pesan(ex.Message);
            }
            catch (Exception ex)
            {
                Pesan("Gagal memproses: " + ex.Message);
            }
            finally
            {
                loading.Berhenti();
                btnCek.Enabled = btnLokasiSaya.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            timerKartu.Stop();
            this.Close();
        }
    }
}
