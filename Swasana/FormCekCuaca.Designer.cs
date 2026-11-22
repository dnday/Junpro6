namespace Swasana
{
    partial class FormCekCuaca
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblJudul = new Label();
            lblLokasi = new Label();
            tbLokasi = new TextBox();
            btnFavorit = new Button();
            btnLokasiSaya = new Button();
            lblLatitude = new Label();
            tbLatitude = new TextBox();
            lblLongitude = new Label();
            tbLongitude = new TextBox();
            btnCek = new Button();
            lstSaran = new ListBox();
            flowFavorit = new FlowLayoutPanel();
            lblPesan = new Label();
            pnlHero = new Panel();
            lblLokasiHasil = new Label();
            lblSuhu = new Label();
            lblKondisi = new Label();
            lblTerasa = new Label();
            picCuaca = new PictureBox();
            pnlJam = new Panel();
            lblJamJudul = new Label();
            btnPerJam = new Button();
            btnHarian = new Button();
            flowJam = new FlowLayoutPanel();
            pnlKelembapan = new Panel();
            picKelembapan = new PictureBox();
            lblKelembapanJudul = new Label();
            lblKelembapan = new Label();
            pnlAngin = new Panel();
            picAngin = new PictureBox();
            lblAnginJudul = new Label();
            lblAngin = new Label();
            pnlHujan = new Panel();
            picHujan = new PictureBox();
            lblHujanJudul = new Label();
            lblHujan = new Label();
            pnlUv = new Panel();
            picUv = new PictureBox();
            lblUvJudul = new Label();
            lblUv = new Label();
            pnlUdara = new Panel();
            picUdara = new PictureBox();
            lblUdaraJudul = new Label();
            lblUdara = new Label();
            pnlRisiko = new Panel();
            picStatus = new PictureBox();
            lblRisikoJudul = new Label();
            lblStatus = new Label();
            lblRincian = new Label();
            lblRekomendasiJudul = new Label();
            flowRekomendasi = new FlowLayoutPanel();
            lblSumber = new Label();
            btnKembali = new Button();
            pnlHeader.SuspendLayout();
            flowFavorit.SuspendLayout();
            pnlHero.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picCuaca).BeginInit();
            pnlJam.SuspendLayout();
            flowJam.SuspendLayout();
            pnlKelembapan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picKelembapan).BeginInit();
            pnlAngin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picAngin).BeginInit();
            pnlHujan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picHujan).BeginInit();
            pnlUv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picUv).BeginInit();
            pnlUdara.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picUdara).BeginInit();
            pnlRisiko.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picStatus).BeginInit();
            flowRekomendasi.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(lblJudul);
            pnlHeader.BackColor = Color.FromArgb(15, 76, 92);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Size = new Size(880, 60);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.TabIndex = 0;
            // 
            // lblJudul
            // 
            lblJudul.Font = new Font("Bahnschrift SemiBold", 18F);
            lblJudul.ForeColor = Color.White;
            lblJudul.Location = new Point(22, 15);
            lblJudul.Text = "Cek cuaca";
            lblJudul.AutoSize = true;
            lblJudul.Name = "lblJudul";
            lblJudul.TabIndex = 1;
            // 
            // lblLokasi
            // 
            lblLokasi.Font = new Font("Segoe UI Semibold", 9.75F);
            lblLokasi.ForeColor = Color.FromArgb(28, 43, 54);
            lblLokasi.Location = new Point(24, 72);
            lblLokasi.Text = "Lokasi";
            lblLokasi.AutoSize = true;
            lblLokasi.Name = "lblLokasi";
            lblLokasi.TabIndex = 2;
            // 
            // tbLokasi
            // 
            tbLokasi.BorderStyle = BorderStyle.FixedSingle;
            tbLokasi.Font = new Font("Segoe UI", 11F);
            tbLokasi.Location = new Point(24, 93);
            tbLokasi.Size = new Size(262, 27);
            tbLokasi.PlaceholderText = "Ketik nama kota, misalnya Yogyakarta";
            tbLokasi.TextChanged += tbLokasi_TextChanged;
            tbLokasi.KeyDown += tbLokasi_KeyDown;
            tbLokasi.Name = "tbLokasi";
            tbLokasi.TabIndex = 3;
            // 
            // btnFavorit
            // 
            btnFavorit.BackColor = Color.White;
            btnFavorit.Cursor = Cursors.Hand;
            btnFavorit.FlatStyle = FlatStyle.Flat;
            btnFavorit.Font = new Font("Segoe UI", 9.75F);
            btnFavorit.ForeColor = Color.FromArgb(15, 76, 92);
            btnFavorit.Location = new Point(292, 92);
            btnFavorit.Size = new Size(32, 29);
            btnFavorit.Text = "☆";
            btnFavorit.UseVisualStyleBackColor = false;
            btnFavorit.Click += btnFavorit_Click;
            btnFavorit.FlatAppearance.BorderColor = Color.FromArgb(200, 212, 219);
            btnFavorit.Name = "btnFavorit";
            btnFavorit.TabIndex = 4;
            // 
            // btnLokasiSaya
            // 
            btnLokasiSaya.BackColor = Color.White;
            btnLokasiSaya.Cursor = Cursors.Hand;
            btnLokasiSaya.FlatStyle = FlatStyle.Flat;
            btnLokasiSaya.Font = new Font("Segoe UI", 9.75F);
            btnLokasiSaya.ForeColor = Color.FromArgb(15, 76, 92);
            btnLokasiSaya.Location = new Point(334, 92);
            btnLokasiSaya.Size = new Size(130, 29);
            btnLokasiSaya.Text = "Lokasi saya";
            btnLokasiSaya.UseVisualStyleBackColor = false;
            btnLokasiSaya.Click += btnLokasiSaya_Click;
            btnLokasiSaya.FlatAppearance.BorderColor = Color.FromArgb(200, 212, 219);
            btnLokasiSaya.Name = "btnLokasiSaya";
            btnLokasiSaya.TabIndex = 5;
            // 
            // lblLatitude
            // 
            lblLatitude.Font = new Font("Segoe UI Semibold", 9.75F);
            lblLatitude.ForeColor = Color.FromArgb(28, 43, 54);
            lblLatitude.Location = new Point(24, 126);
            lblLatitude.Text = "Latitude";
            lblLatitude.AutoSize = true;
            lblLatitude.Name = "lblLatitude";
            lblLatitude.TabIndex = 6;
            // 
            // tbLatitude
            // 
            tbLatitude.BorderStyle = BorderStyle.FixedSingle;
            tbLatitude.Font = new Font("Segoe UI", 11F);
            tbLatitude.Location = new Point(24, 147);
            tbLatitude.Size = new Size(110, 27);
            tbLatitude.Name = "tbLatitude";
            tbLatitude.TabIndex = 7;
            // 
            // lblLongitude
            // 
            lblLongitude.Font = new Font("Segoe UI Semibold", 9.75F);
            lblLongitude.ForeColor = Color.FromArgb(28, 43, 54);
            lblLongitude.Location = new Point(144, 126);
            lblLongitude.Text = "Longitude";
            lblLongitude.AutoSize = true;
            lblLongitude.Name = "lblLongitude";
            lblLongitude.TabIndex = 8;
            // 
            // tbLongitude
            // 
            tbLongitude.BorderStyle = BorderStyle.FixedSingle;
            tbLongitude.Font = new Font("Segoe UI", 11F);
            tbLongitude.Location = new Point(144, 147);
            tbLongitude.Size = new Size(110, 27);
            tbLongitude.Name = "tbLongitude";
            tbLongitude.TabIndex = 9;
            // 
            // btnCek
            // 
            btnCek.BackColor = Color.FromArgb(15, 76, 92);
            btnCek.Cursor = Cursors.Hand;
            btnCek.FlatStyle = FlatStyle.Flat;
            btnCek.Font = new Font("Segoe UI Semibold", 10F);
            btnCek.ForeColor = Color.White;
            btnCek.Location = new Point(334, 145);
            btnCek.Size = new Size(130, 31);
            btnCek.Text = "Cek cuaca";
            btnCek.UseVisualStyleBackColor = false;
            btnCek.Click += btnCek_Click;
            btnCek.FlatAppearance.BorderSize = 0;
            btnCek.Name = "btnCek";
            btnCek.TabIndex = 10;
            // 
            // lstSaran
            // 
            lstSaran.BorderStyle = BorderStyle.FixedSingle;
            lstSaran.DrawMode = DrawMode.OwnerDrawFixed;
            lstSaran.Font = new Font("Segoe UI", 10F);
            lstSaran.IntegralHeight = false;
            lstSaran.ItemHeight = 36;
            lstSaran.Location = new Point(24, 119);
            lstSaran.Size = new Size(300, 182);
            lstSaran.Visible = false;
            lstSaran.DrawItem += lstSaran_DrawItem;
            lstSaran.MouseClick += lstSaran_MouseClick;
            lstSaran.KeyDown += lstSaran_KeyDown;
            lstSaran.Name = "lstSaran";
            lstSaran.TabIndex = 11;
            // 
            // flowFavorit
            // 
            flowFavorit.Location = new Point(24, 182);
            flowFavorit.Size = new Size(500, 30);
            flowFavorit.WrapContents = false;
            flowFavorit.Name = "flowFavorit";
            flowFavorit.TabIndex = 12;
            // 
            // lblPesan
            // 
            lblPesan.Font = new Font("Segoe UI", 9.75F);
            lblPesan.ForeColor = Color.FromArgb(198, 40, 40);
            lblPesan.Location = new Point(24, 216);
            lblPesan.Text = "";
            lblPesan.AutoSize = false;
            lblPesan.Size = new Size(500, 20);
            lblPesan.Name = "lblPesan";
            lblPesan.TabIndex = 13;
            // 
            // pnlHero
            // 
            pnlHero.Controls.Add(picCuaca);
            pnlHero.Controls.Add(lblTerasa);
            pnlHero.Controls.Add(lblKondisi);
            pnlHero.Controls.Add(lblSuhu);
            pnlHero.Controls.Add(lblLokasiHasil);
            pnlHero.BackColor = Color.FromArgb(15, 76, 92);
            pnlHero.Location = new Point(24, 240);
            pnlHero.Size = new Size(500, 176);
            pnlHero.Paint += pnlHero_Paint;
            pnlHero.Name = "pnlHero";
            pnlHero.TabIndex = 14;
            // 
            // lblLokasiHasil
            // 
            lblLokasiHasil.Font = new Font("Segoe UI Semibold", 12F);
            lblLokasiHasil.ForeColor = Color.White;
            lblLokasiHasil.Location = new Point(18, 14);
            lblLokasiHasil.Text = "Belum ada lokasi";
            lblLokasiHasil.AutoSize = false;
            lblLokasiHasil.Size = new Size(320, 24);
            lblLokasiHasil.BackColor = Color.Transparent;
            lblLokasiHasil.Name = "lblLokasiHasil";
            lblLokasiHasil.TabIndex = 15;
            // 
            // lblSuhu
            // 
            lblSuhu.Font = new Font("Bahnschrift SemiBold", 54F);
            lblSuhu.ForeColor = Color.White;
            lblSuhu.Location = new Point(12, 38);
            lblSuhu.Text = "—";
            lblSuhu.AutoSize = true;
            lblSuhu.BackColor = Color.Transparent;
            lblSuhu.Name = "lblSuhu";
            lblSuhu.TabIndex = 16;
            // 
            // lblKondisi
            // 
            lblKondisi.Font = new Font("Segoe UI", 11F);
            lblKondisi.ForeColor = Color.White;
            lblKondisi.Location = new Point(19, 122);
            lblKondisi.Text = "Ketik nama kota, lalu tekan Cek cuaca.";
            lblKondisi.AutoSize = false;
            lblKondisi.Size = new Size(320, 22);
            lblKondisi.BackColor = Color.Transparent;
            lblKondisi.Name = "lblKondisi";
            lblKondisi.TabIndex = 17;
            // 
            // lblTerasa
            // 
            lblTerasa.Font = new Font("Segoe UI", 9.75F);
            lblTerasa.ForeColor = Color.White;
            lblTerasa.Location = new Point(19, 145);
            lblTerasa.Text = "";
            lblTerasa.AutoSize = false;
            lblTerasa.Size = new Size(320, 20);
            lblTerasa.BackColor = Color.Transparent;
            lblTerasa.Name = "lblTerasa";
            lblTerasa.TabIndex = 18;
            // 
            // picCuaca
            // 
            picCuaca.BackColor = Color.Transparent;
            picCuaca.Location = new Point(352, 22);
            picCuaca.Size = new Size(132, 132);
            picCuaca.SizeMode = PictureBoxSizeMode.Zoom;
            picCuaca.Name = "picCuaca";
            picCuaca.TabStop = false;
            // 
            // pnlJam
            // 
            pnlJam.Controls.Add(flowJam);
            pnlJam.Controls.Add(btnHarian);
            pnlJam.Controls.Add(btnPerJam);
            pnlJam.Controls.Add(lblJamJudul);
            pnlJam.BackColor = Color.White;
            pnlJam.BorderStyle = BorderStyle.FixedSingle;
            pnlJam.Location = new Point(24, 426);
            pnlJam.Size = new Size(500, 112);
            pnlJam.Name = "pnlJam";
            pnlJam.TabIndex = 19;
            // 
            // lblJamJudul
            // 
            lblJamJudul.Font = new Font("Segoe UI Semibold", 9F);
            lblJamJudul.ForeColor = Color.FromArgb(94, 110, 122);
            lblJamJudul.Location = new Point(10, 6);
            lblJamJudul.Text = "Prakiraan";
            lblJamJudul.AutoSize = true;
            lblJamJudul.Name = "lblJamJudul";
            lblJamJudul.TabIndex = 20;
            // 
            // btnPerJam
            // 
            btnPerJam.BackColor = Color.FromArgb(15, 76, 92);
            btnPerJam.Cursor = Cursors.Hand;
            btnPerJam.FlatStyle = FlatStyle.Flat;
            btnPerJam.Font = new Font("Segoe UI Semibold", 10F);
            btnPerJam.ForeColor = Color.White;
            btnPerJam.Location = new Point(334, 3);
            btnPerJam.Size = new Size(76, 22);
            btnPerJam.Text = "Per jam";
            btnPerJam.UseVisualStyleBackColor = false;
            btnPerJam.Click += btnPerJam_Click;
            btnPerJam.FlatAppearance.BorderSize = 0;
            btnPerJam.Name = "btnPerJam";
            btnPerJam.TabIndex = 21;
            // 
            // btnHarian
            // 
            btnHarian.BackColor = Color.White;
            btnHarian.Cursor = Cursors.Hand;
            btnHarian.FlatStyle = FlatStyle.Flat;
            btnHarian.Font = new Font("Segoe UI", 9.75F);
            btnHarian.ForeColor = Color.FromArgb(15, 76, 92);
            btnHarian.Location = new Point(412, 3);
            btnHarian.Size = new Size(76, 22);
            btnHarian.Text = "7 hari";
            btnHarian.UseVisualStyleBackColor = false;
            btnHarian.Click += btnHarian_Click;
            btnHarian.FlatAppearance.BorderColor = Color.FromArgb(200, 212, 219);
            btnHarian.Name = "btnHarian";
            btnHarian.TabIndex = 22;
            // 
            // flowJam
            // 
            flowJam.Location = new Point(6, 26);
            flowJam.Size = new Size(486, 82);
            flowJam.WrapContents = false;
            flowJam.Name = "flowJam";
            flowJam.TabIndex = 23;
            // 
            // pnlKelembapan
            // 
            pnlKelembapan.Controls.Add(lblKelembapan);
            pnlKelembapan.Controls.Add(lblKelembapanJudul);
            pnlKelembapan.Controls.Add(picKelembapan);
            pnlKelembapan.BackColor = Color.White;
            pnlKelembapan.BorderStyle = BorderStyle.FixedSingle;
            pnlKelembapan.Location = new Point(24, 548);
            pnlKelembapan.Size = new Size(92, 62);
            pnlKelembapan.Name = "pnlKelembapan";
            pnlKelembapan.TabIndex = 24;
            // 
            // picKelembapan
            // 
            picKelembapan.BackColor = Color.Transparent;
            picKelembapan.Location = new Point(4, 5);
            picKelembapan.Size = new Size(20, 20);
            picKelembapan.SizeMode = PictureBoxSizeMode.Zoom;
            picKelembapan.Name = "picKelembapan";
            picKelembapan.TabStop = false;
            // 
            // lblKelembapanJudul
            // 
            lblKelembapanJudul.Font = new Font("Segoe UI", 8.25F);
            lblKelembapanJudul.ForeColor = Color.FromArgb(94, 110, 122);
            lblKelembapanJudul.Location = new Point(25, 7);
            lblKelembapanJudul.Text = "Kelembapan";
            lblKelembapanJudul.AutoSize = true;
            lblKelembapanJudul.Name = "lblKelembapanJudul";
            lblKelembapanJudul.TabIndex = 25;
            // 
            // lblKelembapan
            // 
            lblKelembapan.Font = new Font("Bahnschrift SemiBold", 11F);
            lblKelembapan.ForeColor = Color.FromArgb(28, 43, 54);
            lblKelembapan.Location = new Point(5, 31);
            lblKelembapan.Text = "-";
            lblKelembapan.AutoSize = true;
            lblKelembapan.Name = "lblKelembapan";
            lblKelembapan.TabIndex = 26;
            // 
            // pnlAngin
            // 
            pnlAngin.Controls.Add(lblAngin);
            pnlAngin.Controls.Add(lblAnginJudul);
            pnlAngin.Controls.Add(picAngin);
            pnlAngin.BackColor = Color.White;
            pnlAngin.BorderStyle = BorderStyle.FixedSingle;
            pnlAngin.Location = new Point(126, 548);
            pnlAngin.Size = new Size(92, 62);
            pnlAngin.Name = "pnlAngin";
            pnlAngin.TabIndex = 27;
            // 
            // picAngin
            // 
            picAngin.BackColor = Color.Transparent;
            picAngin.Location = new Point(4, 5);
            picAngin.Size = new Size(20, 20);
            picAngin.SizeMode = PictureBoxSizeMode.Zoom;
            picAngin.Name = "picAngin";
            picAngin.TabStop = false;
            // 
            // lblAnginJudul
            // 
            lblAnginJudul.Font = new Font("Segoe UI", 8.25F);
            lblAnginJudul.ForeColor = Color.FromArgb(94, 110, 122);
            lblAnginJudul.Location = new Point(25, 7);
            lblAnginJudul.Text = "Angin";
            lblAnginJudul.AutoSize = true;
            lblAnginJudul.Name = "lblAnginJudul";
            lblAnginJudul.TabIndex = 28;
            // 
            // lblAngin
            // 
            lblAngin.Font = new Font("Bahnschrift SemiBold", 11F);
            lblAngin.ForeColor = Color.FromArgb(28, 43, 54);
            lblAngin.Location = new Point(5, 31);
            lblAngin.Text = "-";
            lblAngin.AutoSize = true;
            lblAngin.Name = "lblAngin";
            lblAngin.TabIndex = 29;
            // 
            // pnlHujan
            // 
            pnlHujan.Controls.Add(lblHujan);
            pnlHujan.Controls.Add(lblHujanJudul);
            pnlHujan.Controls.Add(picHujan);
            pnlHujan.BackColor = Color.White;
            pnlHujan.BorderStyle = BorderStyle.FixedSingle;
            pnlHujan.Location = new Point(228, 548);
            pnlHujan.Size = new Size(92, 62);
            pnlHujan.Name = "pnlHujan";
            pnlHujan.TabIndex = 30;
            // 
            // picHujan
            // 
            picHujan.BackColor = Color.Transparent;
            picHujan.Location = new Point(4, 5);
            picHujan.Size = new Size(20, 20);
            picHujan.SizeMode = PictureBoxSizeMode.Zoom;
            picHujan.Name = "picHujan";
            picHujan.TabStop = false;
            // 
            // lblHujanJudul
            // 
            lblHujanJudul.Font = new Font("Segoe UI", 8.25F);
            lblHujanJudul.ForeColor = Color.FromArgb(94, 110, 122);
            lblHujanJudul.Location = new Point(25, 7);
            lblHujanJudul.Text = "Hujan";
            lblHujanJudul.AutoSize = true;
            lblHujanJudul.Name = "lblHujanJudul";
            lblHujanJudul.TabIndex = 31;
            // 
            // lblHujan
            // 
            lblHujan.Font = new Font("Bahnschrift SemiBold", 11F);
            lblHujan.ForeColor = Color.FromArgb(28, 43, 54);
            lblHujan.Location = new Point(5, 31);
            lblHujan.Text = "-";
            lblHujan.AutoSize = true;
            lblHujan.Name = "lblHujan";
            lblHujan.TabIndex = 32;
            // 
            // pnlUv
            // 
            pnlUv.Controls.Add(lblUv);
            pnlUv.Controls.Add(lblUvJudul);
            pnlUv.Controls.Add(picUv);
            pnlUv.BackColor = Color.White;
            pnlUv.BorderStyle = BorderStyle.FixedSingle;
            pnlUv.Location = new Point(330, 548);
            pnlUv.Size = new Size(92, 62);
            pnlUv.Name = "pnlUv";
            pnlUv.TabIndex = 33;
            // 
            // picUv
            // 
            picUv.BackColor = Color.Transparent;
            picUv.Location = new Point(4, 5);
            picUv.Size = new Size(20, 20);
            picUv.SizeMode = PictureBoxSizeMode.Zoom;
            picUv.Name = "picUv";
            picUv.TabStop = false;
            // 
            // lblUvJudul
            // 
            lblUvJudul.Font = new Font("Segoe UI", 8.25F);
            lblUvJudul.ForeColor = Color.FromArgb(94, 110, 122);
            lblUvJudul.Location = new Point(25, 7);
            lblUvJudul.Text = "Indeks UV";
            lblUvJudul.AutoSize = true;
            lblUvJudul.Name = "lblUvJudul";
            lblUvJudul.TabIndex = 34;
            // 
            // lblUv
            // 
            lblUv.Font = new Font("Bahnschrift SemiBold", 11F);
            lblUv.ForeColor = Color.FromArgb(28, 43, 54);
            lblUv.Location = new Point(5, 31);
            lblUv.Text = "-";
            lblUv.AutoSize = true;
            lblUv.Name = "lblUv";
            lblUv.TabIndex = 35;
            // 
            // pnlUdara
            // 
            pnlUdara.Controls.Add(lblUdara);
            pnlUdara.Controls.Add(lblUdaraJudul);
            pnlUdara.Controls.Add(picUdara);
            pnlUdara.BackColor = Color.White;
            pnlUdara.BorderStyle = BorderStyle.FixedSingle;
            pnlUdara.Location = new Point(432, 548);
            pnlUdara.Size = new Size(92, 62);
            pnlUdara.Name = "pnlUdara";
            pnlUdara.TabIndex = 36;
            // 
            // picUdara
            // 
            picUdara.BackColor = Color.Transparent;
            picUdara.Location = new Point(4, 5);
            picUdara.Size = new Size(20, 20);
            picUdara.SizeMode = PictureBoxSizeMode.Zoom;
            picUdara.Name = "picUdara";
            picUdara.TabStop = false;
            // 
            // lblUdaraJudul
            // 
            lblUdaraJudul.Font = new Font("Segoe UI", 8.25F);
            lblUdaraJudul.ForeColor = Color.FromArgb(94, 110, 122);
            lblUdaraJudul.Location = new Point(25, 7);
            lblUdaraJudul.Text = "Udara (AQI)";
            lblUdaraJudul.AutoSize = true;
            lblUdaraJudul.Name = "lblUdaraJudul";
            lblUdaraJudul.TabIndex = 37;
            // 
            // lblUdara
            // 
            lblUdara.Font = new Font("Bahnschrift SemiBold", 11F);
            lblUdara.ForeColor = Color.FromArgb(28, 43, 54);
            lblUdara.Location = new Point(5, 31);
            lblUdara.Text = "-";
            lblUdara.AutoSize = true;
            lblUdara.Name = "lblUdara";
            lblUdara.TabIndex = 38;
            // 
            // pnlRisiko
            // 
            pnlRisiko.Controls.Add(lblRincian);
            pnlRisiko.Controls.Add(lblStatus);
            pnlRisiko.Controls.Add(lblRisikoJudul);
            pnlRisiko.Controls.Add(picStatus);
            pnlRisiko.BackColor = Color.FromArgb(214, 223, 229);
            pnlRisiko.Location = new Point(544, 72);
            pnlRisiko.Size = new Size(312, 196);
            pnlRisiko.Name = "pnlRisiko";
            pnlRisiko.TabIndex = 39;
            // 
            // picStatus
            // 
            picStatus.BackColor = Color.Transparent;
            picStatus.Location = new Point(14, 16);
            picStatus.Size = new Size(44, 44);
            picStatus.SizeMode = PictureBoxSizeMode.Zoom;
            picStatus.Name = "picStatus";
            picStatus.TabStop = false;
            // 
            // lblRisikoJudul
            // 
            lblRisikoJudul.Font = new Font("Segoe UI", 9.75F);
            lblRisikoJudul.ForeColor = Color.FromArgb(28, 43, 54);
            lblRisikoJudul.Location = new Point(66, 14);
            lblRisikoJudul.Text = "Status risiko";
            lblRisikoJudul.AutoSize = true;
            lblRisikoJudul.BackColor = Color.Transparent;
            lblRisikoJudul.Name = "lblRisikoJudul";
            lblRisikoJudul.TabIndex = 40;
            // 
            // lblStatus
            // 
            lblStatus.Font = new Font("Bahnschrift SemiBold", 22F);
            lblStatus.ForeColor = Color.FromArgb(28, 43, 54);
            lblStatus.Location = new Point(63, 32);
            lblStatus.Text = "Belum dicek";
            lblStatus.AutoSize = true;
            lblStatus.BackColor = Color.Transparent;
            lblStatus.Name = "lblStatus";
            lblStatus.TabIndex = 41;
            // 
            // lblRincian
            // 
            lblRincian.Font = new Font("Segoe UI", 9F);
            lblRincian.ForeColor = Color.FromArgb(28, 43, 54);
            lblRincian.Location = new Point(14, 76);
            lblRincian.Text = "Status muncul setelah cuaca dicek.";
            lblRincian.AutoSize = false;
            lblRincian.Size = new Size(286, 112);
            lblRincian.BackColor = Color.Transparent;
            lblRincian.Name = "lblRincian";
            lblRincian.TabIndex = 42;
            // 
            // lblRekomendasiJudul
            // 
            lblRekomendasiJudul.Font = new Font("Segoe UI Semibold", 9.75F);
            lblRekomendasiJudul.ForeColor = Color.FromArgb(28, 43, 54);
            lblRekomendasiJudul.Location = new Point(544, 280);
            lblRekomendasiJudul.Text = "Yang sebaiknya dilakukan";
            lblRekomendasiJudul.AutoSize = true;
            lblRekomendasiJudul.Name = "lblRekomendasiJudul";
            lblRekomendasiJudul.TabIndex = 43;
            // 
            // flowRekomendasi
            // 
            flowRekomendasi.AutoScroll = true;
            flowRekomendasi.FlowDirection = FlowDirection.TopDown;
            flowRekomendasi.Location = new Point(544, 302);
            flowRekomendasi.Size = new Size(316, 308);
            flowRekomendasi.WrapContents = false;
            flowRekomendasi.Name = "flowRekomendasi";
            flowRekomendasi.TabIndex = 44;
            // 
            // lblSumber
            // 
            lblSumber.Font = new Font("Segoe UI", 9F);
            lblSumber.ForeColor = Color.FromArgb(94, 110, 122);
            lblSumber.Location = new Point(24, 628);
            lblSumber.Text = "Data cuaca dari Open-Meteo";
            lblSumber.AutoSize = false;
            lblSumber.Size = new Size(700, 18);
            lblSumber.Name = "lblSumber";
            lblSumber.TabIndex = 45;
            // 
            // btnKembali
            // 
            btnKembali.BackColor = Color.White;
            btnKembali.Cursor = Cursors.Hand;
            btnKembali.FlatStyle = FlatStyle.Flat;
            btnKembali.Font = new Font("Segoe UI", 9.75F);
            btnKembali.ForeColor = Color.FromArgb(15, 76, 92);
            btnKembali.Location = new Point(756, 622);
            btnKembali.Size = new Size(100, 32);
            btnKembali.Text = "Kembali";
            btnKembali.UseVisualStyleBackColor = false;
            btnKembali.Click += btnKembali_Click;
            btnKembali.FlatAppearance.BorderColor = Color.FromArgb(200, 212, 219);
            btnKembali.Name = "btnKembali";
            btnKembali.TabIndex = 46;
            // 
            // FormCekCuaca
            // 
            AcceptButton = btnCek;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 248);
            ClientSize = new Size(880, 668);
            Controls.Add(btnKembali);
            Controls.Add(lblSumber);
            Controls.Add(flowRekomendasi);
            Controls.Add(lblRekomendasiJudul);
            Controls.Add(pnlRisiko);
            Controls.Add(pnlUdara);
            Controls.Add(pnlUv);
            Controls.Add(pnlHujan);
            Controls.Add(pnlAngin);
            Controls.Add(pnlKelembapan);
            Controls.Add(pnlJam);
            Controls.Add(pnlHero);
            Controls.Add(lblPesan);
            Controls.Add(flowFavorit);
            Controls.Add(lstSaran);
            Controls.Add(btnCek);
            Controls.Add(tbLongitude);
            Controls.Add(lblLongitude);
            Controls.Add(tbLatitude);
            Controls.Add(lblLatitude);
            Controls.Add(btnLokasiSaya);
            Controls.Add(btnFavorit);
            Controls.Add(tbLokasi);
            Controls.Add(lblLokasi);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormCekCuaca";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SWASANA - Cek cuaca";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            flowFavorit.ResumeLayout(false);
            flowFavorit.PerformLayout();
            pnlHero.ResumeLayout(false);
            pnlHero.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picCuaca).EndInit();
            pnlJam.ResumeLayout(false);
            pnlJam.PerformLayout();
            flowJam.ResumeLayout(false);
            flowJam.PerformLayout();
            pnlKelembapan.ResumeLayout(false);
            pnlKelembapan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picKelembapan).EndInit();
            pnlAngin.ResumeLayout(false);
            pnlAngin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picAngin).EndInit();
            pnlHujan.ResumeLayout(false);
            pnlHujan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picHujan).EndInit();
            pnlUv.ResumeLayout(false);
            pnlUv.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picUv).EndInit();
            pnlUdara.ResumeLayout(false);
            pnlUdara.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picUdara).EndInit();
            pnlRisiko.ResumeLayout(false);
            pnlRisiko.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picStatus).EndInit();
            flowRekomendasi.ResumeLayout(false);
            flowRekomendasi.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlHeader;
        private Label lblJudul;
        private Label lblLokasi;
        private TextBox tbLokasi;
        private Button btnFavorit;
        private Button btnLokasiSaya;
        private Label lblLatitude;
        private TextBox tbLatitude;
        private Label lblLongitude;
        private TextBox tbLongitude;
        private Button btnCek;
        private ListBox lstSaran;
        private FlowLayoutPanel flowFavorit;
        private Label lblPesan;
        private Panel pnlHero;
        private Label lblLokasiHasil;
        private Label lblSuhu;
        private Label lblKondisi;
        private Label lblTerasa;
        private PictureBox picCuaca;
        private Panel pnlJam;
        private Label lblJamJudul;
        private Button btnPerJam;
        private Button btnHarian;
        private FlowLayoutPanel flowJam;
        private Panel pnlKelembapan;
        private PictureBox picKelembapan;
        private Label lblKelembapanJudul;
        private Label lblKelembapan;
        private Panel pnlAngin;
        private PictureBox picAngin;
        private Label lblAnginJudul;
        private Label lblAngin;
        private Panel pnlHujan;
        private PictureBox picHujan;
        private Label lblHujanJudul;
        private Label lblHujan;
        private Panel pnlUv;
        private PictureBox picUv;
        private Label lblUvJudul;
        private Label lblUv;
        private Panel pnlUdara;
        private PictureBox picUdara;
        private Label lblUdaraJudul;
        private Label lblUdara;
        private Panel pnlRisiko;
        private PictureBox picStatus;
        private Label lblRisikoJudul;
        private Label lblStatus;
        private Label lblRincian;
        private Label lblRekomendasiJudul;
        private FlowLayoutPanel flowRekomendasi;
        private Label lblSumber;
        private Button btnKembali;
    }
}
