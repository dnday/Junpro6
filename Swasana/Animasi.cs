using FluentTransitions;

namespace Swasana
{
    // Animasi kecil memakai FluentTransitions (MIT, github.com/awaescher/FluentTransitions).
    // Semua dimatikan kalau "Animation effects" di Windows dimatikan (reduced motion).
    public static class Animasi
    {
        public static bool Aktif => SystemInformation.UIEffectsEnabled;

        // Form muncul perlahan saat pertama tampil
        public static void FadeIn(Form form)
        {
            if (!Aktif) return;
            form.Opacity = 0;
            form.Shown += (s, e) => Transition.With(form, nameof(Form.Opacity), 1.0).EaseInEaseOut(TimeSpan.FromMilliseconds(220));
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
