using FluentTransitions;

namespace Swasana
{
    // Animasi kecil memakai FluentTransitions (MIT, github.com/awaescher/FluentTransitions).
    // Semua dimatikan kalau "Animation effects" di Windows dimatikan (reduced motion).
    public static class Animasi
    {
        public static bool Aktif => SystemInformation.UIEffectsEnabled;

        // Form muncul perlahan sambil naik sedikit dari bawah (transisi pindah halaman)
        public static void FadeIn(Form form)
        {
            if (!Aktif) return;
            form.Opacity = 0;
            form.Shown += (s, e) =>
            {
                var top = form.Top;
                form.Top = top + form.LogicalToDeviceUnits(18);
                Transition.With(form, nameof(Form.Opacity), 1.0)
                          .With(form, nameof(Form.Top), top)
                          .EaseInEaseOut(TimeSpan.FromMilliseconds(280));
            };
        }

        // Jalankan aksi tiap frame (~60 fps) selama durasi tertentu; p = 0..1. Selesai -> aksi(1).
        public static void Selama(int milidetik, Action<double> aksi)
        {
            if (!Aktif) { aksi(1); return; }
            var mulai = Environment.TickCount64;
            var timer = new System.Windows.Forms.Timer { Interval = 16 };
            timer.Tick += (s, e) =>
            {
                var p = Math.Min(1.0, (Environment.TickCount64 - mulai) / (double)milidetik);
                aksi(p);
                if (p >= 1) { timer.Stop(); timer.Dispose(); }
            };
            timer.Start();
        }

        // Kontrol bergetar ke kiri-kanan lalu diam (misalnya kartu status saat Bahaya)
        public static void Getar(Control c, int amplitudo, int milidetik = 450)
        {
            var asal = c.Left;
            Selama(milidetik, p => { if (!c.IsDisposed) c.Left = asal + (int)Math.Round(amplitudo * (1 - p) * Math.Sin(p * Math.PI * 7)); });
        }

        // Kotak sedikit terangkat saat kursor di atasnya (termasuk di atas isi kotak), turun lagi saat keluar
        public static void Angkat(Control kotak)
        {
            int? asal = null;
            void Masuk(object? s, EventArgs e)
            {
                asal ??= kotak.Top;
                Ubah(kotak, nameof(Control.Top), asal.Value - kotak.LogicalToDeviceUnits(3), 120);
            }
            void Keluar(object? s, EventArgs e)
            {
                if (asal == null || kotak.ClientRectangle.Contains(kotak.PointToClient(Cursor.Position))) return;
                Ubah(kotak, nameof(Control.Top), asal.Value, 160);
            }
            foreach (var c in new[] { kotak }.Concat(kotak.Controls.Cast<Control>()))
            {
                c.MouseEnter += Masuk;
                c.MouseLeave += Keluar;
            }
        }

        // Gambar ikon cuaca beranimasi Meteocons pada PictureBox, hanya saat jendelanya aktif
        public static void IkonHidup(PictureBox pic, Form form, string nama)
        {
            if (!Aktif) return;
            var jam = System.Diagnostics.Stopwatch.StartNew();
            var timer = new System.Windows.Forms.Timer { Interval = 33 };
            timer.Tick += (s, e) =>
            {
                if (Form.ActiveForm == form && Ikon.FrameAnimasi(nama, jam.Elapsed.TotalSeconds) is { } f) pic.Image = f;
            };
            form.FormClosed += (s, e) => timer.Dispose();
            timer.Start();
        }

        // Teks tombol diganti spinner berputar selama proses; Dispose() mengembalikan teks asli
        public static IDisposable Spinner(Button tombol, string teks)
        {
            var asli = tombol.Text;
            var frame = new[] { "◐", "◓", "◑", "◒" };
            int i = 0;
            var timer = new System.Windows.Forms.Timer { Interval = 120 };
            timer.Tick += (s, e) => tombol.Text = $"{frame[i++ % frame.Length]}  {teks}";
            tombol.Text = $"{frame[0]}  {teks}";
            if (Aktif) timer.Start();
            return new Selesai(() => { timer.Dispose(); if (!tombol.IsDisposed) tombol.Text = asli; });
        }

