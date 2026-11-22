using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Swasana
{
    // Grafik garis sederhana (GDI+) untuk tren suhu maksimum & minimum harian.
    // WinForms di .NET modern tidak punya kontrol chart bawaan.
    public class GrafikTren : Control
    {
        private List<(DateTime Tanggal, double Maks, double Min)> data = new();

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Judul { get; set; } = "Tren suhu harian";

        public GrafikTren()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = Color.White;
        }

        public void Tampilkan(List<(DateTime Tanggal, double Maks, double Min)> titik, string judul)
        {
            data = titik;
            Judul = judul;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using var garisTepi = new Pen(Color.FromArgb(200, 212, 219));
            g.DrawRectangle(garisTepi, 0, 0, Width - 1, Height - 1);

            using var fJudul = new Font("Segoe UI Semibold", 9.75F);
            using var fKecil = new Font("Segoe UI", 8.25F);
            TextRenderer.DrawText(g, Judul, fJudul, new Point(10, 8), Tema.Tinta);

            // Legenda
            TextRenderer.DrawText(g, "Maks", fKecil, new Point(Width - 112, 10), Tema.Redup);
            TextRenderer.DrawText(g, "Min", fKecil, new Point(Width - 52, 10), Tema.Redup);
            using (var kM = new SolidBrush(WarnaMaks)) g.FillEllipse(kM, Width - 124, 14, 8, 8);
            using (var kN = new SolidBrush(WarnaMin)) g.FillEllipse(kN, Width - 64, 14, 8, 8);

            if (data.Count == 0)
            {
                TextRenderer.DrawText(g, "Belum ada data untuk ditampilkan.", fKecil, new Point(10, Height / 2 - 8), Tema.Redup);
                return;
            }

            var area = new RectangleF(44, 36, Width - 64, Height - 64);
            var atas = Math.Ceiling(data.Max(d => d.Maks) + 1);
            var bawah = Math.Floor(data.Min(d => d.Min) - 1);
            if (atas - bawah < 4) { atas += 2; bawah -= 2; }

            float X(int i) => data.Count == 1 ? area.Left + area.Width / 2 : area.Left + area.Width * i / (data.Count - 1);
            float Y(double v) => (float)(area.Bottom - (v - bawah) / (atas - bawah) * area.Height);

            // Garis bantu sumbu Y
            using var bantu = new Pen(Color.FromArgb(232, 238, 241));
            for (int k = 0; k <= 4; k++)
            {
                var v = bawah + (atas - bawah) * k / 4;
                var y = Y(v);
                g.DrawLine(bantu, area.Left, y, area.Right, y);
                TextRenderer.DrawText(g, $"{v:0}°", fKecil, new Rectangle(0, (int)y - 8, 40, 16), Tema.Redup, TextFormatFlags.Right);
            }

            // Label tanggal (dikurangi kalau terlalu rapat)
            var loncat = Math.Max(1, (int)Math.Ceiling(data.Count / (area.Width / 56)));
            for (int i = 0; i < data.Count; i += loncat)
                TextRenderer.DrawText(g, data[i].Tanggal.ToString("dd/MM"), fKecil,
                    new Rectangle((int)X(i) - 24, (int)area.Bottom + 6, 48, 16), Tema.Redup, TextFormatFlags.HorizontalCenter);

            Garis(g, data.Select((d, i) => new PointF(X(i), Y(d.Min))).ToArray(), WarnaMin);
            Garis(g, data.Select((d, i) => new PointF(X(i), Y(d.Maks))).ToArray(), WarnaMaks);

            if (data.Count == 1)
                TextRenderer.DrawText(g, "Tren terlihat setelah ada data dari 2 hari atau lebih.", fKecil,
                    new Point((int)area.Left, (int)area.Top), Tema.Redup);
        }

        private static readonly Color WarnaMaks = Color.FromArgb(198, 40, 40);
        private static readonly Color WarnaMin = Color.FromArgb(21, 96, 170);

        private static void Garis(Graphics g, PointF[] titik, Color warna)
        {
            using var pena = new Pen(warna, 2.2f);
            if (titik.Length > 1) g.DrawLines(pena, titik);
            using var kuas = new SolidBrush(warna);
            foreach (var t in titik) g.FillEllipse(kuas, t.X - 3.5f, t.Y - 3.5f, 7, 7);
        }
    }
}
