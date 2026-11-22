---
title: SWASANA
description: Sistem Waspada Suasana & Adaptasi Iklim
---

# SWASANA

**Sistem Waspada Suasana & Adaptasi Iklim**: aplikasi desktop Windows untuk mengecek cuaca terkini, menilai tingkat risikonya (**Aman / Waspada / Bahaya**), dan memberi saran tindakan yang bisa langsung dilakukan.

Junior Project (Teknik Basis Data, Pemrograman Berbasis Objek), Departemen Teknik Elektro dan Teknologi Informasi, Fakultas Teknik, Universitas Gadjah Mada.

**[Unduh versi terbaru](https://github.com/dnday/Junpro6/releases/latest)**: ekstrak `SWASANA.zip`, jalankan `Swasana.exe`, lalu masuk dengan username. Tidak perlu menginstal .NET maupun PostgreSQL.

![Cek cuaca](pengujian/T11-hasil-sleman.png)

## Daftar isi
- [Latar belakang](#latar-belakang)
- [Fitur](#fitur)
- [Tampilan](#tampilan)
- [Cara pakai](#cara-pakai)
- [Aturan risiko](#aturan-risiko)
- [Rancangan sistem](#rancangan-sistem)
- [Basis data](#basis-data)
- [Class diagram](#class-diagram)
- [Pengujian](#pengujian)
- [Teknologi](#teknologi)
- [Tim](#tim)
- [Kredit](#kredit)

## Latar belakang

Aplikasi cuaca pada umumnya hanya menampilkan angka mentah (suhu, kelembapan, curah hujan) tanpa menjelaskan seberapa berbahaya kondisinya. SWASANA mengubah data cuaca dari Open-Meteo menjadi status risiko yang mudah dipahami, lengkap dengan alasan dan saran tindakan, serta menyimpan riwayat pengecekan untuk melihat tren.

## Fitur

| Fitur | Keterangan |
|---|---|
| Cek cuaca terkini | Suhu, suhu yang terasa, kelembapan, angin, curah hujan, indeks UV, dan kualitas udara (AQI) |
| Pencarian lokasi | Saran lokasi muncul saat mengetik (misalnya "yog" menjadi DI Yogyakarta), lokasi Indonesia didahulukan |
| Lokasi saya | Memakai layanan lokasi Windows, lalu nama tempatnya dicari otomatis |
| Lokasi favorit | Simpan lokasi dengan tombol ☆, cek cuacanya sekali klik |
| Prakiraan | Per jam (8 jam) dan 7 hari ke depan |
| Status risiko | Aman / Waspada / Bahaya dari 6 aturan, ditampilkan beserta nilai yang dinilai |
| Saran tindakan | Kartu saran per kategori (aktivitas, kesehatan, perlindungan diri) |
| Riwayat | Semua pengecekan tersimpan, bisa dicari per lokasi dan disaring per status, dengan grafik tren suhu harian dan ekspor ke CSV |
| Database online | Langsung tersambung ke database Supabase; bisa diganti ke PostgreSQL sendiri |
| Tampilan | Ikon cuaca, latar langit sesuai cuaca, adegan animasi (hujan, panas, badai, angin, malam), kartu risiko berputar 3D |
| Aksesibilitas | Kontras teks memenuhi WCAG AA, status tidak hanya ditandai warna, animasi mati jika efek animasi Windows dimatikan |

## Tampilan

| Masuk | Menu |
|---|---|
| ![Masuk](pengujian/T05a-login.png) | ![Menu](pengujian/T05-menu-utama.png) |

**Saran lokasi saat mengetik**

![Saran lokasi](pengujian/T10-saran-lokasi.png)

**Lokasi favorit dan prakiraan 7 hari**

![Prakiraan 7 hari](pengujian/T21-prakiraan-7-hari.png)

**Status berbeda menyesuaikan kondisi**

![Ho Chi Minh](pengujian/T13-hasil-ho-chi-minh.png)

![Alice Springs](pengujian/T14-hasil-alice-springs.png)

**Riwayat dan grafik tren suhu harian**

![Riwayat](pengujian/T16-riwayat-dan-tren.png)

## Cara pakai

1. Unduh `SWASANA.zip` dari halaman [rilis](https://github.com/dnday/Junpro6/releases/latest) dan ekstrak ke folder mana saja.
2. Jalankan `Swasana.exe`. Jika muncul peringatan SmartScreen, pilih **More info → Run anyway**.
3. Ketik username lalu **Masuk**. Username baru otomatis didaftarkan.
4. Pilih **Cek cuaca**, ketik nama kota lalu pilih dari daftar saran, atau tekan **Lokasi saya**.
5. Tekan **☆** untuk menyimpan lokasi favorit, dan buka **Riwayat** untuk melihat tren.

Kebutuhan: Windows 10 (versi 2004+) / 11 64-bit dan koneksi internet. Panduan lengkap, termasuk memakai database sendiri, ada di [PANDUAN.md](https://github.com/dnday/Junpro6/blob/main/PANDUAN.md).

## Aturan risiko

Status akhir adalah tingkat tertinggi dari enam aturan. Setiap aturan adalah class turunan dari `RiskRule` (inheritance) yang mengisi method `Evaluate()` dengan caranya sendiri (polymorphism).

| Aturan | Waspada | Bahaya |
|---|---|---|
| Suhu udara | di atas 33 °C | di atas 35 °C |
| Curah hujan | mulai 5 mm/jam | – |
| Hujan dan angin | – | hujan mulai 10 mm/jam dan angin mulai 45 km/jam |
| Indeks UV (skala WHO) | mulai 8 | mulai 11 |
| Heat index (rumus NOAA, dari suhu dan kelembapan) | mulai 32 °C | mulai 41 °C |
| Kualitas udara (US AQI) | 101 sampai 150 | mulai 151 |

## Rancangan sistem

### Arsitektur

Tampilan (Windows Forms) memakai lapisan layanan untuk data cuaca dan penilaian risiko, dan lapisan akses data untuk PostgreSQL.

![Arsitektur](diagram/arsitektur.png)

### Use case

![Use case](diagram/use-case.png)

### Alur cek cuaca (activity diagram)

![Alur cek cuaca](diagram/alur-cek-cuaca.png)

## Basis data

Enam tabel di PostgreSQL. Database, tabel, dan data awal dibuat otomatis oleh aplikasi.

![ERD](diagram/erd.png)

| Relasi | Kardinalitas | Keterangan |
|---|---|---|
| users – locations | 1 : N | Satu pengguna memantau banyak lokasi, termasuk lokasi favorit |
| locations – weather_data | 1 : N | Setiap pengecekan menghasilkan satu data cuaca untuk lokasi itu |
| risk_levels – weather_data | 1 : N | Setiap data cuaca diklasifikasikan ke satu tingkat risiko |
| risk_levels – recommendations | 1 : N | Satu tingkat risiko memiliki beberapa saran tindakan |
| weather_data – search_history | 1 : N | Setiap data cuaca dicatat di riwayat |
| users – search_history | 1 : N | Riwayat terpisah untuk setiap pengguna |

## Class diagram

### Model dan akses data

`BaseRepository<T>` menulis fungsi ambil data sekali lalu diwariskan ke `UserRepository` dan `LocationRepository`; koneksi disimpan `private` di `DatabaseContext` (encapsulation).

![Class diagram model dan akses data](diagram/class-model-data.png)

### Layanan dan aturan risiko

`RiskEvaluator` menyimpan enam objek turunan `RiskRule` dalam satu daftar dan memanggil `Evaluate()` milik masing-masing (polymorphism).

![Class diagram layanan](diagram/class-layanan.png)

Sumber semua diagram (format Mermaid) ada di folder [`docs/diagram`](https://github.com/dnday/Junpro6/tree/main/docs/diagram) sehingga mudah diperbarui.

## Pengujian

- **Pengujian fungsional**: 22 skenario dijalankan otomatis pada aplikasi hasil rilis, mulai dari database kosong; semuanya lulus. Rincian di [PENGUJIAN.md](https://github.com/dnday/Junpro6/blob/main/PENGUJIAN.md).
- **Pengujian unit**: 61 test (aturan risiko, pembacaan data Open-Meteo, koneksi database, ekspor CSV, dan lainnya); semuanya lulus.

## Teknologi

| Bagian | Teknologi |
|---|---|
| Aplikasi | C#, .NET 10, Windows Forms |
| Basis data | PostgreSQL (lokal) atau Supabase (online), driver Npgsql |
| Data cuaca | Open-Meteo: Geocoding, Forecast, Air Quality |
| Lokasi | Layanan lokasi Windows, BigDataCloud (nama tempat) |
| Tampilan | Ikon Meteocons, animasi FluentTransitions |
| Pengujian | xUnit, UI Automation |

## Struktur repositori

| Folder / file | Isi |
|---|---|
| `Swasana/` | Aplikasi Windows Forms: halaman (`Form*.cs`), tema, ikon, dan animasi |
| `Swasana/Models/` | Class data: `User`, `Location`, `WeatherData`, `SearchHistory`, `RiskLevel`, prakiraan |
| `Swasana/Data/` | Akses database: `DatabaseContext`, `BaseRepository` dan turunannya, `DatabaseSetup` |
| `Swasana/Services/` | Layanan: data cuaca (Open-Meteo), aturan risiko, saran tindakan, riwayat, ekspor CSV |
| `Swasana.Tests/` | Unit test xUnit |
| `database/` | Skema SQL (ikut tertanam di aplikasi) dan skrip akun aplikasi Supabase |
| `docs/` | Halaman GitHub Pages, diagram, dan screenshot pengujian |
| `PANDUAN.md`, `PENGUJIAN.md` | Panduan pemakaian dan hasil pengujian |

Menjalankan dari kode sumber: `dotnet run --project Swasana`; menjalankan test: `dotnet test Swasana.Tests`.

## Tim

| Nama | NIM | Peran |
|---|---|---|
| Marcelinus Dinoglide Yoga Prakoso | 24/533842/TK/59152 | Software Architect & Team Lead |
| Raka Bagus Samudra | 24/543213/TK/60349 | Backend Developer |
| Muhammad Izzuddin Prakoso | 24/537712/TK/59618 | Frontend Developer |

## Kredit

- Data cuaca, kualitas udara, dan geocoding: [Open-Meteo](https://open-meteo.com) (CC BY 4.0)
- Nama tempat dari koordinat: [BigDataCloud](https://www.bigdatacloud.com)
- Ikon cuaca: [Meteocons](https://github.com/basmilius/meteocons) oleh Bas Milius (MIT)
- Animasi: [FluentTransitions](https://github.com/awaescher/FluentTransitions) (MIT)
- Driver PostgreSQL: [Npgsql](https://www.npgsql.org)
