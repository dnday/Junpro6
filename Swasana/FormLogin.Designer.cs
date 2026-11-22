namespace Swasana
{
    partial class FormLogin
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
            lblTagline = new Label();
            lblUsername = new Label();
            tbUsername = new TextBox();
            lblInfo = new Label();
            lblPesan = new Label();
            btnLogin = new Button();
            btnClose = new Button();
            lnkPengaturan = new LinkLabel();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            SuspendLayout();
            //
            // pnlHeader
            //
            pnlHeader.BackColor = Color.FromArgb(15, 76, 92);
            pnlHeader.Controls.Add(lblTagline);
            pnlHeader.Controls.Add(lblJudul);
            pnlHeader.Controls.Add(picLogo);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(400, 104);
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
            // lblTagline
            //
            lblTagline.AutoSize = true;
            lblTagline.Font = new Font("Segoe UI", 9.75F);
            lblTagline.ForeColor = Color.FromArgb(198, 224, 230);
            lblTagline.Location = new Point(95, 60);
            lblTagline.Name = "lblTagline";
            lblTagline.Size = new Size(205, 17);
            lblTagline.TabIndex = 2;
            lblTagline.Text = "Waspada cuaca, siap beradaptasi";
            //
            // lblUsername
            //
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI Semibold", 9.75F);
            lblUsername.ForeColor = Color.FromArgb(28, 43, 54);
            lblUsername.Location = new Point(28, 128);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(66, 17);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "Username";
            //
            // tbUsername
            //
            tbUsername.BorderStyle = BorderStyle.FixedSingle;
            tbUsername.Font = new Font("Segoe UI", 11F);
            tbUsername.Location = new Point(28, 150);
            tbUsername.MaxLength = 50;
            tbUsername.Name = "tbUsername";
            tbUsername.Size = new Size(344, 27);
            tbUsername.TabIndex = 2;
            //
            // lblInfo
            //
            lblInfo.Font = new Font("Segoe UI", 9F);
            lblInfo.ForeColor = Color.FromArgb(94, 110, 122);
            lblInfo.Location = new Point(28, 182);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(344, 18);
            lblInfo.TabIndex = 3;
            lblInfo.Text = "Belum punya akun? Ketik username baru, lalu konfirmasi.";
            //
            // lblPesan
            //
            lblPesan.Font = new Font("Segoe UI", 9F);
            lblPesan.ForeColor = Color.FromArgb(198, 40, 40);
            lblPesan.Location = new Point(28, 204);
            lblPesan.Name = "lblPesan";
            lblPesan.Size = new Size(344, 34);
            lblPesan.TabIndex = 4;
            //
            // btnLogin
            //
            btnLogin.BackColor = Color.FromArgb(15, 76, 92);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI Semibold", 10F);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(28, 242);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(344, 40);
            btnLogin.TabIndex = 5;
            btnLogin.Text = "Masuk";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            //
            // btnClose
            //
            btnClose.BackColor = Color.White;
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(200, 212, 219);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 9.75F);
            btnClose.ForeColor = Color.FromArgb(15, 76, 92);
            btnClose.Location = new Point(272, 298);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 32);
            btnClose.TabIndex = 7;
            btnClose.Text = "Keluar";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            //
            // lnkPengaturan
            //
            lnkPengaturan.ActiveLinkColor = Color.FromArgb(15, 76, 92);
            lnkPengaturan.AutoSize = true;
            lnkPengaturan.Font = new Font("Segoe UI", 9.75F);
            lnkPengaturan.LinkColor = Color.FromArgb(15, 76, 92);
            lnkPengaturan.Location = new Point(26, 305);
            lnkPengaturan.Name = "lnkPengaturan";
            lnkPengaturan.Size = new Size(140, 17);
            lnkPengaturan.TabIndex = 6;
            lnkPengaturan.TabStop = true;
            lnkPengaturan.Text = "Atur koneksi database";
            lnkPengaturan.LinkClicked += lnkPengaturan_LinkClicked;
            //
            // FormLogin
            //
            AcceptButton = btnLogin;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 248);
            ClientSize = new Size(400, 350);
            Controls.Add(lnkPengaturan);
            Controls.Add(btnClose);
            Controls.Add(btnLogin);
            Controls.Add(lblPesan);
            Controls.Add(lblInfo);
            Controls.Add(tbUsername);
            Controls.Add(lblUsername);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SWASANA - Masuk";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlHeader;
        private PictureBox picLogo;
        private Label lblJudul;
        private Label lblTagline;
        private Label lblUsername;
        private TextBox tbUsername;
        private Label lblInfo;
        private Label lblPesan;
        private Button btnLogin;
        private Button btnClose;
        private LinkLabel lnkPengaturan;
    }
}
