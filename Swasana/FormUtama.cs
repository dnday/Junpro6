namespace Swasana
{
    public partial class FormUtama : Form
    {
        public FormUtama()
        {
            InitializeComponent();
        }

        private void btnCekCuaca_Click(object sender, EventArgs e)
        {
            new FormCekCuaca().ShowDialog(this);
        }

        private void btnRiwayat_Click(object sender, EventArgs e)
        {
            new FormRiwayat().ShowDialog(this);
        }

        private void btnTentang_Click(object sender, EventArgs e)
        {
            MessageBox.Show("SWASANA - Sistem Waspada Suasana & Adaptasi Iklim\n" +
                "Memantau cuaca dan memberi status risiko (Aman, Waspada, Bahaya) beserta saran tindakan.",
                "Tentang");
        }

        private void btnKeluar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
