# SWASANA
Sistem Waspada Suasana & Adaptasi Iklim

JUNPRO6
Ketua Kelompok: Marcelinus Dinoglide Yoga Prakoso - 24/533842/TK/59152
Anggota 1: Muhammad Izzuddin Prakoso - 24/537712/TK/59618
Anggota 2: Raka Bagus Samudra - 24/543213/TK/60349

## Fitur (MVP Modul 10)

- Login per username (dengan konfirmasi sebelum akun baru dibuat), setiap user punya riwayat sendiri
- Cek cuaca terkini lewat **Open-Meteo Web API**: saran lokasi saat mengetik, tombol **Lokasi saya** (layanan lokasi Windows), prakiraan per jam & 7 hari, suhu terasa, indeks UV, kualitas udara (AQI)
- **Lokasi favorit** (kolom `is_favorite` di tabel `locations`) untuk cek cuaca sekali klik
- Penilaian risiko Aman / Waspada / Bahaya lewat 6 turunan `RiskRule` (inheritance + polymorphism): suhu, hujan, hujan + angin, UV, heat index, kualitas udara
- Kartu saran tindakan dari tabel `recommendations`, dengan ikon per kategori
- Riwayat pengecekan tersimpan di PostgreSQL, bisa dicari per lokasi, disaring per status, diurutkan per kolom, dan diekspor ke CSV, dengan grafik tren suhu harian dari `weather_data`
- Database & tabel dibuat otomatis dari aplikasi
- Tampilan bergaya aplikasi cuaca: ikon Meteocons, latar langit sesuai cuaca, adegan animasi (hujan, panas, badai, angin, malam), kartu risiko berputar 3D, transisi FluentTransitions. Animasi mati otomatis kalau efek animasi Windows dimatikan

Panduan instalasi, konfigurasi database, dan cara pakai: **[PANDUAN.md](PANDUAN.md)**.
Hasil pengujian: **[PENGUJIAN.md](PENGUJIAN.md)**.

## Struktur

- `Swasana/`: aplikasi Windows Forms (.NET 10)
  - `Form*.cs`: Login, Koneksi database, Menu, Cek cuaca, Riwayat
  - `Tema.cs`, `Ikon.cs`, `Animasi.cs`, `Flip3D.cs`, `AdeganCuaca.cs`: warna, font, ikon, animasi
  - `KomponenCuaca.cs`, `GrafikTren.cs`, `PanelGandaBuffer.cs`: komponen tampilan (item prakiraan, kartu saran, grafik tren)
  - `Ikon/`: ikon Meteocons (MIT, lihat `Ikon/LICENSE-meteocons.txt`)
  - `Data/`: `DatabaseContext` (Npgsql), `BaseRepository` + turunannya, `DatabaseSetup` (buat database & skema)
  - `Models/`: class dari class diagram
  - `Services/`: `WeatherService` (Open-Meteo + reverse geocoding), `LokasiPerangkat` (GPS Windows), `RiskRule`/`RiskEvaluator`, `ActionRecommender`, `WeatherHistoryManager`, `FormatCuaca`, `EksporRiwayat`
- `Swasana.Tests/`: unit test xUnit (`dotnet test Swasana.Tests`)
- `database/swasana_schema.sql`: skema final (ikut tertanam di aplikasi)
- `database/swasana_db_creation.sql`, `swasana_db_tambahan.sql`: skrip Modul 5
