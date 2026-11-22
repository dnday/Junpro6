using Windows.Devices.Geolocation;

namespace Swasana.Services;

// Lokasi perangkat dari layanan lokasi Windows (GPS, Wi-Fi, atau jaringan)
public static class LokasiPerangkat
{
    public static async Task<(double Latitude, double Longitude)> AmbilAsync()
    {
        var izin = await Geolocator.RequestAccessAsync();
        if (izin != GeolocationAccessStatus.Allowed)
            throw new InvalidOperationException(
                "Akses lokasi ditolak. Aktifkan di Settings > Privacy & security > Location, lalu coba lagi.");

        var posisi = await new Geolocator().GetGeopositionAsync(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(15));
        var titik = posisi.Coordinate.Point.Position;
        return (titik.Latitude, titik.Longitude);
    }
}
