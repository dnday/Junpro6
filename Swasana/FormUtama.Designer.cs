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
            lblJudul = new Label();
            lblSubjudul = new Label();
            lblUser = new Label();
            btnCekCuaca = new Button();
            btnRiwayat = new Button();
            btnTentang = new Button();
            btnKeluar = new Button();
            SuspendLayout();
            //
            // lblJudul
            //
            lblJudul.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblJudul.ForeColor = Color.FromArgb(21, 101, 192);
            lblJudul.Location = new Point(0, 30);
            lblJudul.Name = "lblJudul";
            lblJudul.Size = new Size(420, 50);
            lblJudul.TabIndex = 0;
            lblJudul.Text = "SWASANA";
            lblJudul.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblSubjudul
            //
            lblSubjudul.Font = new Font("Segoe UI", 9.5F);
            lblSubjudul.ForeColor = Color.DimGray;
            lblSubjudul.Location = new Point(0, 80);
            lblSubjudul.Name = "lblSubjudul";
            lblSubjudul.Size = new Size(420, 20);
            lblSubjudul.TabIndex = 1;
            lblSubjudul.Text = "Sistem Waspada Suasana && Adaptasi Iklim";
            lblSubjudul.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUser
            // 
            lblUser.ForeColor = Color.DimGray;
            lblUser.Location = new Point(0, 322);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(420, 20);
            lblUser.TabIndex = 6;
            lblUser.Text = "Masuk sebagai : -";
            lblUser.TextAlign = ContentAlignment.MiddleCenter;
            //
            // btnCekCuaca
            //
            btnCekCuaca.Font = new Font("Segoe UI", 10F);
            btnCekCuaca.Location = new Point(110, 130);
            btnCekCuaca.Name = "btnCekCuaca";
            btnCekCuaca.Size = new Size(200, 38);
            btnCekCuaca.TabIndex = 2;
            btnCekCuaca.Text = "Cek Cuaca";
            btnCekCuaca.UseVisualStyleBackColor = true;
            btnCekCuaca.Click += btnCekCuaca_Click;
            //
            // btnRiwayat
            //
            btnRiwayat.Font = new Font("Segoe UI", 10F);
            btnRiwayat.Location = new Point(110, 178);
            btnRiwayat.Name = "btnRiwayat";
            btnRiwayat.Size = new Size(200, 38);
            btnRiwayat.TabIndex = 3;
            btnRiwayat.Text = "Riwayat";
            btnRiwayat.UseVisualStyleBackColor = true;
            btnRiwayat.Click += btnRiwayat_Click;
            //
            // btnTentang
            //
            btnTentang.Font = new Font("Segoe UI", 10F);
            btnTentang.Location = new Point(110, 226);
            btnTentang.Name = "btnTentang";
            btnTentang.Size = new Size(200, 38);
            btnTentang.TabIndex = 4;
            btnTentang.Text = "Tentang";
            btnTentang.UseVisualStyleBackColor = true;
            btnTentang.Click += btnTentang_Click;
            //
            // btnKeluar
            //
            btnKeluar.Font = new Font("Segoe UI", 10F);
            btnKeluar.Location = new Point(110, 274);
            btnKeluar.Name = "btnKeluar";
            btnKeluar.Size = new Size(200, 38);
            btnKeluar.TabIndex = 5;
            btnKeluar.Text = "Keluar";
            btnKeluar.UseVisualStyleBackColor = true;
            btnKeluar.Click += btnKeluar_Click;
            //
            // FormUtama
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(420, 355);
            Controls.Add(btnKeluar);
            Controls.Add(btnTentang);
            Controls.Add(btnRiwayat);
            Controls.Add(btnCekCuaca);
            Controls.Add(lblUser);
            Controls.Add(lblSubjudul);
            Controls.Add(lblJudul);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormUtama";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SWASANA - Menu Utama";
            ResumeLayout(false);
        }

        #endregion

        private Label lblJudul;
        private Label lblSubjudul;
        private Label lblUser;
        private Button btnCekCuaca;
        private Button btnRiwayat;
        private Button btnTentang;
        private Button btnKeluar;
    }
}
