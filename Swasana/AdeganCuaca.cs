using System.Drawing.Drawing2D;

namespace Swasana
{
    // Ilustrasi bergerak di latar panel cuaca yang menyesuaikan kondisi:
    //  panas      -> orang bertopi lebar + botol minum, keringat, gelombang panas, sinar matahari berdenyut
    //  panas >35  -> orang berteduh di bawah payung
    //  cerah      -> orang bertopi, burung terbang
    //  berawan    -> awan bergeser
    //  hujan      -> rintik + cipratan genangan, orang berpayung
    //  badai      -> hujan deras miring, petir + kilat, orang condong
    //  angin      -> daun beterbangan, orang condong (bisa bersamaan dengan kondisi lain)
    //  kabut      -> pita kabut
    //  malam      -> bintang berkelip + bulan
    public class AdeganCuaca
    {
        public enum Jenis { Kosong, Cerah, Panas, SangatPanas, Berawan, Hujan, Badai, Kabut, Malam }

        private Jenis jenis = Jenis.Kosong;
        private bool berangin;
        private float t;   // waktu berjalan dalam satuan "frame 30 fps" (detik x 30), naik halus berapa pun fps-nya
        private readonly Random acak = new Random(7);
        private readonly List<(float X, float Y, float Kecepatan)> tetes = new();
        private readonly List<(float X, float Y, float Fase)> bintang = new();
        private readonly List<(float X, float Y, float Fase)> daun = new();
        // Orang berjalan bolak-balik dengan gerak sinus: melambat sendiri sebelum berbalik
        private double faseJalan, faseLangkah, kecepatanX;
        private float orangX = 272, arah = 1;

        // Siluet terang supaya terlihat di semua warna langit (langit selalu cukup gelap untuk teks putih)
        private static readonly Color Siluet = Color.FromArgb(235, 244, 248, 252);

        public void Atur(int kodeCuaca, bool siang, double suhu, double angin, Size area)
        {
            jenis = kodeCuaca switch
            {
                >= 95 => Jenis.Badai,
                >= 51 and <= 67 or >= 80 and <= 82 => Jenis.Hujan,
                45 or 48 => Jenis.Kabut,
                _ when !siang => Jenis.Malam,
                _ when suhu > 35 => Jenis.SangatPanas,
                _ when suhu > 33 => Jenis.Panas,
                2 or 3 => Jenis.Berawan,
                _ => Jenis.Cerah
            };
            berangin = angin >= 30 || jenis == Jenis.Badai;

            tetes.Clear();
            var jumlah = jenis == Jenis.Badai ? 150 : kodeCuaca is >= 51 and <= 57 ? 45 : 90;   // gerimis lebih jarang
            for (int i = 0; i < jumlah; i++)
                tetes.Add((acak.Next(area.Width), acak.Next(area.Height), 5 + (float)acak.NextDouble() * 4));

            bintang.Clear();
            for (int i = 0; i < 28; i++)
                bintang.Add((acak.Next(area.Width), acak.Next(area.Height - 50), (float)(acak.NextDouble() * Math.PI * 2)));

            daun.Clear();
            for (int i = 0; i < 9; i++)
                daun.Add((acak.Next(area.Width), 20 + acak.Next(area.Height - 40), (float)(acak.NextDouble() * Math.PI * 2)));
        }

