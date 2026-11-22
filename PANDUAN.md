# Panduan SWASANA

SWASANA (Sistem Waspada Suasana & Adaptasi Iklim) adalah aplikasi desktop yang mengambil cuaca terkini dari **Open-Meteo Web API**, menilai tingkat risikonya (**Aman / Waspada / Bahaya**), memberi saran tindakan, dan menyimpan riwayat pengecekan ke **PostgreSQL**.

## 1. Kebutuhan

| Komponen | Keterangan |
|---|---|
| Windows 10/11 64-bit | .NET **tidak** perlu diinstal, sudah ikut di dalam `Swasana.exe` |
| PostgreSQL 14 atau lebih baru | Unduh di https://www.postgresql.org/download/windows/ dan ingat password user `postgres` |
| Koneksi internet | Untuk mengambil data cuaca dari Open-Meteo |

## 2. Instalasi

1. Instal PostgreSQL (biarkan port **5432**), lalu catat password user `postgres`.
2. Ekstrak `SWASANA.zip` ke folder mana saja, misalnya `D:\SWASANA`.
3. Jalankan `Swasana.exe`. Tidak ada langkah instalasi lain.

> Jika muncul peringatan Windows SmartScreen, klik **More info → Run anyway** (aplikasi belum ditandatangani digital).

## 3. Konfigurasi database (sekali saja)

Saat pertama dijalankan muncul jendela **Pengaturan Database**:

| Isian | Nilai umum |
|---|---|
| Host | `localhost` |
| Port | `5432` |
| Database | `swasana_db` |
| Username | `postgres` |
| Password | password PostgreSQL Anda |

Klik **Tes & Simpan**. Aplikasi akan:
- membuat database `swasana_db` jika belum ada,
- membuat semua tabel, constraint, dan data awal (tingkat risiko + rekomendasi),
- menyimpan pengaturan di `%APPDATA%\SWASANA\koneksi.txt`.

Pengaturan bisa diubah kapan saja lewat tautan **Pengaturan Database** di jendela login.

Alternatif tanpa jendela: isi environment variable `SWASANA_DB`, contoh
`Host=localhost;Port=5432;Database=swasana_db;Username=postgres;Password=xxx`.

Skema SQL lengkap ada di `database/swasana_schema.sql` jika ingin dijalankan manual lewat pgAdmin.

## 4. Cara pakai

1. **Login**: ketik username lalu **Masuk**. Username baru otomatis didaftarkan, dan setiap username punya riwayat sendiri.
2. **Menu Utama**: pilih **Cek Cuaca**, **Riwayat**, **Tentang**, atau **Keluar**.
3. **Cek Cuaca**:
   - Ketik nama kota (misalnya `Sleman`), lalu klik **Cari Koordinat**. Latitude dan longitude terisi otomatis dari Open-Meteo Geocoding.
   - Klik **Cek Cuaca**. Hasilnya menampilkan suhu, kelembapan, angin, dan curah hujan, status risiko berwarna, rincian tiap aturan, serta rekomendasi.
   - Bisa juga langsung klik **Cek Cuaca** tanpa mencari koordinat, atau mengisi latitude/longitude sendiri (koma atau titik sama-sama diterima).
   - Setiap hasil otomatis tersimpan ke riwayat.
4. **Riwayat**: menampilkan semua pengecekan milik user yang sedang login, terbaru di atas. Ketik nama lokasi lalu **Cari** untuk menyaring.

### Aturan risiko

| Status | Kondisi |
|---|---|
| Bahaya | suhu > 35 °C, **atau** hujan ≥ 10 mm/jam **dan** angin ≥ 45 km/jam |
| Waspada | suhu > 33 °C, **atau** hujan ≥ 5 mm/jam |
| Aman | selain kondisi di atas |

## 5. Masalah umum

| Pesan | Penyebab dan solusi |
|---|---|
| *Gagal terhubung ke PostgreSQL: password authentication failed* | Password salah. Ulangi di Pengaturan Database. |
| *Failed to connect to 127.0.0.1:5432* | Service PostgreSQL belum jalan. Buka `services.msc` lalu start `postgresql-x64-xx`. |
| *Gagal mengambil data dari Open-Meteo* | Tidak ada internet atau koneksi diblokir. |
| *Lokasi "..." tidak ditemukan* | Periksa ejaan nama kota, atau isi latitude/longitude manual. |

## 6. Untuk pengembang

```
dotnet run --project Swasana          # jalankan dari source
dotnet test Swasana.Tests             # unit test
dotnet publish Swasana -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -o publish/SWASANA
```
