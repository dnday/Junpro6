# SWASANA
Sistem Waspada Suasana & Adaptasi Iklim

JUNPRO6
Ketua Kelompok: Marcelinus Dinoglide Yoga Prakoso - 24/533842/TK/59152
Anggota 1: Muhammad Izzuddin Prakoso - 24/537712/TK/59618
Anggota 2: Raka Bagus Samudra - 24/543213/TK/60349

## Menjalankan (Modul 8: PostgreSQL to C#)

1. Di pgAdmin buat database: `CREATE DATABASE swasana_db;`
2. Buka Query Tool di `swasana_db`, jalankan berurutan:
   - `database/swasana_db_creation.sql` (tabel Modul 5)
   - `database/swasana_db_tambahan.sql` (constraint + seed data)
3. Jalankan aplikasi:
   ```
   cd Swasana
   dotnet run
   dotnet run -- Sleman -7.7162 110.3555
   ```
   Program akan meminta password user `postgres`. Bisa juga diisi lewat environment variable `SWASANA_DB`, contoh:
   `Host=localhost;Port=5432;Database=swasana_db;Username=postgres;Password=xxx`

Struktur `Swasana/`:
- `Data/DatabaseContext.cs`: koneksi Npgsql (connect, executeQuery, executeScalar, executeNonQuery)
- `Data/BaseRepository.cs`: abstract class induk repository (GetAll, GetById diwarisi; Insert, MapRow di-override)
- `Data/UserRepository.cs`, `Data/LocationRepository.cs`: turunan untuk tabel `users` dan `locations`
- `Models/`, `Services/`: class dari class diagram (`classDiagram/classCode.cs`)
