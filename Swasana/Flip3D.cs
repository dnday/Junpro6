using System.ComponentModel;
using FluentTransitions;

namespace Swasana
{
    // Efek "kartu berputar" 3D di sumbu Y untuk WinForms (yang tidak punya mesin 3D).
    // Caranya: potret panel sebelum & sesudah berubah, lalu gambar ulang per irisan vertikal
    // dengan skala perspektif (irisan yang "menjauh" digambar lebih pendek).
    public class Flip3D : Control
    {
        private readonly Bitmap depan;
        private Bitmap? belakang;
        private double sudut;   // derajat, 0..180

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double Sudut { get => sudut; set { sudut = value; Invalidate(); } }

        private Flip3D(Bitmap depan, Color latar)
        {
            this.depan = depan;
            BackColor = latar;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        // Putar panel: ubahIsi() dijalankan di tengah, lalu sisi barunya ikut berputar masuk
        public static void Putar(Control panel, Action ubahIsi, int milidetik = 700)
        {
            if (!Animasi.Aktif || panel.Parent == null || panel.Width == 0)
            {
                ubahIsi();
                return;
            }

            // Lapisan animasi dipasang dulu (menampilkan sisi lama), baru isi panel diubah di belakangnya,
            // supaya perubahan mentah tidak pernah sempat terlihat
            var lama = Potret(panel);
            var flip = new Flip3D(lama, panel.Parent.BackColor) { Bounds = panel.Bounds };
            panel.Parent.Controls.Add(flip);
            flip.BringToFront();
            flip.Refresh();

            ubahIsi();
            flip.belakang = Potret(panel);

            Transition.With(flip, nameof(Sudut), 180.0)
                .HookOnCompletionInUiThread(flip, () =>
                {
                    flip.Parent?.Controls.Remove(flip);
                    flip.Dispose();
                })
                .EaseInEaseOut(TimeSpan.FromMilliseconds(milidetik));
        }

        private static Bitmap Potret(Control c)
        {
            var bmp = new Bitmap(c.Width, c.Height);
            c.DrawToBitmap(bmp, new Rectangle(Point.Empty, c.Size));
            return bmp;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Paruh pertama memperlihatkan sisi lama, paruh kedua sisi baru yang datang dari sisi sebaliknya
            var gambar = sudut <= 90 || belakang == null ? depan : belakang;
            var rad = (sudut <= 90 ? sudut : sudut - 180) * Math.PI / 180;
            var cos = Math.Cos(rad);
            var sin = Math.Sin(rad);

            int w = Width, h = Height;
            double cx = w / 2.0, jarakKamera = w * 2.2;
            const int irisan = 4;

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            // Bayangan makin gelap saat kartu menyamping, supaya kesan 3D lebih terasa
            using var bayangan = new SolidBrush(Color.FromArgb((int)(90 * (1 - Math.Abs(cos))), 0, 0, 0));
            for (int x = 0; x < w; x += irisan)
            {
                // Posisi irisan setelah diputar, lalu diproyeksikan dengan perspektif
                double u1 = x - cx, u2 = Math.Min(x + irisan, w) - cx;
                double f1 = jarakKamera / (jarakKamera + u1 * sin);
                double f2 = jarakKamera / (jarakKamera + u2 * sin);
                double x1 = cx + u1 * cos * f1, x2 = cx + u2 * cos * f2;
                double tinggi = h * (f1 + f2) / 2;

                var tujuan = new RectangleF((float)Math.Min(x1, x2), (float)((h - tinggi) / 2),
                                            (float)Math.Abs(x2 - x1) + 1, (float)tinggi);
                var sumber = new RectangleF(x, 0, Math.Min(irisan, w - x), h);
                e.Graphics.DrawImage(gambar, tujuan, sumber, GraphicsUnit.Pixel);
                e.Graphics.FillRectangle(bayangan, tujuan);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                depan.Dispose();
                belakang?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
