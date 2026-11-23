using System.Drawing.Drawing2D;
using Swasana.Models;

namespace Swasana
{
    // Isi kartu status risiko: deretan chip (satu per aturan) lalu rincian aturan yang memicu.
    // Sebelum ada data, menampilkan Text biasa.
    public class RincianRisiko : Label
    {
        private List<(string Nama, string Nilai, RiskLevel Level)> isi = new();

        public RincianRisiko()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            AutoSize = false;
        }

        public void Isi(List<(string Nama, string Nilai, RiskLevel Level)> aturan)
        {
            isi = aturan;
            var pemicu = aturan.Where(a => a.Level != RiskLevel.Aman).ToList();
            // Text tetap diisi untuk pembaca layar dan pengujian otomatis
            Text = pemicu.Count > 0
                ? string.Join("\n", pemicu.Select(a => $"{a.Nama} ({a.Nilai}): {a.Level}"))
                : "Semua aturan risiko dalam batas aman.";
            Invalidate();
        }

        private static string Singkat(string nama) => nama switch
        {
            "Curah hujan" => "Hujan",
            "Hujan + angin" => "Angin",
            "Indeks UV" => "UV",
            "Heat index" => "Panas",
            "Kualitas udara" => "Udara",
            "Peringatan BMKG" => "BMKG",
            _ => nama
        };

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Parent?.BackColor ?? BackColor);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            float k = DeviceDpi / 96f;

            if (isi.Count == 0)
            {
                TextRenderer.DrawText(g, Text, Font, ClientRectangle, ForeColor, TextFormatFlags.WordBreak);
                return;
            }

            // Chip: 3 per baris
            int kolom = 3, jarak = (int)(6 * k), tinggi = (int)(22 * k);
            int lebar = (Width - jarak * (kolom - 1)) / kolom;
            using var hurufChip = new Font("Segoe UI Semibold", 8.25F);
            for (int i = 0; i < isi.Count; i++)
            {
                var (nama, _, level) = isi[i];
                var r = new Rectangle(i % kolom * (lebar + jarak), i / kolom * (tinggi + jarak), lebar, tinggi);
                bool aman = level == RiskLevel.Aman;
                using (var jalur = Bulat(r, tinggi / 2))
                using (var isiChip = new SolidBrush(aman ? Color.FromArgb(70, Color.White) : Color.White))
                    g.FillPath(isiChip, jalur);

                var teks = aman ? ForeColor : Tema.Tinta;
                int x = r.X + (int)(8 * k);
                if (aman)
                    TextRenderer.DrawText(g, "✓", hurufChip, new Point(x - 1, r.Y + (tinggi - hurufChip.Height) / 2), teks);
                else
                    using (var titik = new SolidBrush(level == RiskLevel.Bahaya ? Tema.WarnaStatus(RiskLevel.Bahaya) : Tema.WaspadaGelap))
                        g.FillEllipse(titik, x, r.Y + tinggi / 2 - 4 * k, 8 * k, 8 * k);
                TextRenderer.DrawText(g, Singkat(nama), hurufChip,
                    new Rectangle(x + (int)(12 * k), r.Y, r.Right - x - (int)(14 * k), tinggi), teks,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }

            // Rincian aturan yang memicu
            int y = (isi.Count + kolom - 1) / kolom * (tinggi + jarak) + (int)(2 * k);
            var pemicu = isi.Where(a => a.Level != RiskLevel.Aman).ToList();
            using var hurufTebal = new Font("Segoe UI Semibold", 9F);
            if (pemicu.Count == 0)
            {
                TextRenderer.DrawText(g, "Semua aturan dalam batas aman.", Font, new Point(0, y), ForeColor);
                return;
            }
            foreach (var (nama, nilai, level) in pemicu)
            {
                var r = new Rectangle(0, y, Width, (int)(18 * k));
                var lebarNama = TextRenderer.MeasureText(g, nama, hurufTebal, r.Size, TextFormatFlags.NoPadding).Width;
                TextRenderer.DrawText(g, nama, hurufTebal, r, ForeColor, TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
                var keterangan = $"  {nilai} · {level}";
                // Keterangan panjang (misalnya peringatan BMKG) turun ke baris berikutnya
                if (lebarNama + TextRenderer.MeasureText(g, keterangan, Font, r.Size, TextFormatFlags.NoPadding).Width > Width)
                {
                    y += r.Height; lebarNama = (int)(4 * k); keterangan = keterangan.TrimStart();
                }
                TextRenderer.DrawText(g, keterangan, Font,
                    new Rectangle(lebarNama, y, Width - lebarNama, r.Height), ForeColor, TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
                y += r.Height;
                if (y + r.Height > Height) break;
            }
        }

        private static GraphicsPath Bulat(Rectangle r, int jari)
        {
            var p = new GraphicsPath();
            int d = jari * 2;
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }
    }
}
