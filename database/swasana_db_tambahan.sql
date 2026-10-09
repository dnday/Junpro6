-- Tambahan untuk swasana_db (jalankan SETELAH swasana_db_creation.sql dari Modul 5)
-- Isi: constraint yang kurang, threshold per parameter, dan seed data risk_levels + recommendations.

BEGIN;

-- 1. users: username tidak boleh dobel
ALTER TABLE users ADD CONSTRAINT uq_users_username UNIQUE (username);

-- 2. locations: rentang koordinat (sesuai alasan tipe data di Modul 4)
ALTER TABLE locations
    ADD CONSTRAINT ck_locations_latitude  CHECK (latitude  BETWEEN -90  AND 90),
    ADD CONSTRAINT ck_locations_longitude CHECK (longitude BETWEEN -180 AND 180);

-- 3. weather_data: rentang nilai cuaca
ALTER TABLE weather_data
    ADD CONSTRAINT ck_weather_humidity   CHECK (humidity   BETWEEN 0 AND 100),
    ADD CONSTRAINT ck_weather_wind_speed CHECK (wind_speed >= 0),
    ADD CONSTRAINT ck_weather_rainfall   CHECK (rainfall   >= 0);

-- 4. risk_levels: satu pasang threshold_min/max tidak cukup untuk aturan W1
--    (suhu ATAU hujan, dan hujan lebat DAN angin kencang), jadi diganti threshold per parameter.
ALTER TABLE risk_levels
    DROP COLUMN threshold_min,
    DROP COLUMN threshold_max,
    ADD COLUMN min_temperature DECIMAL(5,2),  -- °C, status aktif jika suhu > nilai ini
    ADD COLUMN min_rainfall    DECIMAL(6,2),  -- mm/jam
    ADD COLUMN min_wind_speed  DECIMAL(5,2),  -- km/jam
    ADD CONSTRAINT uq_risk_levels_name UNIQUE (level_name);

-- 5. ON DELETE: hapus user -> lokasi & riwayatnya ikut terhapus
ALTER TABLE locations DROP CONSTRAINT locations_user_id_fkey,
    ADD CONSTRAINT locations_user_id_fkey FOREIGN KEY (user_id)
        REFERENCES users(user_id) ON DELETE CASCADE;

ALTER TABLE weather_data DROP CONSTRAINT weather_data_location_id_fkey,
    ADD CONSTRAINT weather_data_location_id_fkey FOREIGN KEY (location_id)
        REFERENCES locations(location_id) ON DELETE CASCADE;

ALTER TABLE search_history DROP CONSTRAINT search_history_user_id_fkey,
    ADD CONSTRAINT search_history_user_id_fkey FOREIGN KEY (user_id)
        REFERENCES users(user_id) ON DELETE CASCADE;

ALTER TABLE search_history DROP CONSTRAINT search_history_weather_id_fkey,
    ADD CONSTRAINT search_history_weather_id_fkey FOREIGN KEY (weather_id)
        REFERENCES weather_data(weather_id) ON DELETE CASCADE;

-- 6. Index untuk kolom FK yang sering dipakai query riwayat
CREATE INDEX idx_locations_user        ON locations(user_id);
CREATE INDEX idx_weather_location_time ON weather_data(location_id, recorded_at);
CREATE INDEX idx_history_user_time     ON search_history(user_id, searched_at);

-- 7. Seed risk_levels (risk_id = nilai enum RiskLevel di classCode.cs)
--    Hujan: kategori BMKG per jam (sedang 5-10, lebat >10 mm/jam). Angin kencang: >=45 km/jam (~25 knot).
--    Bahaya = suhu > 35 ATAU (hujan >= 10 DAN angin >= 45). Waspada = suhu > 33 ATAU hujan >= 5.
INSERT INTO risk_levels (risk_id, level_name, color_code, min_temperature, min_rainfall, min_wind_speed) VALUES
    (1, 'Aman',    '#2E7D32', NULL,  NULL,  NULL),
    (2, 'Waspada', '#F9A825', 33.00, 5.00,  NULL),
    (3, 'Bahaya',  '#C62828', 35.00, 10.00, 45.00);
SELECT setval(pg_get_serial_sequence('risk_levels', 'risk_id'), 3);

-- 8. Seed recommendations (contoh; boleh ditambah)
INSERT INTO recommendations (risk_id, action_text, category) VALUES
    (1, 'Cuaca aman untuk beraktivitas di luar ruangan.', 'aktivitas'),
    (2, 'Bawa air minum dan payung saat keluar rumah.', 'kesehatan'),
    (2, 'Kurangi aktivitas fisik berat di luar ruangan.', 'aktivitas'),
    (3, 'Batasi aktivitas luar ruang pukul 11.00-14.00.', 'aktivitas'),
    (3, 'Tingkatkan konsumsi air minum untuk mencegah dehidrasi.', 'kesehatan'),
    (3, 'Gunakan pelindung diri (topi, tabir surya, jas hujan) dan hindari area rawan pohon tumbang.', 'perlindungan diri');

COMMIT;
