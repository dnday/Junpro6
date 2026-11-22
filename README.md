# SWASANA
Sistem Waspada Suasana & Adaptasi Iklim

JUNPRO6
Ketua Kelompok: Marcelinus Dinoglide Yoga Prakoso - 24/533842/TK/59152
Anggota 1: Muhammad Izzuddin Prakoso - 24/537712/TK/59618
Anggota 2: Raka Bagus Samudra - 24/543213/TK/60349

## Fitur (MVP Modul 10)

- Login per username, setiap user punya riwayat sendiri
- Cek cuaca terkini lewat **Open-Meteo Web API**: saran lokasi saat mengetik, tombol **Lokasi saya** (layanan lokasi Windows), prakiraan 8 jam, suhu terasa, indeks UV
- Penilaian risiko Aman / Waspada / Bahaya lewat aturan `RiskRule` (inheritance + polymorphism)
- Kartu saran tindakan dari tabel `recommendations`, dengan ikon per kategori
- Riwayat pengecekan tersimpan di PostgreSQL, bisa dicari per lokasi
- Database & tabel dibuat otomatis dari aplikasi
- Tampilan bergaya aplikasi cuaca: ikon Meteocons, latar langit sesuai cuaca, adegan animasi (hujan, panas, badai, angin, malam), kartu risiko berputar 3D, transisi FluentTransitions. Animasi mati otomatis kalau efek animasi Windows dimatikan

Panduan instalasi, konfigurasi database, dan cara pakai: **[PANDUAN.md](PANDUAN.md)**.
Hasil pengujian: **[PENGUJIAN.md](PENGUJIAN.md)**.

## Struktur

- `Swasana/`: aplikasi Windows Forms (.NET 10)
  - `Form*.cs`: Login, Koneksi database, Menu, Cek cuaca, Riwayat
  - `Tema.cs`, `Ikon.cs`, `Animasi.cs`, `Flip3D.cs`, `AdeganCuaca.cs`: warna, ikon, animasi
  - `Ikon/`: ikon Meteocons (MIT, lihat `Ikon/LICENSE-meteocons.txt`)
  - `Data/`: `DatabaseContext` (Npgsql), `BaseRepository` + turunannya, `DatabaseSetup` (buat database & skema)
  - `Models/`: class dari class diagram
  - `Services/`: `WeatherService` (Open-Meteo + reverse geocoding), `LokasiPerangkat` (GPS Windows), `RiskRule`/`RiskEvaluator`, `ActionRecommender`, `WeatherHistoryManager`
- `Swasana.Tests/`: unit test xUnit (`dotnet test Swasana.Tests`)
- `database/swasana_schema.sql`: skema final (ikut tertanam di aplikasi)
- `database/swasana_db_creation.sql`, `swasana_db_tambahan.sql`: skrip Modul 5
