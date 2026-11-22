using FluentTransitions;

namespace Swasana
{
    // Animasi kecil memakai FluentTransitions (MIT, github.com/awaescher/FluentTransitions).
    // Semua dimatikan kalau "Animation effects" di Windows dimatikan (reduced motion).
    public static class Animasi
    {
        public static bool Aktif => SystemInformation.UIEffectsEnabled;

        // Form muncul perlahan saat pertama tampil
        public static void FadeIn(Form form)
        {
            if (!Aktif) return;
            form.Opacity = 0;
            form.Shown += (s, e) => Transition.With(form, nameof(Form.Opacity), 1.0).EaseInEaseOut(TimeSpan.FromMilliseconds(220));
        }

        // Warna latar tombol berubah halus saat kursor masuk/keluar
        public static void Hover(Button tombol, Color hover)
        {
            var normal = tombol.BackColor;
            tombol.MouseEnter += (s, e) => Ubah(tombol, nameof(Control.BackColor), hover, 150);
            tombol.MouseLeave += (s, e) => Ubah(tombol, nameof(Control.BackColor), normal, 200);
        }

        // Ubah satu properti (int, double, Color, string) dengan transisi; tanpa animasi kalau dimatikan
        public static void Ubah(object target, string properti, object nilai, int milidetik)
        {
            if (Aktif)
                Transition.With(target, properti, nilai).EaseInEaseOut(TimeSpan.FromMilliseconds(milidetik));
            else
                target.GetType().GetProperty(properti)!.SetValue(target, nilai);
        }
    }

    // Garis loading tipis yang bolak-balik selama data diambil (indeterminate progress)
    public class GarisLoading
    {
        private readonly Panel garis;
        private bool jalan;

        public GarisLoading(Control induk, Color warna)
        {
            garis = new Panel { Height = 4, Width = 140, BackColor = warna, Top = 0, Left = -140, Visible = false };
            induk.Controls.Add(garis);
            garis.BringToFront();
        }

        public void Mulai()
        {
            jalan = true;
            garis.Visible = true;
            if (!Animasi.Aktif) { garis.Left = 0; garis.Width = garis.Parent!.Width; return; }
            Putar();
        }

        public void Berhenti()
        {
            jalan = false;
            garis.Visible = false;
        }

        private void Putar()
        {
            if (!jalan) return;
            garis.Left = -garis.Width;
            Transition.With(garis, nameof(Control.Left), garis.Parent!.Width)
                .HookOnCompletionInUiThread(garis, Putar)
                .EaseInEaseOut(TimeSpan.FromMilliseconds(900));
        }
    }
}
