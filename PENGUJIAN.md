# Pengujian SWASANA (MVP Modul 10)

Pengujian dilakukan pada 22/11/2026 terhadap `Swasana.exe` hasil publish (Windows 11, PostgreSQL 18). Database `swasana_db` dihapus dulu sebelum pengujian, supaya pembuatan database otomatis ikut teruji.
Langkah-langkahnya dijalankan otomatis lewat UI Automation. Bukti screenshot ada di folder [`docs/pengujian/`](docs/pengujian/).

## 1. Pengujian fungsional (black box)

| ID | Fitur | Skenario / input | Hasil yang diharapkan | Hasil aktual | Status |
|---|---|---|---|---|---|
| T01 | Koneksi database | Password PostgreSQL salah | Pesan di form menjelaskan password salah | *"Password PostgreSQL salah. Pakai password yang dibuat saat menginstal PostgreSQL."* | Lulus |
| T02 | Koneksi database | Port diisi `abc` | Pesan validasi di form | *"Host, database, dan username wajib diisi, dan port harus berupa angka (biasanya 5432)."* | Lulus |
| T03 | Koneksi database | Data benar, database belum ada | Database, 6 tabel, dan data awal dibuat, lalu jendela login terbuka | Jendela login terbuka; 6 tabel dan 6 rekomendasi ada di database | Lulus |
| T04 | Login | Username kosong | Pesan validasi di form | *"Isi username dulu untuk masuk."* | Lulus |
| T05 | Login | Username baru `marcel` | User didaftarkan, menu utama terbuka | *"Halo, marcel"* | Lulus |
| T06 | Menu | Klik **Tentang SWASANA** | Info aplikasi tampil | MessageBox Tentang | Lulus |
| T07 | Navigasi | Buka **Cek cuaca** | Menu disembunyikan selama form terbuka | Menu tersembunyi | Lulus |
| T08 | Cek cuaca | Lokasi kosong | Pesan validasi di form | *"Isi nama lokasi dulu, misalnya Sleman."* | Lulus |
| T09 | Cek cuaca | Lokasi `Xyzqwerty` | Pesan lokasi tidak ditemukan | *"Lokasi "Xyzqwerty" tidak ditemukan. ..."* | Lulus |
| T10 | Saran lokasi (Web API) | Ketik `Slem`, lalu tekan Enter | Daftar saran muncul; memilih saran mengisi nama dan koordinat lalu langsung mengecek cuaca | 8 saran tampil; terpilih *Sleman, Yogyakarta* (-7.7156, 110.3556) | Lulus |
| T11 | Cuaca + risiko (Web API) | Hasil cek Sleman | Suhu, kondisi, detail, prakiraan per jam, status, rincian 6 aturan beserta nilainya, dan rekomendasi tampil, lalu tersimpan | 25,5°, Hujan sedang, 90 %, AQI 125 → **Waspada** (dari aturan kualitas udara); tersimpan | Lulus |
| T20 | Lokasi favorit | Klik ☆ pada Sleman | Lokasi tersimpan sebagai favorit, muncul tombol cepat, bintang menjadi ★ | *"Sleman, Yogyakarta disimpan ke favorit."*, tombol ★ Sleman muncul | Lulus |
| T21 | Prakiraan 7 hari | Klik **7 hari** | Tampil 7 kolom hari: ikon, suhu maks/min, peluang hujan | 7 kolom tampil | Lulus |
| T12 | Validasi koordinat | Latitude `100` | Pesan validasi di form | *"Latitude harus -90 s.d. 90 dan longitude -180 s.d. 180."* | Lulus |
| T13 | Status Waspada | Lokasi `Ho Chi Minh` | Status **Waspada**, kartu rekomendasi Waspada | 34,3°, **Waspada** | Lulus |
| T14 | Status Bahaya + koordinat manual | Alice Springs, lat `-23,698` lon `133,8807` (pakai koma) | Koma diterima; status **Bahaya** | 37°, **Bahaya** (suhu), heat index 35,2 °C Waspada, 3 kartu rekomendasi | Lulus |
| T22 | Lokasi favorit | Klik tombol **★ Sleman** | Cuaca Sleman langsung dicek sekali klik | Sleman, Yogyakarta, **Waspada** | Lulus |
| T15 | Navigasi | Klik **Kembali** | Menu tampil lagi | Menu tampil | Lulus |
| T16 | Riwayat + grafik tren | Buka **Riwayat** | Semua pengecekan user tampil, terbaru di atas; grafik suhu maks/min harian dari `weather_data` | 4 data; grafik tampil (1 hari data, dengan petunjuk bahwa tren butuh ≥ 2 hari) | Lulus |
| T17 | Riwayat | Cari `hO cHi` | Hanya Ho Chi Minh tampil (tidak peka huruf besar/kecil) | 1 data | Lulus |
| T18 | Riwayat | Cari `Bandung` | Pesan riwayat kosong | 0 data, pesan *"Tidak ada riwayat untuk "Bandung"..."* | Lulus |
| T19 | Keluar | Klik **Keluar** | Aplikasi tertutup | Proses berhenti | Lulus |

