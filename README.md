# SWASANA
Sistem Waspada Suasana & Adaptasi Iklim

JUNPRO6
Ketua Kelompok: Marcelinus Dinoglide Yoga Prakoso - 24/533842/TK/59152
Anggota 1: Muhammad Izzuddin Prakoso - 24/537712/TK/59618
Anggota 2: Raka Bagus Samudra - 24/543213/TK/60349

## Fitur (MVP Modul 10)

- Login per username, setiap user punya riwayat sendiri
- Cek cuaca terkini dari **Open-Meteo Web API** (cari koordinat dari nama kota + data cuaca)
- Penilaian risiko Aman / Waspada / Bahaya lewat aturan `RiskRule` (inheritance + polymorphism)
- Rekomendasi tindakan dari tabel `recommendations`
- Riwayat pengecekan tersimpan di PostgreSQL, bisa dicari per lokasi
- Database & tabel dibuat otomatis dari aplikasi (Pengaturan Database)

Panduan instalasi, konfigurasi database, dan cara pakai: **[PANDUAN.md](PANDUAN.md)**.
Hasil pengujian: **[PENGUJIAN.md](PENGUJIAN.md)**.

## Struktur

- `Swasana/`: aplikasi Windows Forms (.NET 10)
  - `Form*.cs`: Login, Pengaturan Database, Menu Utama, Cek Cuaca, Riwayat
  - `Data/`: `DatabaseContext` (Npgsql), `BaseRepository` + turunannya, `DatabaseSetup` (buat database & skema)
  - `Models/`: class dari class diagram
  - `Services/`: `WeatherService` (Open-Meteo), `RiskRule`/`RiskEvaluator`, `ActionRecommender`, `WeatherHistoryManager`
- `Swasana.Tests/`: unit test xUnit (`dotnet test Swasana.Tests`)
- `database/swasana_schema.sql`: skema final (ikut tertanam di aplikasi)
- `database/swasana_db_creation.sql`, `swasana_db_tambahan.sql`: skrip Modul 5