        private sealed class Selesai(Action aksi) : IDisposable
        {
            private Action? aksi = aksi;
            public void Dispose() { aksi?.Invoke(); aksi = null; }
        }

        // Warna latar tombol berubah halus saat kursor masuk/keluar
        public static void Hover(Button tombol, Color hover)
        {
            var normal = tombol.BackColor;
            tombol.MouseEnter += (s, e) => Ubah(tombol, nameof(Control.BackColor), hover, 150);
            tombol.MouseLeave += (s, e) => Ubah(tombol, nameof(Control.BackColor), normal, 200);
        }

        // Kontrol muncul satu per satu dari kiri ke kanan (misalnya item prakiraan)
        public static void Bergiliran(IList<Control> daftar, int jedaMs = 55)
        {
            if (!Aktif || daftar.Count == 0) return;
            foreach (var c in daftar) c.Visible = false;
            var i = 0;
            var timer = new System.Windows.Forms.Timer { Interval = jedaMs };
            timer.Tick += (s, e) =>
            {
                while (i < daftar.Count && daftar[i].IsDisposed) i++;   // daftar sudah diganti sebelum selesai
                if (i < daftar.Count) daftar[i++].Visible = true;
                if (i >= daftar.Count) { timer.Stop(); timer.Dispose(); }
            };
            timer.Start();
        }

        // Angka pada label berjalan dari nilai lama ke nilai baru (ease-out), lalu teks akhirnya dipasang
        public static void Hitung(Label label, double dari, double ke, Func<double, string> format, string teksAkhir, int milidetik = 700)
        {
            if (!Aktif || dari == ke) { label.Text = teksAkhir; return; }
            var mulai = Environment.TickCount64;
            var timer = new System.Windows.Forms.Timer { Interval = 16 };
            timer.Tick += (s, e) =>
            {
                var t = Math.Min(1.0, (Environment.TickCount64 - mulai) / (double)milidetik);
                var e3 = 1 - Math.Pow(1 - t, 3);
                if (label.IsDisposed || t >= 1) { timer.Stop(); timer.Dispose(); if (!label.IsDisposed) label.Text = teksAkhir; return; }
                label.Text = format(dari + (ke - dari) * e3);
            };
            timer.Start();
        }

        // Ubah satu properti (int, double, Color, string) dengan transisi; tanpa animasi kalau dimatikan
        public static void Ubah(object target, string properti, object nilai, int milidetik)
        {
            if (Aktif)
                Transition.With(target, properti, nilai).EaseInEaseOut(TimeSpan.FromMilliseconds(milidetik));
            else
                target.GetType().GetProperty(properti)!.SetValue(target, nilai);
        }
    }

    // Pita peringatan kuning dengan teks berjalan (marquee), dipakai untuk peringatan dini BMKG.
    // Tanpa animasi, teks diam dan dipotong dengan "...".
    public class PitaBerjalan : Control
    {
        private readonly System.Windows.Forms.Timer timer = new() { Interval = 30 };
        private readonly System.Diagnostics.Stopwatch jam = System.Diagnostics.Stopwatch.StartNew();

