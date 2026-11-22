namespace Swasana
{
    public class KomponenCuaca
    {
        private readonly Control induk;

        public KomponenCuaca(Control induk)
        {
            this.induk = induk;
        }

        public int Px(int nilai) => induk.LogicalToDeviceUnits(nilai);

        public Label Petunjuk(string teks, int lebar) => new Label
        {
            Text = teks, ForeColor = Tema.Redup, Font = Tema.HurufBesar,
            AutoSize = false, Size = new Size(Px(lebar), Px(40))
        };

        public Panel ItemPrakiraan(string waktu, string ikon, string suhu, Font fontSuhu, int peluangHujan, int lebar)
        {
            var item = new Panel { Size = new Size(Px(lebar), Px(78)), Margin = new Padding(Px(1), 0, Px(1), 0) };
            item.Controls.Add(Teks(waktu, Tema.HurufKecil, Tema.Redup, lebar, 15, 0));
            item.Controls.Add(new PictureBox
            {
                Image = Ikon.Muat(ikon), SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(Px(34), Px(34)), Location = new Point(Px((lebar - 34) / 2), Px(15))
            });
            item.Controls.Add(Teks(suhu, fontSuhu, Tema.Tinta, lebar, 16, 48));
            item.Controls.Add(Teks($"{peluangHujan}%", Tema.HurufKecil, Tema.Hujan, lebar, 14, 64));
            return item;
        }

        public Panel Kartu(string kategori, string teks, Color warna)
        {
            var lebarIsi = Px(226);
            var isi = new Label
            {
                Text = teks, Font = Tema.HurufBesar, ForeColor = Tema.Tinta,
                AutoSize = false, Location = new Point(Px(58), Px(24)), MaximumSize = new Size(lebarIsi, 0)
            };
            var ukuran = TextRenderer.MeasureText(teks, isi.Font, new Size(lebarIsi, 0), TextFormatFlags.WordBreak);
            isi.Size = new Size(lebarIsi, ukuran.Height + Px(2));

            var k = new Panel
            {
                BackColor = Color.White,
                Size = new Size(Px(294), Math.Max(Px(64), isi.Bottom + Px(10))),
                Margin = new Padding(0, 0, 0, Px(8))
            };
            var garis = Px(5);
            k.Paint += (s, e) => { using var kuas = new SolidBrush(warna); e.Graphics.FillRectangle(kuas, 0, 0, garis, k.Height); };
            k.Controls.Add(new PictureBox
            {
                Image = Ikon.Muat(IkonKategori(kategori)), SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(Px(38), Px(38)), Location = new Point(Px(13), (k.Height - Px(38)) / 2)
            });
            k.Controls.Add(new Label
            {
                Text = char.ToUpper(kategori[0]) + kategori[1..], Font = Tema.HurufKategori,
                ForeColor = Tema.Redup, AutoSize = true, Location = new Point(Px(57), Px(6))
            });
            k.Controls.Add(isi);
            return k;
        }

        private Label Teks(string teks, Font font, Color warna, int lebar, int tinggi, int y) => new Label
        {
            Text = teks, Font = font, ForeColor = warna, AutoSize = false,
            Size = new Size(Px(lebar), Px(tinggi)), Location = new Point(0, Px(y)), TextAlign = ContentAlignment.MiddleCenter
        };

        public static string IkonKategori(string kategori) => kategori switch
        {
            "aktivitas" => "sun-hot",
            "kesehatan" => "thermometer-water",
            "perlindungan diri" => "umbrella",
            _ => "not-available"
        };
    }
}
