# Panduan SWASANA

SWASANA (Sistem Waspada Suasana & Adaptasi Iklim) adalah aplikasi desktop untuk mengecek cuaca terkini lokasi mana pun lewat **Open-Meteo Web API**. Aplikasi menilai tingkat risikonya (**Aman / Waspada / Bahaya**), memberi saran tindakan, dan menyimpan riwayat pengecekan ke **PostgreSQL**.

## 1. Kebutuhan

| Komponen | Keterangan |
|---|---|
| Windows 10 (versi 2004+) / 11, 64-bit | .NET **tidak** perlu diinstal, sudah ikut di dalam `Swasana.exe` |
| Database PostgreSQL | PostgreSQL 14 atau lebih baru di komputer sendiri, **atau** database online (misalnya Supabase) |
| Koneksi internet | Untuk data cuaca dan pencarian lokasi |

## 2. Instalasi

1. Ekstrak `SWASANA.zip` ke folder mana saja, misalnya `D:\SWASANA`.
2. Jalankan `Swasana.exe`. Saat pertama dibuka, isi koneksi database (lihat bagian 3), lalu masuk dengan username.

> Jika muncul peringatan Windows SmartScreen, klik **More info → Run anyway** (aplikasi belum ditandatangani digital).

## 3. Konfigurasi database

Saat pertama dibuka, aplikasi menampilkan form **Koneksi database**. Isi dengan PostgreSQL milik sendiri (lihat **Memakai database sendiri**) atau tempel URI database online (lihat **Memakai database online (Supabase)**). Pengaturan tersimpan dan bisa diubah lagi lewat **Atur koneksi database**.

Beberapa build bisa membawa database online bawaan. Pada build seperti itu aplikasi langsung tersambung, dan tautan **Pakai database online bawaan** muncul di form koneksi.

> Untuk pengembang: koneksi bawaan dibaca dari `Swasana/koneksi-bawaan.txt` (tidak ikut di repo karena berisi password) dan ditanam ke exe saat build. Akun aplikasinya dibuat dengan `database/supabase_akun_aplikasi.sql`. Tanpa file itu, exe meminta pengaturan koneksi seperti biasa.

### Memakai database sendiri

Saat pertama dijalankan muncul jendela **Koneksi database**:

| Isian | Nilai umum |
|---|---|
| Host | `localhost` |
| Port | `5432` |
| Database | `swasana_db` |
| Username | `postgres` |
| Password | password PostgreSQL Anda |

Klik **Sambungkan**. Aplikasi akan:
- membuat database `swasana_db` jika belum ada,
- membuat semua tabel, constraint, dan data awal (tingkat risiko + rekomendasi),
- menyimpan pengaturan di `%APPDATA%\SWASANA\koneksi.txt`.

Pengaturan bisa diubah kapan saja lewat tautan **Atur koneksi database** di jendela masuk.

Alternatif tanpa jendela: isi environment variable `SWASANA_DB`, contoh
`Host=localhost;Port=5432;Database=swasana_db;Username=postgres;Password=xxx`.

### Memakai database online (Supabase)

Supaya semua pengguna memakai satu database bersama dan tidak perlu menginstal PostgreSQL:

