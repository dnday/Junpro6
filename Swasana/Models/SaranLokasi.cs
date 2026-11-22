namespace Swasana.Models;

// Satu saran lokasi dari Open-Meteo Geocoding
public record SaranLokasi(string Nama, string Wilayah, string Negara, string KodeNegara, double Latitude, double Longitude)
{
    // Nama yang disimpan & ditampilkan, contoh "Sleman, Yogyakarta"
    public string NamaLengkap => Wilayah.Length > 0 && Wilayah != Nama ? $"{Nama}, {Wilayah}" : Nama;

    // Baris kedua di daftar saran
    public string Keterangan => string.Join(", ", new[] { Wilayah, Negara }.Where(x => x.Length > 0));
}