        // dt = detik sejak frame sebelumnya; semua gerak dikalikan dt supaya kecepatannya stabil
        public void Tick(Size area, double dt)
        {
            var k = (float)(dt * 30);   // 1.0 = satu langkah pada 30 fps
            t += k;
            var miring = jenis == Jenis.Badai ? 0.6f : 0.25f;
            for (int i = 0; i < tetes.Count; i++)
            {
                var (x, y, v) = tetes[i];
                y += v * k * (jenis == Jenis.Badai ? 1.4f : 1f); x -= v * k * miring;
                if (y > area.Height) { y = -10; x = acak.Next(area.Width + 80); }
                tetes[i] = (x, y, v);
            }
            for (int i = 0; i < daun.Count; i++)
            {
                var (x, y, f) = daun[i];
                x += 3.2f * k; y += (float)Math.Sin(t * 0.08 + f) * 0.8f * k;
                if (x > area.Width + 10) x = -10;
                daun[i] = (x, y, f);
            }

            // Orang berjalan bolak-balik di ruang kosong antara teks dan ikon (lebih pelan saat berteduh/badai)
            var tempo = jenis is Jenis.SangatPanas ? 0.10 : jenis == Jenis.Badai ? 0.14 : 0.22;   // putaran per detik
            faseJalan += tempo * dt;
            var xBaru = 272 + 58 * (float)Math.Sin(faseJalan);
            kecepatanX = (xBaru - orangX) / Math.Max(dt, 0.001);
            if (Math.Abs(xBaru - orangX) > 0.02) arah = xBaru > orangX ? 1 : -1;
            faseLangkah += Math.Abs(xBaru - orangX) * 0.16;   // langkah kaki mengikuti jarak -> kaki tidak "meluncur"
            orangX = xBaru;
        }

        public void Gambar(Graphics g, Size area)
        {
            if (jenis == Jenis.Kosong) return;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            switch (jenis)
            {
                case Jenis.Malam: GambarMalam(g, area); break;
                case Jenis.Berawan: GambarAwan(g, area); break;
                case Jenis.Kabut: GambarKabut(g, area); break;
                case Jenis.Cerah: GambarBurung(g, area); break;
                case Jenis.Panas:
                case Jenis.SangatPanas: GambarPanas(g, area); break;
            }

            if (jenis is Jenis.Hujan or Jenis.Badai) GambarHujan(g, area);
            if (berangin) GambarDaun(g);

            GambarOrang(g, area);

            if (jenis == Jenis.Badai) GambarPetir(g, area);
        }

        private void GambarHujan(Graphics g, Size area)
        {
            using var pena = new Pen(Color.FromArgb(160, 225, 238, 255), jenis == Jenis.Badai ? 1.6f : 1.3f);
            var miring = jenis == Jenis.Badai ? 0.9f : 0.4f;
            foreach (var (x, y, v) in tetes)
                g.DrawLine(pena, x, y, x - v * miring, y + v * 1.6f);

            // Cipratan genangan: elips kecil yang membesar lalu hilang
            for (int i = 0; i < 5; i++)
            {
                var fase = (t + i * 13) % 30 / 30f;
                var x = 40 + i * 95 + (i * 37 % 20);
                using var cincin = new Pen(Color.FromArgb((int)(150 * (1 - fase)), 225, 238, 255), 1.2f);
                g.DrawEllipse(cincin, x - 10 * fase, area.Height - 5 - 2 * fase, 20 * fase, 4 * fase);
            }
        }

        private void GambarPetir(Graphics g, Size area)
        {
            var siklus = t % 130;
            if (siklus > 6) return;
            if (siklus is 0 or 4)
            {
                using var kilat = new SolidBrush(Color.FromArgb(110, 255, 255, 255));
                g.FillRectangle(kilat, 0, 0, area.Width, area.Height);
            }
            using var petir = new Pen(Color.FromArgb(240, 255, 244, 170), 3f) { LineJoin = LineJoin.Round };
            var x = 170f;
            g.DrawLines(petir, new[] { new PointF(x, 0), new PointF(x - 14, 34), new PointF(x + 4, 38), new PointF(x - 12, 78) });
        }

        private void GambarMalam(Graphics g, Size area)
        {
            foreach (var (x, y, fase) in bintang)
            {
                var alpha = (int)(130 + 110 * Math.Sin(t * 0.06 + fase));
                using var kuas = new SolidBrush(Color.FromArgb(Math.Clamp(alpha, 0, 255), 255, 255, 230));
                g.FillEllipse(kuas, x, y, 2.4f, 2.4f);
            }
            // Bulan sabit kecil
            using var bulan = new SolidBrush(Color.FromArgb(220, 255, 247, 214));
            using var langit = new SolidBrush(Color.FromArgb(255, 24, 37, 72));
            g.FillEllipse(bulan, 268, 14, 26, 26);
            g.FillEllipse(langit, 276, 10, 24, 24);
        }

