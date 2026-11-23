# Pengujian SWASANA (MVP Modul 10)

Pengujian terakhir dilakukan pada 23/11/2026 pagi (sekitar pukul 10.30 WIB, Windows 11) dengan aturan risiko berbasis BMKG dan jurnal. T01 dan T02 dijalankan pada PostgreSQL 18 lokal; T03 sampai T22 memakai database online Supabase dengan akun baru `penguji_2311`. Pembuatan database dan tabel otomatis diuji pada pengujian sebelumnya (database lokal dikosongkan dulu). Karena memakai cuaca sungguhan, nilai suhu dan status di bawah mengikuti kondisi saat pengujian.
Langkah-langkahnya dijalankan otomatis lewat UI Automation. Bukti screenshot ada di folder [`docs/pengujian/`](docs/pengujian/).

## 1. Pengujian fungsional (black box)

| ID | Fitur | Skenario / input | Hasil yang diharapkan | Hasil aktual | Status |
|---|---|---|---|---|---|
| T01 | Koneksi database | Password PostgreSQL salah | Pesan di form menjelaskan password salah | *"Password PostgreSQL salah. Pakai password yang dibuat saat menginstal PostgreSQL."* | Lulus |
| T02 | Koneksi database | Port diisi `abc` | Pesan validasi di form | *"Host, database, dan username wajib diisi, dan port harus berupa angka (biasanya 5432)."* | Lulus |
| T03 | Koneksi database | Exe dengan database online bawaan dibuka | Langsung tersambung, jendela login terbuka | Jendela login terbuka | Lulus |
| T04 | Login | Username kosong | Pesan validasi di form | *"Isi username dulu untuk masuk."* | Lulus |
| T05 | Login | Username baru `penguji_2311`, pilih **Yes** pada konfirmasi | Konfirmasi akun baru muncul; user didaftarkan, menu utama terbuka | *"Halo, penguji_2311"* | Lulus |
| T06 | Menu | Klik **Tentang SWASANA** | Info aplikasi tampil | MessageBox Tentang | Lulus |
| T07 | Navigasi | Buka **Cek cuaca** | Cek cuaca terbuka sebagai jendela dialog di atas menu | Sesuai | Lulus |
| T08 | Cek cuaca | Lokasi kosong | Pesan validasi di form | *"Isi nama lokasi dulu, misalnya Sleman."* | Lulus |
| T09 | Cek cuaca | Lokasi `Xyzqwerty` | Pesan lokasi tidak ditemukan | *"Lokasi "Xyzqwerty" tidak ditemukan. ..."* | Lulus |
| T10 | Saran lokasi (Web API) | Ketik `Slem`, lalu tekan Enter | Daftar saran muncul; memilih saran mengisi nama dan koordinat lalu langsung mengecek cuaca | 8 saran tampil; terpilih *Sleman, Yogyakarta* (-7.7156, 110.3556) | Lulus |
| T11 | Cuaca + risiko (Web API) | Hasil cek Sleman | Suhu, kondisi, detail, prakiraan per jam, status, chip 6 aturan, rincian aturan pemicu beserta nilainya, dan rekomendasi tampil, lalu tersimpan | 28,6°, Gerimis, 79 %, 7,2 km/j, 0,1 mm/j → **Waspada** (indeks UV 8,3 dan heat index 33,5 °C); tersimpan | Lulus |
| T20 | Lokasi favorit | Klik ☆ pada Sleman | Lokasi tersimpan sebagai favorit, muncul tombol cepat, bintang menjadi ★ | *"Sleman, Yogyakarta disimpan ke favorit."*, tombol ★ Sleman muncul | Lulus |
| T21 | Prakiraan 7 hari | Klik **7 hari** | Tampil 7 kolom hari: ikon, suhu maks/min, peluang hujan | 7 kolom tampil | Lulus |
| T12 | Validasi koordinat | Latitude `100` | Pesan validasi di form | *"Latitude harus -90 s.d. 90 dan longitude -180 s.d. 180."* | Lulus |
| T13 | Status sesuai aturan | Lokasi `Ho Chi Minh` | Status dihitung dari 6 aturan, kartu rekomendasi sesuai status | 31,8°, Mendung, **Waspada**; kartu saran Waspada | Lulus |
| T14 | Koordinat manual | Alice Springs, lat `-23,698` lon `133,8807` (pakai koma) | Koma diterima; status dihitung dari 6 aturan | 36,1°, **Waspada** (indeks UV 9,5 dan heat index 34,3 °C); tidak Bahaya karena suhu udara saja tidak lagi dinilai | Lulus |
| T22 | Lokasi favorit | Klik tombol **★ Sleman** | Cuaca Sleman langsung dicek sekali klik | Sleman, Yogyakarta, **Waspada** | Lulus |
| T15 | Navigasi | Klik **Kembali** | Jendela Cek cuaca tertutup, menu aktif lagi | Sesuai | Lulus |
| T16 | Riwayat + grafik tren | Buka **Riwayat** | Semua pengecekan user tampil, terbaru di atas; grafik suhu maks/min harian dari `weather_data` | 4 data; grafik tampil | Lulus |
| T17 | Riwayat | Cari `hO cHi` | Hanya Ho Chi Minh tampil (tidak peka huruf besar/kecil) | 1 data | Lulus |
| T18 | Riwayat | Cari `Bandung` | Pesan riwayat kosong | 0 data, pesan *"Tidak ada riwayat untuk "Bandung"..."* | Lulus |
| T19 | Keluar | Klik **Keluar** | Aplikasi tertutup | Proses berhenti | Lulus |