Diuji manual (di luar skrip otomatis):

| Fitur | Hasil |
|---|---|
| **Lokasi saya** (layanan lokasi Windows) | Menemukan *Melati, Yogyakarta* (-7.7593, 110.3712), lalu langsung menampilkan cuacanya |
| Adegan animasi per cuaca | Hujan: orang berpayung + rintik. Panas: topi lebar + botol minum + gelombang panas. Sangat panas: berteduh di bawah payung |
| Animasi saat hasil keluar | Garis loading, suhu berjalan ke nilai baru, warna langit berganti halus, kartu risiko berputar 3D, kartu rekomendasi masuk bergantian |

## 2. Pengujian unit (xUnit)

Perintah: `dotnet test Swasana.Tests` → **37 lulus, 0 gagal**.

| Test | Yang diuji |
|---|---|
| `UvRule_SkalaWho`, `HeatIndex_SesuaiTabelNoaa`, `HeatIndexRule_Klasifikasi`, `KualitasUdaraRule_UsAqi` | Aturan baru: UV 8/11, heat index NOAA (32 °C Waspada, 41 °C Bahaya), US AQI 101/151; AQI tidak tersedia tidak menaikkan risiko |
| `RiskEvaluator_MemakaiEnamAturan` | Evaluator menjalankan 6 aturan; risiko naik hanya karena kualitas udara |
| `ParseAirQuality_*`, `ParseCurrent_MembacaPrakiraanHarian` | Data AQI/PM2.5 dan prakiraan 7 hari dibaca benar, nilai null dari API ditangani |
| `EvaluateRisk_MengikutiAturanModul1` (7 kasus) | Aman (30 °C; tepat 33 °C), Waspada (34 °C; hujan 5 mm/jam; hujan 12 mm/jam tanpa angin kencang), Bahaya (36 °C; hujan 15 mm/jam + angin 50 km/jam) |
| `Rincian_BerisiSemuaAturan` | `RiskEvaluator.rincian()` mengembalikan hasil 6 aturan |
| `ParseCurrent_MembacaJsonOpenMeteo` | JSON cuaca dibaca menjadi `WeatherData`, termasuk kode cuaca dan siang/malam |
| `ParseGeocode_*`, `ParseSaran_*` | Hasil geocoding dan saran lokasi; lokasi Indonesia didahulukan; tanpa hasil menghasilkan null/kosong |
| `NamaCuaca_SesuaiKodeWmo` (7 kasus) | Kode cuaca WMO dipetakan ke ikon yang benar (cerah siang/malam, hujan, hujan lebat, badai) |
| `SemuaIkonTertanamDiAplikasi` | Ikon Meteocons tertanam di dalam exe |
| `SetLocation_TolakKoordinatDiLuarRentang` | `Location.setLocation()` menolak lat > 90 dan lon > 180 |

## 3. Pengujian instalasi dan aksesibilitas

| Aspek | Hasil |
|---|---|
| Jalankan `Swasana.exe` tanpa pengaturan sebelumnya | Langsung terbuka **Koneksi database** |
| .NET tidak perlu diinstal | Ya, runtime sudah ikut di dalam exe (self-contained, ±59 MB) |
| Database tidak perlu dibuat manual | Ya, dibuat otomatis (T03) |
| Kontras teks (WCAG AA ≥ 4,5:1) | Dihitung dari nilai warna: teks putih di langit cerah 5,0:1, mendung 5,8:1, hujan 7,1:1, malam 10,5:1; status Aman 5,1:1, Waspada 6,7:1, Bahaya 5,6:1 |
| Reduced motion | Semua animasi berhenti kalau *Animation effects* Windows dimatikan |
