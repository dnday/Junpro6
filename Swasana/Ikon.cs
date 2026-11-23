using System.Reflection;
using Swasana.Models;

namespace Swasana
{
    // Ikon cuaca Meteocons (MIT, github.com/basmilius/meteocons), tertanam di dalam .exe
    public static class Ikon
    {
        private static readonly Dictionary<string, Image> cache = new Dictionary<string, Image>();

        public static Image Muat(string nama)
        {
            if (!cache.TryGetValue(nama, out var gambar))
            {
                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Ikon.{nama}.png")
                    ?? Assembly.GetExecutingAssembly().GetManifestResourceStream("Ikon.not-available.png")!;
                gambar = Image.FromStream(stream);
                cache[nama] = gambar;
            }
            return gambar;
        }

        // Ikon cuaca beranimasi: frame dari animasi resmi Meteocons (@meteocons/svg, MIT) yang dirender
        // menjadi lembar 10 kolom, 168 px per frame, 15 fps, berulang tiap 6 detik.
        // Hanya frame ikon yang sedang tampil yang disimpan di memori. Null kalau ikon tidak punya animasi.
        private const int UkuranFrame = 168, FpsAnimasi = 15;
        private static (string Nama, Bitmap[] Frame)? animasi;

        public static Image? FrameAnimasi(string nama, double detik)
        {
            if (animasi?.Nama != nama)
            {
                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Animasi.{nama}.png");
                if (stream == null) return null;
                using var lembar = new Bitmap(stream);
                int jumlah = lembar.Width / UkuranFrame * (lembar.Height / UkuranFrame);
                var frame = new Bitmap[jumlah];
                for (int i = 0; i < jumlah; i++)
                    frame[i] = lembar.Clone(new Rectangle(i % 10 * UkuranFrame, i / 10 * UkuranFrame, UkuranFrame, UkuranFrame), lembar.PixelFormat);
                if (animasi is { } lama) foreach (var b in lama.Frame) b.Dispose();
                animasi = (nama, frame);
            }
            var f = animasi.Value.Frame;
            return f[(int)(detik * FpsAnimasi) % f.Length];
        }

        public static Image Status(RiskLevel level) => Muat(level switch
        {
            RiskLevel.Bahaya => "code-red",
            RiskLevel.Waspada => "code-yellow",
            _ => "code-green"
        });

        // Kode cuaca WMO dari Open-Meteo -> nama ikon
        public static string NamaCuaca(int kode, bool siang) => kode switch
        {
            0 => siang ? "clear-day" : "clear-night",
            1 or 2 => siang ? "partly-cloudy-day" : "partly-cloudy-night",
            3 => siang ? "overcast-day" : "overcast-night",
            45 or 48 => siang ? "fog-day" : "fog-night",
            >= 51 and <= 57 => "drizzle",
            65 or 67 or 82 => "extreme-rain",
            >= 61 and <= 67 or >= 80 and <= 82 => "rain",
            >= 71 and <= 77 or 85 or 86 => "snow",
            >= 95 => "thunderstorms-rain",
            _ => "not-available"
        };

        // Keterangan singkat kode cuaca WMO dalam bahasa Indonesia
        public static string KeteranganCuaca(int kode) => kode switch
        {
            0 => "Cerah",
            1 => "Cerah berawan",
            2 => "Berawan sebagian",
            3 => "Mendung",
            45 or 48 => "Berkabut",
            >= 51 and <= 57 => "Gerimis",
            61 or 80 => "Hujan ringan",
            63 or 81 => "Hujan sedang",
            65 or 67 or 82 => "Hujan lebat",
            66 => "Hujan beku",
            >= 71 and <= 77 or 85 or 86 => "Salju",
            >= 95 => "Badai petir",
            _ => "Kondisi tidak diketahui"
        };
    }
}
