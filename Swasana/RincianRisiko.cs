using System.Drawing.Drawing2D;
using Swasana.Models;

namespace Swasana
{
    // Isi kartu status risiko: deretan chip (satu per aturan) lalu rincian aturan yang memicu.
    // Sebelum ada data, menampilkan Text biasa.
    public class RincianRisiko : Label
    {
        private List<(string Nama, string Nilai, RiskLevel Level)> isi = new();

        // Animasi: chip muncul bergiliran ("pop"), rincian menyusul, lalu titik aturan pemicu berdenyut.
        // Mati kalau efek animasi Windows dimatikan.
        private const int JedaChip = 70, LamaChip = 260, LamaDenyut = 1600;   // ms
        private readonly System.Windows.Forms.Timer timer = new() { Interval = 33 };
        private long mulai;

        public RincianRisiko()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            AutoSize = false;
            timer.Tick += (s, e) =>
            {
                bool introJalan = Waktu < JedaChip * isi.Count + LamaChip + 200;
                bool denyut = isi.Any(a => a.Level != RiskLevel.Aman);
                if (!introJalan && !denyut) { timer.Stop(); return; }
                // Denyut hanya digambar ulang saat jendelanya aktif, supaya hemat CPU
                if (introJalan || (Visible && Form.ActiveForm == FindForm())) Invalidate();
            };
        }

        private long Waktu => Animasi.Aktif ? Environment.TickCount64 - mulai : long.MaxValue / 2;

        protected override void Dispose(bool disposing)
        {
            if (disposing) timer.Dispose();
            base.Dispose(disposing);
        }

        public void Isi(List<(string Nama, string Nilai, RiskLevel Level)> aturan)
        {
            isi = aturan;
            var pemicu = aturan.Where(a => a.Level != RiskLevel.Aman).ToList();
            // Text tetap diisi untuk pembaca layar dan pengujian otomatis
            Text = pemicu.Count > 0
                ? string.Join("\n", pemicu.Select(a => $"{a.Nama} ({a.Nilai}): {a.Level}"))
                : "Semua aturan risiko dalam batas aman.";
            mulai = Environment.TickCount64 + 250;   // tunggu kartu selesai berputar 3D
            if (Animasi.Aktif) timer.Start();
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
            var waktu = Waktu;
            for (int i = 0; i < isi.Count; i++)
            {
                var (nama, _, level) = isi[i];
                var r = new Rectangle(i % kolom * (lebar + jarak), i / kolom * (tinggi + jarak), lebar, tinggi);
                bool aman = level == RiskLevel.Aman;

                // Pop: chip membesar dari tengah dengan sedikit memantul (ease-out-back)
                var p = Math.Clamp((waktu - i * JedaChip) / (double)LamaChip, 0, 1);
                if (p <= 0) continue;
                if (p < 1)
                {
                    var c1 = 1.70158; var q = p - 1;
                    var skala = (float)(1 + (c1 + 1) * q * q * q + c1 * q * q);
                    var rp = Rectangle.Round(new RectangleF(r.X + r.Width * (1 - skala) / 2, r.Y + r.Height * (1 - skala) / 2, r.Width * skala, r.Height * skala));
                    if (rp.Width < 2 || rp.Height < 2) continue;
                    using var jp = Bulat(rp, rp.Height / 2);
                    using var bp = new SolidBrush(Color.FromArgb((int)((aman ? 70 : 255) * Math.Min(1, p * 1.5)), Color.White));
                    g.FillPath(bp, jp);
                    continue;
                }
                using (var jalur = Bulat(r, tinggi / 2))
                using (var isiChip = new SolidBrush(aman ? Color.FromArgb(70, Color.White) : Color.White))
                    g.FillPath(isiChip, jalur);

                var teks = aman ? ForeColor : Tema.Tinta;
                int x = r.X + (int)(8 * k);
                if (aman)
                    TextRenderer.DrawText(g, "✓", hurufChip, new Point(x - 1, r.Y + (tinggi - hurufChip.Height) / 2), teks);
                else
                {
                    var warnaTitik = level == RiskLevel.Bahaya ? Tema.WarnaStatus(RiskLevel.Bahaya) : Tema.WaspadaGelap;
                    float cx = x + 4 * k, cy = r.Y + tinggi / 2f;
                    if (Animasi.Aktif)
                    {
                        // Denyut: cincin yang membesar lalu memudar
                        var f = (waktu % LamaDenyut) / (float)LamaDenyut;
                        var jari = (4 + 6 * f) * k;
                        using var pena = new Pen(Color.FromArgb((int)(170 * (1 - f)), warnaTitik), 1.5f * k);
                        g.DrawEllipse(pena, cx - jari, cy - jari, jari * 2, jari * 2);
                    }
                    using var titik = new SolidBrush(warnaTitik);
                    g.FillEllipse(titik, cx - 4 * k, cy - 4 * k, 8 * k, 8 * k);
                }
                TextRenderer.DrawText(g, Singkat(nama), hurufChip,
                    new Rectangle(x + (int)(12 * k), r.Y, r.Right - x - (int)(14 * k), tinggi), teks,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }

            // Rincian aturan yang memicu, muncul setelah semua chip
            if (waktu < JedaChip * isi.Count + LamaChip / 2) return;
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
