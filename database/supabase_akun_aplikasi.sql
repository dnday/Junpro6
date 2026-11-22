-- Akun database khusus aplikasi SWASANA untuk Supabase.
-- Jalankan SEKALI di Supabase > SQL Editor, setelah tabel dibuat (aplikasi sudah pernah tersambung sebagai postgres).
-- Akun ini yang ditanam di Swasana.exe, jadi hak aksesnya dibatasi:
--   boleh   : SELECT, INSERT, UPDATE pada tabel aplikasi
--   tidak   : DELETE, mengubah/menghapus tabel, mengakses schema lain
-- Ganti GANTI_DENGAN_PASSWORD dengan password yang sama seperti di Swasana/koneksi-bawaan.txt.

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'swasana_app') THEN
        CREATE ROLE swasana_app LOGIN PASSWORD 'GANTI_DENGAN_PASSWORD';
    ELSE
        ALTER ROLE swasana_app LOGIN PASSWORD 'GANTI_DENGAN_PASSWORD';
    END IF;
END $$;

GRANT USAGE ON SCHEMA public TO swasana_app;
GRANT SELECT, INSERT, UPDATE ON users, locations, weather_data, search_history TO swasana_app;
GRANT SELECT ON risk_levels, recommendations TO swasana_app;
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO swasana_app;

-- Semua tabel memakai Row Level Security; akun ini butuh policy supaya boleh mengakses.
-- (Akun anon/authenticated dari API publik Supabase tetap tidak punya policy, jadi tetap tertutup.)
DO $$
DECLARE t TEXT;
BEGIN
    FOREACH t IN ARRAY ARRAY['users', 'locations', 'weather_data', 'search_history', 'risk_levels', 'recommendations']
    LOOP
        EXECUTE format('DROP POLICY IF EXISTS swasana_app_akses ON %I', t);
        EXECUTE format('CREATE POLICY swasana_app_akses ON %I TO swasana_app USING (true) WITH CHECK (true)', t);
    END LOOP;
END $$;
