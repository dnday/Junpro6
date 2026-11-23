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
3. Tempel URI tersebut ke kolom **Host** di jendela Koneksi database. Host, port, database, dan username terisi otomatis. Connection string bergaya .NET juga bisa ditempel, misalnya `Host=...;Port=5432;Database=postgres;Username=...;Password=...;SSL Mode=Require`; kalau berisi password, password ikut terisi.
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

Status akhir = tingkat tertinggi dari enam aturan berikut. Ambangnya diambil dari peraturan BMKG, pedoman resmi, dan jurnal. Kartu status menampilkan satu chip per aturan (✓ berarti aman) dan rincian aturan yang memicu.

| Aturan | Waspada | Bahaya | Rujukan |
|---|---|---|---|
| Curah hujan | hujan lebat, 10 sampai di bawah 20 mm/jam | hujan sangat lebat, mulai 20 mm/jam | [1], [2] |
| Hujan dan angin | – | hujan mulai 10 mm/jam dan angin di atas 45 km/jam (25 knot) | [1] |
| Indeks UV | sangat tinggi, 8–10 | ekstrem, mulai 11 | [3], [4] |
| Heat index (dari suhu dan kelembapan) | *extreme caution*, 32–41 °C | *danger*, mulai 41 °C | [5], [6] |
| Kualitas udara (US AQI) | tidak sehat bagi kelompok sensitif, 101–150 | tidak sehat, mulai 151 | [7], [8] |
| Peringatan dini BMKG (khusus Indonesia) | ada peringatan aktif di lokasi | peringatan berkeparahan *Severe* atau *Extreme* | [9], [10] |

Aturan suhu udara saja (di atas 33/35 °C) dihapus karena ambang itu tidak ada di peraturan BMKG maupun jurnal; BMKG mendefinisikan suhu ekstrem sebagai penyimpangan dari normal setempat. Penilaian panas memakai heat index, yang menggabungkan suhu dan kelembapan. Peringatan dini BMKG diambil dari feed resmi BMKG (format CAP): aplikasi mengecek apakah koordinat lokasi berada di dalam polygon wilayah peringatan yang masih berlaku.

**Rujukan**

1. Badan Meteorologi, Klimatologi, dan Geofisika. (2010). *Peraturan Kepala BMKG Nomor Kep.009 Tahun 2010 tentang Prosedur Standar Operasional Pelaksanaan Peringatan Dini, Pelaporan, dan Diseminasi Informasi Cuaca Ekstrem* (Berita Negara RI 2010 No. 497). Kriteria cuaca ekstrem: hujan lebat mulai 20 mm/jam, angin kencang di atas 25 knot (45 km/jam). Kini digantikan Peraturan BMKG Nomor 9 Tahun 2022.
2. Lestari, T. A., dkk. Analisis hujan sangat lebat yang menyebabkan genangan di sebagian wilayah Jakarta menggunakan citra radar (studi kasus 18 Januari 2022). *Universitas Negeri Jakarta*. https://journal.unj.ac.id/unj/index.php/castle/article/view/67087 (kategori intensitas hujan BMKG; lebih dari 20 mm/jam tergolong sangat lebat).
3. World Health Organization. (2002). *Global Solar UV Index: A Practical Guide*. WHO, WMO, UNEP, ICNIRP.
4. Fioletov, V., Kerr, J. B., & Fergusson, A. (2010). The UV Index: definition, distribution and factors affecting it. *Canadian Journal of Public Health, 101*(4), I5–I9. https://doi.org/10.1007/BF03405303
5. Rothfusz, L. P. (1990). *The Heat Index "Equation"* (NWS Technical Attachment SR 90-23). National Weather Service.
6. Anderson, G. B., Bell, M. L., & Peng, R. D. (2013). Methods to calculate the heat index as an exposure metric in environmental health research. *Environmental Health Perspectives, 121*(10), 1111–1119. https://doi.org/10.1289/ehp.1206273
7. U.S. Environmental Protection Agency. (2018). *Technical Assistance Document for the Reporting of Daily Air Quality – the Air Quality Index (AQI)* (EPA-454/B-18-007).
8. Plaia, A., & Ruggieri, M. (2011). Air quality indices: a review. *Reviews in Environmental Science and Bio/Technology, 10*(2), 165–179. https://doi.org/10.1007/s11157-010-9227-2
9. Badan Meteorologi, Klimatologi, dan Geofisika. (2022). *Peraturan BMKG Nomor 9 Tahun 2022 tentang Peringatan Dini Cuaca Ekstrem*. Data: https://www.bmkg.go.id/alerts/nowcast/id
10. OASIS. (2010). *Common Alerting Protocol Version 1.2*. OASIS Standard. http://docs.oasis-open.org/emergency/cap/v1.2/CAP-v1.2-os.html

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
