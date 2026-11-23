using Swasana.Models;

namespace Swasana
{
    // Warna dan gaya bersama semua form, supaya tampilan konsisten
    public static class Tema
    {
        // Ikon aplikasi (juga dipakai exe lewat ApplicationIcon di Swasana.csproj)
        public static readonly Icon IkonAplikasi = new Icon(typeof(Tema).Assembly.GetManifestResourceStream("swasana.ico")!);

        public static readonly Color Langit = Color.FromArgb(15, 76, 92);      // header & tombol utama
        public static readonly Color Kabut = Color.FromArgb(243, 246, 248);    // latar form
        public static readonly Color Tinta = Color.FromArgb(28, 43, 54);       // teks utama
        public static readonly Color Redup = Color.FromArgb(94, 110, 122);     // teks keterangan
        public static readonly Color Garis = Color.FromArgb(200, 212, 219);    // border
        public static readonly Color Kosong = Color.FromArgb(214, 223, 229);   // panel status sebelum ada data
        public static readonly Color Galat = Color.FromArgb(198, 40, 40);
        public static readonly Color Hujan = Color.FromArgb(30, 100, 200);
        public static readonly Color HoverUtama = Color.FromArgb(28, 104, 124);
        public static readonly Color HoverSekunder = Color.FromArgb(231, 241, 244);
        public static readonly Color Terpilih = Color.FromArgb(214, 232, 238);
        public static readonly Color WaspadaGelap = Color.FromArgb(150, 100, 0);   // kuning gelap supaya terbaca di latar putih

        public static readonly Font HurufKecil = new Font("Segoe UI", 8.25F);
        public static readonly Font HurufSedang = new Font("Segoe UI", 9F);
        public static readonly Font HurufBesar = new Font("Segoe UI", 9.75F);
        public static readonly Font HurufKategori = new Font("Segoe UI Semibold", 8.25F);
        public static readonly Font HurufTebal = new Font("Segoe UI Semibold", 9.75F);
        public static readonly Font HurufSuhuHarian = new Font("Bahnschrift SemiBold", 9F);
        public static readonly Font HurufSuhuJam = new Font("Bahnschrift SemiBold", 10F);
        public static readonly Font HurufBintang = new Font("Segoe UI Symbol", 12F);

        public static Color WarnaStatus(RiskLevel level) => level switch
        {
            RiskLevel.Bahaya => Galat,
            RiskLevel.Waspada => Color.FromArgb(232, 163, 23),
            _ => Color.FromArgb(46, 125, 50)
        };

        // Teks gelap di atas kuning supaya tetap terbaca
        public static Color TeksStatus(RiskLevel level) => level == RiskLevel.Waspada ? Tinta : Color.White;

        // Semua pasangan warna diuji kontrasnya terhadap teks putih: minimal 4,5:1 (WCAG AA)
        public static (Color Atas, Color Bawah) WarnaLangit(int kode, bool siang)
        {
            if (!siang) return (Color.FromArgb(18, 30, 62), Color.FromArgb(44, 62, 104));                 // malam   10,5:1
            if (kode >= 51) return (Color.FromArgb(48, 62, 80), Color.FromArgb(72, 90, 110));             // hujan    7,1:1
            if (kode == 3 || kode == 45 || kode == 48) return (Color.FromArgb(62, 82, 102), Color.FromArgb(84, 104, 124)); // mendung 5,8:1
            return (Color.FromArgb(21, 96, 170), Color.FromArgb(36, 114, 186));                           // cerah    5,0:1
        }

        public static void Pesan(Label label, string teks, bool error = true)
        {
            label.ForeColor = error ? Galat : Redup;
            label.Text = teks;
        }

        public static void BatasiKeLayar(Form form)
        {
            form.AutoScroll = true;
            form.MaximumSize = Screen.FromControl(form).WorkingArea.Size;
        }
    }
}