Ketiga status (Aman, Waspada, Bahaya) beserta batas setiap aturan juga diuji lewat unit test, karena status dari cuaca sungguhan bergantung pada kondisi saat pengujian. Contohnya hujan mulai 20 mm/jam, angin di atas 45 km/jam saat hujan lebat, heat index mulai 41 °C, AQI mulai 151, dan peringatan BMKG berkeparahan *Severe* menghasilkan **Bahaya**.

Diuji manual (di luar skrip otomatis):

| Fitur | Hasil |
|---|---|
| **Peringatan dini BMKG** | Rajabasa, Lampung (-4,2943; 105,2519) pada 23/11/2026 pukul 10.00 WIB: peringatan *Hujan Lebat dan Petir* (berlaku s.d. 11.30) terdeteksi, aturan BMKG **Waspada**, chip BMKG menyala ([screenshot](docs/pengujian/T23-peringatan-bmkg.png)) |
| **Lokasi saya** (layanan lokasi Windows) | Menemukan *Melati, Yogyakarta* (-7.7593, 110.3712), lalu langsung menampilkan cuacanya |
| Adegan animasi per cuaca | Hujan: orang berpayung + rintik. Panas: topi lebar + botol minum + gelombang panas. Sangat panas: berteduh di bawah payung |
| Animasi saat hasil keluar | Garis loading, suhu berjalan ke nilai baru, warna langit berganti halus, kartu risiko berputar 3D, kartu rekomendasi masuk bergantian |

## 2. Pengujian unit (xUnit)

Perintah: `dotnet test Swasana.Tests` → **72 lulus, 0 gagal**.