        private void GambarAwan(Graphics g, Size area)
        {
            using var kuas = new SolidBrush(Color.FromArgb(55, 255, 255, 255));
            for (int i = 0; i < 3; i++)
            {
                var x = (t * (0.25f + i * 0.1f) + i * 190) % (area.Width + 160) - 160;
                var y = 18 + i * 34;
                g.FillEllipse(kuas, x, y, 90, 30);
                g.FillEllipse(kuas, x + 28, y - 14, 60, 34);
            }
        }

        private void GambarBurung(Graphics g, Size area)
        {
            using var pena = new Pen(Color.FromArgb(200, 255, 255, 255), 1.6f);
            for (int i = 0; i < 3; i++)
            {
                var x = (t * 0.9f + i * 60) % (area.Width + 60) - 30;
                var y = 30 + i * 12 + (float)Math.Sin(t * 0.05 + i) * 4;
                var kepak = (float)Math.Sin(t * 0.35 + i) * 4;
                g.DrawLines(pena, new[] { new PointF(x - 7, y - kepak), new PointF(x, y), new PointF(x + 7, y - kepak) });
            }
        }

        private void GambarKabut(Graphics g, Size area)
        {
            for (int i = 0; i < 4; i++)
            {
                var x = (float)(Math.Sin(t * 0.01 + i) * 40) - 40;
                using var kuas = new SolidBrush(Color.FromArgb(50, 255, 255, 255));
                g.FillRectangle(kuas, x, 40 + i * 32, area.Width + 80, 14);
            }
        }

        private void GambarPanas(Graphics g, Size area)
        {
            // Cahaya matahari berdenyut di belakang ikon cuaca
            var denyut = (float)(1 + 0.08 * Math.Sin(t * 0.08));
            var r = 70 * denyut;
            using (var jalur = new GraphicsPath())
            {
                jalur.AddEllipse(418 - r, 88 - r, r * 2, r * 2);
                using var cahaya = new PathGradientBrush(jalur)
                {
                    CenterColor = Color.FromArgb(jenis == Jenis.SangatPanas ? 120 : 80, 255, 200, 60),
                    SurroundColors = new[] { Color.FromArgb(0, 255, 200, 60) }
                };
                g.FillPath(cahaya, jalur);
            }

            // Gelombang panas naik dari tanah
            var jumlah = jenis == Jenis.SangatPanas ? 6 : 3;
            using var pena = new Pen(Color.FromArgb(90, 255, 230, 190), 2f);
            for (int i = 0; i < jumlah; i++)
            {
                var x0 = 200 + i * 28;
                var titik = new PointF[8];
                for (int k = 0; k < titik.Length; k++)
                    titik[k] = new PointF(x0 + (float)Math.Sin(k + t * 0.15 + i) * 4, area.Height - 8 - k * 9 - (t + i * 11) % 40);
                g.DrawCurve(pena, titik);
            }
        }

        private void GambarDaun(Graphics g)
        {
            using var kuas = new SolidBrush(Color.FromArgb(200, 170, 214, 120));
            foreach (var (x, y, f) in daun)
            {
                var state = g.Save();
                g.TranslateTransform(x, y);
                g.RotateTransform((t * 9 + f * 60) % 360);
                g.FillEllipse(kuas, -5, -2.5f, 10, 5);
                g.Restore(state);
            }
        }