1. Buat project di [supabase.com](https://supabase.com) (paket gratis cukup).
2. Di dashboard klik **Connect**, pilih **Session pooler**, lalu salin URI-nya
   (`postgresql://postgres.<id-project>:[YOUR-PASSWORD]@aws-…pooler.supabase.com:5432/postgres`).
   Jangan pakai *Direct connection*: di paket gratis hanya lewat IPv6.
3. Tempel URI tersebut ke kolom **Host** di jendela Koneksi database. Host, port, database, dan username terisi otomatis.
4. Ketik password database Supabase, lalu **Sambungkan**. Tabel dan data awal dibuat otomatis.

Koneksi ke server online selalu memakai SSL. Semua tabel memakai *Row Level Security* sehingga tidak bisa dibaca lewat API publik Supabase, hanya lewat aplikasi ini.

> Catatan keamanan: password database tersimpan di setiap komputer yang memakai aplikasi. Bagikan hanya kepada pengguna yang dipercaya.

Skema SQL lengkap ada di `swasana_schema.sql` jika ingin dijalankan manual lewat pgAdmin.

## 4. Cara pakai

1. **Masuk**: ketik username lalu **Masuk**. Kalau username belum terdaftar, muncul konfirmasi untuk membuat akun baru (pilih **Tidak** kalau hanya salah ketik). Setiap username punya riwayat sendiri.
2. **Menu**: pilih **Cek cuaca**, **Riwayat**, **Tentang SWASANA**, atau **Keluar**.
3. **Cek cuaca**:
   - Lokasi yang terakhir Anda cek langsung ditampilkan cuacanya saat halaman dibuka (tidak ditambahkan lagi ke riwayat sampai Anda menekan **Cek cuaca**).
   - Lokasi **favorit** tampil sebagai tombol di bawah kotak lokasi: klik sekali untuk langsung mengecek cuacanya. Tekan **☆** untuk menyimpan lokasi yang sedang dipilih (berubah menjadi ★), tekan lagi untuk menghapusnya. Favorit juga bisa dihapus dengan klik kanan atau tombol **Delete** saat tombolnya terpilih.
   - Mulai ketik nama kota, misalnya `yog`. Daftar saran muncul (lokasi Indonesia di atas). Pilih dengan klik, atau dengan panah ↑/↓ lalu Enter, dan cuacanya langsung tampil.
   - Atau klik **Lokasi saya** untuk memakai lokasi perangkat (layanan lokasi Windows harus aktif: *Settings › Privacy & security › Location*).
   - Bisa juga mengisi latitude/longitude sendiri (koma atau titik sama-sama diterima) lalu klik **Cek cuaca**.
   - Hasilnya:
     - panel cuaca dengan suhu, kondisi, dan suhu yang terasa, dengan latar dan animasi yang menyesuaikan cuaca;
     - prakiraan **per jam** (8 jam) atau **7 hari** (suhu maks/min dan peluang hujan), pilih dengan tombol di pojok panel prakiraan;
     - kelembapan, angin, curah hujan, indeks UV, dan kualitas udara (US AQI);
     - **status risiko** beserta alasannya;
     - kartu **saran tindakan**.
   - Setiap hasil otomatis tersimpan ke riwayat.
4. **Riwayat**: grafik **tren suhu maksimum & minimum harian** dari data yang tersimpan, lalu daftar semua pengecekan milik user yang sedang masuk, terbaru di atas. Ketik nama lokasi untuk menyaring langsung (atau tekan **Cari**), dan pilih status risiko di daftar di sebelahnya. Klik judul kolom untuk mengurutkan, arahkan kursor ke grafik untuk melihat suhu maks/min tiap hari, dan klik **Ekspor CSV** untuk menyimpan daftar yang sedang tampil ke file. Jendela ini bisa diperbesar.

### Aturan risiko

Status akhir = tingkat tertinggi dari enam aturan berikut:

| Aturan | Waspada | Bahaya |
|---|---|---|
| Suhu udara | > 33 °C | > 35 °C |
| Curah hujan | ≥ 5 mm/jam | – |
| Hujan + angin | – | hujan ≥ 10 mm/jam **dan** angin ≥ 45 km/jam |
| Indeks UV (skala WHO) | ≥ 8 | ≥ 11 |
| Heat index (suhu + kelembapan, rumus NOAA) | ≥ 32 °C | ≥ 41 °C |
| Kualitas udara (US AQI) | 101–150 | ≥ 151 |

### Animasi

Panel cuaca menampilkan adegan yang mengikuti kondisi cuaca. Contohnya, orang berpayung saat hujan, orang bertopi lebar dengan botol minum saat panas, berteduh di bawah payung saat sangat panas, kilat saat badai, daun beterbangan saat angin kencang, dan bintang di malam hari. Semua animasi otomatis mati kalau *Animation effects* di Windows dimatikan (*Settings › Accessibility › Visual effects*).

## 5. Masalah umum

| Pesan | Penyebab dan solusi |
|---|---|
| *Password PostgreSQL salah* | Masukkan password yang dibuat saat menginstal PostgreSQL. |
| *Tidak bisa tersambung ke PostgreSQL* | Service PostgreSQL belum jalan. Buka `services.msc` lalu start `postgresql-x64-xx`. |
| *Open-Meteo tidak bisa dihubungi* | Tidak ada internet atau koneksi diblokir. |
| *Akses lokasi ditolak* | Aktifkan *Location* dan *Let desktop apps access your location* di Settings Windows. |
| *Lokasi "..." tidak ditemukan* | Periksa ejaan, pilih dari daftar saran, atau isi koordinat manual. |

## 6. Privasi

Koordinat lokasi yang dicek dikirim ke Open-Meteo (data cuaca). Untuk tombol **Lokasi saya**, koordinat juga dikirim ke BigDataCloud untuk mencari nama tempatnya. Riwayat hanya disimpan di database PostgreSQL Anda sendiri.

## 7. Kredit

- Data cuaca, kualitas udara & geocoding: [Open-Meteo](https://open-meteo.com) (CC BY 4.0)
- Nama tempat dari koordinat: [BigDataCloud](https://www.bigdatacloud.com) reverse geocoding (client API gratis)
- Ikon cuaca: [Meteocons](https://github.com/basmilius/meteocons) oleh Bas Milius (MIT)
- Animasi transisi: [FluentTransitions](https://github.com/awaescher/FluentTransitions) (MIT)
- Driver PostgreSQL: [Npgsql](https://www.npgsql.org) (PostgreSQL License)

## 8. Untuk pengembang

```
dotnet run --project Swasana          # jalankan dari source
dotnet test Swasana.Tests             # unit test
dotnet publish Swasana -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -o publish/SWASANA
```