        public PitaBerjalan()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = Color.FromArgb(249, 199, 79);
            ForeColor = Tema.Tinta;
            Font = new Font("Segoe UI Semibold", 9F);
            timer.Tick += (s, e) => Invalidate();
            VisibleChanged += (s, e) => { if (Visible && Animasi.Aktif) timer.Start(); else timer.Stop(); };
            TextChanged += (s, e) => { AccessibleName = Text; Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);
            var lebarTeks = TextRenderer.MeasureText(g, Text, Font).Width;
            var tinggi = ClientSize.Height;
            if (!Animasi.Aktif || lebarTeks <= Width - 12)
            {
                TextRenderer.DrawText(g, Text, Font, new Rectangle(6, 0, Width - 12, tinggi), ForeColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
                return;
            }
            // Dua salinan teks berjalan ke kiri dengan jeda, 45 px per detik
            var jarak = lebarTeks + LogicalToDeviceUnits(60);
            var x = Width - (int)(jam.Elapsed.TotalSeconds * LogicalToDeviceUnits(45) % jarak);
            for (int i = -1; i <= 1; i++)
                TextRenderer.DrawText(g, Text, Font, new Point(x + i * jarak, (tinggi - Font.Height) / 2), ForeColor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { timer.Dispose(); Font.Dispose(); }
            base.Dispose(disposing);
        }
    }

    // Placeholder "skeleton" berkilau selama data dimuat, meniru efek Shimmer dari Facebook
    // (github.com/facebook/shimmer-android). Ditaruh di atas label nilai, lalu disembunyikan saat data tiba.
    public class Shimmer : Control
    {
        private readonly System.Windows.Forms.Timer timer = new() { Interval = 30 };
        private readonly long mulai = Environment.TickCount64;

        public Shimmer()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            timer.Tick += (s, e) => Invalidate();
            VisibleChanged += (s, e) => { if (Visible && Animasi.Aktif) timer.Start(); else timer.Stop(); };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Parent?.BackColor ?? Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, Width - 1, Height - 1);
            using var jalur = new System.Drawing.Drawing2D.GraphicsPath();
            int d = Math.Min(Height, 8);
            jalur.AddArc(r.X, r.Y, d, d, 180, 90); jalur.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            jalur.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90); jalur.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            jalur.CloseFigure();
            var dasar = Color.FromArgb(226, 232, 236);
            using (var b = new SolidBrush(dasar)) g.FillPath(b, jalur);
            if (!Animasi.Aktif) return;

            // Pita terang bergerak dari kiri ke kanan setiap 1,2 detik
            var t = (Environment.TickCount64 - mulai) % 1200 / 1200f;
            int lebarPita = Math.Max(20, Width / 2);
            int x = (int)(-lebarPita + t * (Width + lebarPita * 2));
            var pita = new Rectangle(x, 0, lebarPita, Height);
            using var kilau = new System.Drawing.Drawing2D.LinearGradientBrush(pita, dasar, dasar, 0f);
            kilau.InterpolationColors = new System.Drawing.Drawing2D.ColorBlend
            {
                Colors = new[] { dasar, Color.FromArgb(246, 248, 250), dasar },
                Positions = new[] { 0f, 0.5f, 1f }
            };
            g.SetClip(jalur);
            g.FillRectangle(kilau, pita);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) timer.Dispose();
            base.Dispose(disposing);
        }
    }

    // Garis loading tipis yang bolak-balik selama data diambil (indeterminate progress)
    public class GarisLoading
    {
        private readonly Panel garis;
        private bool jalan;

        public GarisLoading(Control induk, Color warna)
        {
            garis = new Panel { Height = 4, Width = 140, BackColor = warna, Top = 0, Left = -140, Visible = false };
            induk.Controls.Add(garis);
            garis.BringToFront();
        }

        public void Mulai()
        {
            jalan = true;
            garis.Visible = true;
            if (!Animasi.Aktif) { garis.Left = 0; garis.Width = garis.Parent!.Width; return; }
            Putar();
        }

        public void Berhenti()
        {
            jalan = false;
            garis.Visible = false;
        }

        private void Putar()
        {
            if (!jalan) return;
            garis.Left = -garis.Width;
            Transition.With(garis, nameof(Control.Left), garis.Parent!.Width)
                .HookOnCompletionInUiThread(garis, Putar)
                .EaseInEaseOut(TimeSpan.FromMilliseconds(900));
        }
    }
}