        // Siluet orang berjalan: kepala, badan, kaki berayun, plus perlengkapan sesuai cuaca
        private void GambarOrang(Graphics g, Size area)
        {
            var state = g.Save();
            g.TranslateTransform(orangX, area.Height - 6);
            if (arah < 0) g.ScaleTransform(-1, 1);
            if (berangin) g.RotateTransform(-7);   // condong melawan angin

            using var kuas = new SolidBrush(Siluet);
            using var pena = new Pen(Siluet, 4.5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };

            // Seberapa aktif melangkah (0 saat berbalik arah, 1 saat berjalan penuh)
            var aktif = (float)Math.Clamp(Math.Abs(kecepatanX) / 12, 0, 1);
            var ayun = (float)Math.Sin(faseLangkah) * aktif;
            var naik = -Math.Abs((float)Math.Cos(faseLangkah)) * 1.6f * aktif;   // badan naik-turun tiap langkah
            g.TranslateTransform(0, naik);

            GambarKaki(g, pena, ayun);
            GambarKaki(g, pena, -ayun);
            g.FillRectangle(kuas, -5, -46, 10, 25);          // badan
            g.FillEllipse(kuas, -6, -60, 12, 12);            // kepala

            switch (jenis)
            {
                case Jenis.Hujan:
                case Jenis.Badai:
                case Jenis.SangatPanas:
                {
                    // Payung: pelindung hujan, atau peneduh saat sangat panas
                    var goyang = (float)Math.Sin(t * (jenis == Jenis.Badai ? 0.3 : 0.1)) * (jenis == Jenis.Badai ? 4 : 1.5f);
                    using var gagang = new Pen(Siluet, 2f);
                    g.DrawLine(pena, 0, -42, 6, -40);
                    g.DrawLine(gagang, 6, -40, 6 + goyang, -78);
                    var warnaPayung = jenis == Jenis.SangatPanas ? Color.FromArgb(240, 239, 83, 80) : Color.FromArgb(240, 255, 193, 7);
                    using var kain = new SolidBrush(warnaPayung);
                    g.FillPie(kain, -18 + goyang, -92, 48, 30, 180, 180);
                    break;
                }
                case Jenis.Panas:
                {
                    g.FillEllipse(kuas, -13, -61, 26, 6);       // topi lebar
                    g.FillEllipse(kuas, -6, -68, 12, 9);
                    g.DrawLine(pena, 0, -42, 8, -48);           // tangan memegang botol
                    using var botol = new SolidBrush(Color.FromArgb(230, 100, 181, 246));
                    g.FillRectangle(botol, 6, -58, 5, 12);
                    // Keringat menetes
                    var jatuh = (int)(t % 24);
                    using var keringat = new SolidBrush(Color.FromArgb(Math.Max(0, 220 - jatuh * 9), 160, 210, 255));
                    g.FillEllipse(keringat, -11, -58 + jatuh * 0.7f, 3, 4);
                    break;
                }
                case Jenis.Cerah:
                case Jenis.Berawan:
                    GambarLengan(g, pena, -ayun);
                    g.FillEllipse(kuas, -9, -61, 18, 5);        // topi
                    g.FillRectangle(kuas, -6, -66, 12, 7);
                    break;
                default:
                    GambarLengan(g, pena, -ayun);
                    break;
            }
            g.Restore(state);
        }

        // Kaki dua ruas (paha + betis) dengan lutut yang menekuk saat kaki di belakang
        private static void GambarKaki(Graphics g, Pen pena, float ayun)
        {
            var sudutPaha = ayun * 0.5;
            var tekuk = Math.Max(0, -ayun) * 0.7;
            var lututX = (float)(Math.Sin(sudutPaha) * 11);
            var lututY = -23 + (float)(Math.Cos(sudutPaha) * 11);
            var kakiX = lututX + (float)(Math.Sin(sudutPaha - tekuk) * 12);
            var kakiY = lututY + (float)(Math.Cos(sudutPaha - tekuk) * 12);
            g.DrawLines(pena, new[] { new PointF(0, -23), new PointF(lututX, lututY), new PointF(kakiX, kakiY) });
        }

        // Lengan berayun berlawanan dengan kaki
        private static void GambarLengan(Graphics g, Pen pena, float ayun)
        {
            var sudut = ayun * 0.6;
            using var tipis = new Pen(pena.Color, 3.5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
            g.DrawLine(tipis, 0, -43, (float)(Math.Sin(sudut) * 15), -43 + (float)(Math.Cos(sudut) * 15));
        }
    }
}
