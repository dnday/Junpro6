namespace Swasana.Models;

// Satu jam prakiraan dari Open-Meteo (hanya untuk tampilan, tidak disimpan)
public record PrakiraanJam(DateTime Waktu, double Suhu, int PeluangHujan, int KodeCuaca, bool Siang);
