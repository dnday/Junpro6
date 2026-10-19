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
            lblJudul = new Label();
            lblLokasi = new Label();
            tbLokasi = new TextBox();
            lblLatitude = new Label();
            tbLatitude = new TextBox();
            lblLongitude = new Label();
            tbLongitude = new TextBox();
            btnCek = new Button();
            gbHasil = new GroupBox();
            lblSuhu = new Label();
            lblKelembapan = new Label();
            lblAngin = new Label();
            lblHujan = new Label();
            lblStatus = new Label();
            lblRekomendasi = new Label();
            btnKembali = new Button();
            gbHasil.SuspendLayout();
            SuspendLayout();
            //
            // lblJudul
            //
            lblJudul.AutoSize = true;
            lblJudul.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblJudul.Location = new Point(20, 15);
            lblJudul.Name = "lblJudul";
            lblJudul.Size = new Size(126, 30);
            lblJudul.TabIndex = 0;
            lblJudul.Text = "Cek Cuaca";
            //
            // lblLokasi
            //
            lblLokasi.AutoSize = true;
            lblLokasi.Location = new Point(22, 63);
            lblLokasi.Name = "lblLokasi";
            lblLokasi.Size = new Size(46, 15);
            lblLokasi.TabIndex = 1;
            lblLokasi.Text = "Lokasi :";
            //
            // tbLokasi
            //
            tbLokasi.Location = new Point(110, 60);
            tbLokasi.Name = "tbLokasi";
            tbLokasi.Size = new Size(250, 23);
            tbLokasi.TabIndex = 2;
            tbLokasi.Text = "Yogyakarta";
            //
            // lblLatitude
            //
            lblLatitude.AutoSize = true;
            lblLatitude.Location = new Point(22, 96);
            lblLatitude.Name = "lblLatitude";
            lblLatitude.Size = new Size(56, 15);
            lblLatitude.TabIndex = 3;
            lblLatitude.Text = "Latitude :";
            //
            // tbLatitude
            //
            tbLatitude.Location = new Point(110, 93);
            tbLatitude.Name = "tbLatitude";
            tbLatitude.Size = new Size(100, 23);
            tbLatitude.TabIndex = 4;
            tbLatitude.Text = "-7.7956";
            //
            // lblLongitude
            //
            lblLongitude.AutoSize = true;
            lblLongitude.Location = new Point(222, 96);
            lblLongitude.Name = "lblLongitude";
            lblLongitude.Size = new Size(67, 15);
            lblLongitude.TabIndex = 5;
            lblLongitude.Text = "Longitude :";
            //
            // tbLongitude
            //
            tbLongitude.Location = new Point(295, 93);
            tbLongitude.Name = "tbLongitude";
            tbLongitude.Size = new Size(100, 23);
            tbLongitude.TabIndex = 6;
            tbLongitude.Text = "110.3695";
            //
            // btnCek
            //
            btnCek.Location = new Point(410, 59);
            btnCek.Name = "btnCek";
            btnCek.Size = new Size(110, 57);
            btnCek.TabIndex = 7;
            btnCek.Text = "Cek Cuaca";
            btnCek.UseVisualStyleBackColor = true;
            btnCek.Click += btnCek_Click;
            //
            // gbHasil
            //
            gbHasil.Controls.Add(lblRekomendasi);
            gbHasil.Controls.Add(lblStatus);
            gbHasil.Controls.Add(lblHujan);
            gbHasil.Controls.Add(lblAngin);
            gbHasil.Controls.Add(lblKelembapan);
            gbHasil.Controls.Add(lblSuhu);
            gbHasil.Location = new Point(22, 135);
            gbHasil.Name = "gbHasil";
            gbHasil.Size = new Size(498, 230);
            gbHasil.TabIndex = 8;
            gbHasil.TabStop = false;
            gbHasil.Text = "Hasil Cuaca && Risiko";
            //
            // lblSuhu
            //
            lblSuhu.AutoSize = true;
            lblSuhu.Location = new Point(15, 28);
            lblSuhu.Name = "lblSuhu";
            lblSuhu.Size = new Size(48, 15);
            lblSuhu.TabIndex = 0;
            lblSuhu.Text = "Suhu : -";
            //
            // lblKelembapan
            //
            lblKelembapan.AutoSize = true;
            lblKelembapan.Location = new Point(15, 52);
            lblKelembapan.Name = "lblKelembapan";
            lblKelembapan.Size = new Size(85, 15);
            lblKelembapan.TabIndex = 1;
            lblKelembapan.Text = "Kelembapan : -";
            //
            // lblAngin
            //
            lblAngin.AutoSize = true;
            lblAngin.Location = new Point(250, 28);
            lblAngin.Name = "lblAngin";
            lblAngin.Size = new Size(54, 15);
            lblAngin.TabIndex = 2;
            lblAngin.Text = "Angin : -";
            //
            // lblHujan
            //
            lblHujan.AutoSize = true;
            lblHujan.Location = new Point(250, 52);
            lblHujan.Name = "lblHujan";
            lblHujan.Size = new Size(87, 15);
            lblHujan.TabIndex = 3;
            lblHujan.Text = "Curah hujan : -";
            //
            // lblStatus
            //
            lblStatus.BackColor = Color.Gainsboro;
            lblStatus.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(15, 82);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(468, 40);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "STATUS : -";
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblRekomendasi
            //
            lblRekomendasi.Location = new Point(15, 132);
            lblRekomendasi.Name = "lblRekomendasi";
            lblRekomendasi.Size = new Size(468, 85);
            lblRekomendasi.TabIndex = 5;
            lblRekomendasi.Text = "Rekomendasi : -";
            //
            // btnKembali
            //
            btnKembali.Location = new Point(22, 380);
            btnKembali.Name = "btnKembali";
            btnKembali.Size = new Size(100, 30);
            btnKembali.TabIndex = 9;
            btnKembali.Text = "Kembali";
            btnKembali.UseVisualStyleBackColor = true;
            btnKembali.Click += btnKembali_Click;
            //
            // FormCekCuaca
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(544, 425);
            Controls.Add(btnKembali);
            Controls.Add(gbHasil);
            Controls.Add(btnCek);
            Controls.Add(tbLongitude);
            Controls.Add(lblLongitude);
            Controls.Add(tbLatitude);
            Controls.Add(lblLatitude);
            Controls.Add(tbLokasi);
            Controls.Add(lblLokasi);
            Controls.Add(lblJudul);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormCekCuaca";
            StartPosition = FormStartPosition.CenterParent;
            Text = "SWASANA - Cek Cuaca";
            gbHasil.ResumeLayout(false);
            gbHasil.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblJudul;
        private Label lblLokasi;
        private TextBox tbLokasi;
        private Label lblLatitude;
        private TextBox tbLatitude;
        private Label lblLongitude;
        private TextBox tbLongitude;
        private Button btnCek;
        private GroupBox gbHasil;
        private Label lblSuhu;
        private Label lblKelembapan;
        private Label lblAngin;
        private Label lblHujan;
        private Label lblStatus;
        private Label lblRekomendasi;
        private Button btnKembali;
    }
}
