-- Skema swasana_db (Modul 5: Pembuatan Basis Data)
-- Urutan pakai:
--   1. CREATE DATABASE swasana_db;   (jalankan di database "postgres")
--   2. Buka Query Tool di swasana_db, jalankan file ini
--   3. Jalankan swasana_db_tambahan.sql

CREATE TABLE users (
    user_id SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE locations (
    location_id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL,
    city_name VARCHAR(100) NOT NULL,
    latitude DECIMAL(9,6) NOT NULL,
    longitude DECIMAL(9,6) NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users(user_id)
);

CREATE TABLE risk_levels (
    risk_id SMALLSERIAL PRIMARY KEY,
    level_name VARCHAR(20) NOT NULL,
    color_code CHAR(7) NOT NULL,
    threshold_min DECIMAL(5,2),
    threshold_max DECIMAL(5,2)
);

CREATE TABLE recommendations (
    rec_id SERIAL PRIMARY KEY,
    risk_id SMALLINT NOT NULL,
    action_text TEXT NOT NULL,
    category VARCHAR(30) NOT NULL,
    FOREIGN KEY (risk_id) REFERENCES risk_levels(risk_id)
);

CREATE TABLE weather_data (
    weather_id BIGSERIAL PRIMARY KEY,
    location_id INTEGER NOT NULL,
    risk_id SMALLINT NOT NULL,
    temperature DECIMAL(5,2),
    humidity DECIMAL(5,2),
    wind_speed DECIMAL(5,2),
    rainfall DECIMAL(6,2),
    recorded_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (location_id) REFERENCES locations(location_id),
    FOREIGN KEY (risk_id) REFERENCES risk_levels(risk_id)
);

CREATE TABLE search_history (
    history_id BIGSERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL,
    weather_id BIGINT NOT NULL,
    searched_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    note VARCHAR(255),
    FOREIGN KEY (user_id) REFERENCES users(user_id),
    FOREIGN KEY (weather_id) REFERENCES weather_data(weather_id)
);
