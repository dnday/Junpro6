-- Skema final swasana_db (Modul 10 / MVP)
-- Gabungan swasana_db_creation.sql (Modul 5) + swasana_db_tambahan.sql.
-- Aman dijalankan berulang kali: tabel/seed yang sudah ada tidak diubah.
-- Aplikasi menjalankan file ini otomatis lewat menu "Pengaturan Database".

CREATE TABLE IF NOT EXISTS users (
    user_id SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS locations (
    location_id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(user_id) ON DELETE CASCADE,
    city_name VARCHAR(100) NOT NULL,
    latitude DECIMAL(9,6) NOT NULL CHECK (latitude BETWEEN -90 AND 90),
    longitude DECIMAL(9,6) NOT NULL CHECK (longitude BETWEEN -180 AND 180)
);

-- Lokasi favorit pengguna (ditambahkan untuk fitur favorit; aman untuk database lama)
ALTER TABLE locations ADD COLUMN IF NOT EXISTS is_favorite BOOLEAN NOT NULL DEFAULT FALSE;

CREATE TABLE IF NOT EXISTS risk_levels (
    risk_id SMALLSERIAL PRIMARY KEY,
    level_name VARCHAR(20) NOT NULL UNIQUE,
    color_code CHAR(7) NOT NULL,
    min_temperature DECIMAL(5,2),
    min_rainfall DECIMAL(6,2),
    min_wind_speed DECIMAL(5,2)
);

CREATE TABLE IF NOT EXISTS recommendations (
    rec_id SERIAL PRIMARY KEY,
    risk_id SMALLINT NOT NULL REFERENCES risk_levels(risk_id),
    action_text TEXT NOT NULL,
    category VARCHAR(30) NOT NULL
);

CREATE TABLE IF NOT EXISTS weather_data (
    weather_id BIGSERIAL PRIMARY KEY,
    location_id INTEGER NOT NULL REFERENCES locations(location_id) ON DELETE CASCADE,
    risk_id SMALLINT NOT NULL REFERENCES risk_levels(risk_id),
    temperature DECIMAL(5,2),
    humidity DECIMAL(5,2) CHECK (humidity BETWEEN 0 AND 100),
    wind_speed DECIMAL(5,2) CHECK (wind_speed >= 0),
    rainfall DECIMAL(6,2) CHECK (rainfall >= 0),
    recorded_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS search_history (
    history_id BIGSERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(user_id) ON DELETE CASCADE,
    weather_id BIGINT NOT NULL REFERENCES weather_data(weather_id) ON DELETE CASCADE,
    searched_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    note VARCHAR(255)
);

CREATE INDEX IF NOT EXISTS idx_locations_user        ON locations(user_id);
CREATE INDEX IF NOT EXISTS idx_weather_location_time ON weather_data(location_id, recorded_at);
CREATE INDEX IF NOT EXISTS idx_history_user_time     ON search_history(user_id, searched_at);

-- Seed risk_levels (risk_id = nilai enum RiskLevel di Models/RiskLevel.cs)
INSERT INTO risk_levels (risk_id, level_name, color_code) VALUES
    (1, 'Aman',    '#2E7D32'),
    (2, 'Waspada', '#F9A825'),
    (3, 'Bahaya',  '#C62828')
ON CONFLICT DO NOTHING;
SELECT setval(pg_get_serial_sequence('risk_levels', 'risk_id'), 3);

-- Seed recommendations, hanya kalau tabel masih kosong
INSERT INTO recommendations (risk_id, action_text, category)
SELECT * FROM (VALUES
    (1::SMALLINT, 'Cuaca aman untuk beraktivitas di luar ruangan.', 'aktivitas'),
    (2::SMALLINT, 'Bawa air minum dan payung saat keluar rumah.', 'kesehatan'),
    (2::SMALLINT, 'Kurangi aktivitas fisik berat di luar ruangan.', 'aktivitas'),
    (3::SMALLINT, 'Batasi aktivitas luar ruang pukul 11.00-14.00.', 'aktivitas'),
    (3::SMALLINT, 'Tingkatkan konsumsi air minum untuk mencegah dehidrasi.', 'kesehatan'),
    (3::SMALLINT, 'Gunakan pelindung diri (topi, tabir surya, jas hujan) dan hindari area rawan pohon tumbang.', 'perlindungan diri')
) AS seed(risk_id, action_text, category)
WHERE NOT EXISTS (SELECT 1 FROM recommendations);
