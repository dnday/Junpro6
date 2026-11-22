namespace Swasana
{
    partial class FormUtama
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
            picLogo = new PictureBox();
            lblJudul = new Label();
            lblUser = new Label();
            btnCekCuaca = new Button();
            btnRiwayat = new Button();
            btnTentang = new Button();
            btnKeluar = new Button();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            SuspendLayout();
            //
            // pnlHeader
            //
            pnlHeader.BackColor = Color.FromArgb(15, 76, 92);
            pnlHeader.Controls.Add(lblUser);
            pnlHeader.Controls.Add(lblJudul);
            pnlHeader.Controls.Add(picLogo);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(420, 104);
            pnlHeader.TabIndex = 0;
            //
            // picLogo
            //
            picLogo.Location = new Point(22, 20);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(64, 64);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 0;
            picLogo.TabStop = false;
            //
            // lblJudul
            //
            lblJudul.AutoSize = true;
            lblJudul.Font = new Font("Bahnschrift SemiBold", 24F);
            lblJudul.ForeColor = Color.White;
            lblJudul.Location = new Point(92, 20);
            lblJudul.Name = "lblJudul";
            lblJudul.Size = new Size(154, 39);
            lblJudul.TabIndex = 1;
            lblJudul.Text = "SWASANA";
            //
            // lblUser
            //
            lblUser.AutoSize = true;
            lblUser.Font = new Font("Segoe UI", 9.75F);
            lblUser.ForeColor = Color.FromArgb(198, 224, 230);
            lblUser.Location = new Point(95, 60);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(44, 17);
            lblUser.TabIndex = 2;
            lblUser.Text = "Halo, -";
            //
            // btnCekCuaca
            //
            btnCekCuaca.BackColor = Color.White;
            btnCekCuaca.Cursor = Cursors.Hand;
            btnCekCuaca.FlatAppearance.BorderColor = Color.FromArgb(200, 212, 219);
            btnCekCuaca.FlatStyle = FlatStyle.Flat;
            btnCekCuaca.Font = new Font("Segoe UI", 10F);
            btnCekCuaca.ForeColor = Color.FromArgb(28, 43, 54);
            btnCekCuaca.ImageAlign = ContentAlignment.MiddleLeft;
            btnCekCuaca.Location = new Point(24, 128);
            btnCekCuaca.Name = "btnCekCuaca";
            btnCekCuaca.Padding = new Padding(10, 0, 0, 0);
            btnCekCuaca.Size = new Size(372, 64);
            btnCekCuaca.TabIndex = 1;
            btnCekCuaca.Text = "Cek cuaca\r\nCuaca terkini dan tingkat risikonya";
            btnCekCuaca.TextAlign = ContentAlignment.MiddleLeft;
            btnCekCuaca.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCekCuaca.UseVisualStyleBackColor = false;
            btnCekCuaca.Click += btnCekCuaca_Click;
            //
            // btnRiwayat
            //
            btnRiwayat.BackColor = Color.White;
            btnRiwayat.Cursor = Cursors.Hand;
            btnRiwayat.FlatAppearance.BorderColor = Color.FromArgb(200, 212, 219);
            btnRiwayat.FlatStyle = FlatStyle.Flat;
            btnRiwayat.Font = new Font("Segoe UI", 10F);
            btnRiwayat.ForeColor = Color.FromArgb(28, 43, 54);
            btnRiwayat.ImageAlign = ContentAlignment.MiddleLeft;
            btnRiwayat.Location = new Point(24, 204);
            btnRiwayat.Name = "btnRiwayat";
            btnRiwayat.Padding = new Padding(10, 0, 0, 0);
            btnRiwayat.Size = new Size(372, 64);
            btnRiwayat.TabIndex = 2;
            btnRiwayat.Text = "Riwayat\r\nSemua hasil cek cuaca yang tersimpan";
            btnRiwayat.TextAlign = ContentAlignment.MiddleLeft;
            btnRiwayat.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnRiwayat.UseVisualStyleBackColor = false;
            btnRiwayat.Click += btnRiwayat_Click;
            //
            // btnTentang
            //
            btnTentang.BackColor = Color.White;
            btnTentang.Cursor = Cursors.Hand;
            btnTentang.FlatAppearance.BorderColor = Color.FromArgb(200, 212, 219);
            btnTentang.FlatStyle = FlatStyle.Flat;
            btnTentang.Font = new Font("Segoe UI", 9.75F);
            btnTentang.ForeColor = Color.FromArgb(15, 76, 92);
            btnTentang.Location = new Point(24, 292);
            btnTentang.Name = "btnTentang";
            btnTentang.Size = new Size(180, 34);
            btnTentang.TabIndex = 3;
            btnTentang.Text = "Tentang SWASANA";
            btnTentang.UseVisualStyleBackColor = false;
            btnTentang.Click += btnTentang_Click;
            //
            // btnKeluar
            //
            btnKeluar.BackColor = Color.White;
            btnKeluar.Cursor = Cursors.Hand;
            btnKeluar.FlatAppearance.BorderColor = Color.FromArgb(200, 212, 219);
            btnKeluar.FlatStyle = FlatStyle.Flat;
            btnKeluar.Font = new Font("Segoe UI", 9.75F);
            btnKeluar.ForeColor = Color.FromArgb(15, 76, 92);
            btnKeluar.Location = new Point(216, 292);
            btnKeluar.Name = "btnKeluar";
            btnKeluar.Size = new Size(180, 34);
            btnKeluar.TabIndex = 4;
            btnKeluar.Text = "Keluar";
            btnKeluar.UseVisualStyleBackColor = false;
            btnKeluar.Click += btnKeluar_Click;
            //
            // FormUtama
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 248);
            ClientSize = new Size(420, 350);
            Controls.Add(btnKeluar);
            Controls.Add(btnTentang);
            Controls.Add(btnRiwayat);
            Controls.Add(btnCekCuaca);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormUtama";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SWASANA";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private PictureBox picLogo;
        private Label lblJudul;
        private Label lblUser;
        private Button btnCekCuaca;
        private Button btnRiwayat;
        private Button btnTentang;
        private Button btnKeluar;
    }
}