| Test | Yang diuji |
|---|---|
| `UvRule_SkalaWho`, `HeatIndex_SesuaiTabelNoaa`, `HeatIndexRule_Klasifikasi`, `KualitasUdaraRule_UsAqi` | Aturan baru: UV 8/11, heat index NOAA (32 °C Waspada, 41 °C Bahaya), US AQI 101/151; AQI tidak tersedia tidak menaikkan risiko |
| `RiskEvaluator_MemakaiEnamAturan` | Evaluator menjalankan 6 aturan; risiko naik hanya karena kualitas udara |
| `ParseAirQuality_*`, `ParseCurrent_MembacaPrakiraanHarian` | Data AQI/PM2.5 dan prakiraan 7 hari dibaca benar, nilai null dari API ditangani |
| `EvaluateRisk_HujanDanAnginSesuaiBmkg` (7 kasus) | Batas kategori hujan BMKG: Aman (kering; 9,9 mm/jam), Waspada (10 dan 19,9 mm/jam; hujan 15 mm/jam dengan angin tepat 45 km/jam), Bahaya (tepat 20 mm/jam; hujan 15 mm/jam + angin 46 km/jam) |
| `Rincian_BerisiSemuaAturan` | `RiskEvaluator.rincian()` mengembalikan hasil 6 aturan; 34 °C dengan kelembapan 75 % membuat heat index Bahaya |
| `ParseCurrent_MembacaJsonOpenMeteo` | JSON cuaca dibaca menjadi `WeatherData`, termasuk kode cuaca dan siang/malam |
| `ParseGeocode_*`, `ParseSaran_*` | Hasil geocoding dan saran lokasi; lokasi Indonesia didahulukan; tanpa hasil menghasilkan null/kosong |
| `NamaCuaca_SesuaiKodeWmo` (7 kasus) | Kode cuaca WMO dipetakan ke ikon yang benar (cerah siang/malam, hujan, hujan lebat, badai) |
| `SemuaIkonTertanamDiAplikasi` | Ikon Meteocons tertanam di dalam exe |
| `SetLocation_TolakKoordinatDiLuarRentang` | `Location.setLocation()` menolak lat > 90 dan lon > 180 |
| `KategoriAqi_BatasUsAqi`, `KategoriUv_SkalaWho` | Batas kategori kualitas udara (50/100/150/200) dan indeks UV (3/6/8/11) |
| `NilaiAturan_MenampilkanNilaiYangDinilai`, `WarnaLangit_MalamTidakDipengaruhiKodeCuaca` | Nilai yang dinilai tiap aturan ditampilkan; langit malam sama untuk semua kode cuaca |
| `BuatCsv_*` | Ekspor riwayat: header dan satu baris per data; nama lokasi dengan koma atau tanda kutip diberi kutip, awalan `=` diamankan dari rumus Excel |
| `BacaUri_*` | URI koneksi Supabase dipecah ke host, port, database, dan username; password placeholder `[YOUR-...]` diabaikan, password dengan karakter khusus didekode, dan teks yang bukan URI menghasilkan null |
| `BacaCap_MembacaInfoDanPolygon`, `DiDalam_RayCasting` (3 kasus) | Dokumen CAP peringatan dini BMKG terbaca (kejadian, keparahan, waktu berakhir, polygon); titik di dalam/luar polygon wilayah peringatan |
| `Pilih_HanyaPeringatanYangMasihBerlaku`, `CariAsync_LuarIndonesiaTanpaInternet` | Peringatan kedaluwarsa atau di luar wilayah diabaikan; lokasi di luar Indonesia tidak mengakses BMKG |
| `PeringatanBmkgRule_KeparahanCap` (5 kasus) | Tanpa peringatan → Aman; Minor/Moderate → Waspada; Severe/Extreme → Bahaya |
| `PilihSsl_OnlineWajibTerenkripsi` | Host lokal (`localhost`, `127.0.0.1`) memakai SSL `Prefer`; server online memakai `Require` |

## 3. Pengujian instalasi dan aksesibilitas

| Aspek | Hasil |
|---|---|
| Jalankan `Swasana.exe` tanpa pengaturan sebelumnya | Exe tanpa database bawaan langsung membuka **Koneksi database**; exe yang membawa database online bawaan langsung membuka halaman **Masuk** |
| .NET tidak perlu diinstal | Ya, runtime sudah ikut di dalam exe (self-contained, ±59 MB) |
| Database tidak perlu dibuat manual | Ya, dibuat otomatis (T03) |
| Kontras teks (WCAG AA ≥ 4,5:1) | Dihitung dari nilai warna: teks putih di langit cerah 5,0:1, mendung 5,8:1, hujan 7,1:1, malam 10,5:1; status Aman 5,1:1, Waspada 6,7:1, Bahaya 5,6:1 |
| Reduced motion | Semua animasi berhenti kalau *Animation effects* Windows dimatikan |
