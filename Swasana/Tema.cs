using Swasana.Models;

namespace Swasana
{
    // Warna dan gaya bersama semua form, supaya tampilan konsisten
    public static class Tema
    {
        public static readonly Color Langit = Color.FromArgb(15, 76, 92);      // header & tombol utama
        public static readonly Color Kabut = Color.FromArgb(243, 246, 248);    // latar form
        public static readonly Color Tinta = Color.FromArgb(28, 43, 54);       // teks utama
        public static readonly Color Redup = Color.FromArgb(94, 110, 122);     // teks keterangan
        public static readonly Color Garis = Color.FromArgb(200, 212, 219);    // border
        public static readonly Color Kosong = Color.FromArgb(214, 223, 229);   // panel status sebelum ada data

        public static Color WarnaStatus(RiskLevel level) => level switch
        {
            RiskLevel.Bahaya => Color.FromArgb(198, 40, 40),
            RiskLevel.Waspada => Color.FromArgb(232, 163, 23),
            _ => Color.FromArgb(46, 125, 50)
        };

        // Teks gelap di atas kuning supaya tetap terbaca
        public static Color TeksStatus(RiskLevel level) => level == RiskLevel.Waspada ? Tinta : Color.White;
    }
}
