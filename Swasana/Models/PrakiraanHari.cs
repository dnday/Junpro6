namespace Swasana.Models;

// Satu hari prakiraan dari Open-Meteo (hanya untuk tampilan, tidak disimpan)
public record PrakiraanHari(DateTime Tanggal, double SuhuMaks, double SuhuMin, int PeluangHujan, int KodeCuaca);
