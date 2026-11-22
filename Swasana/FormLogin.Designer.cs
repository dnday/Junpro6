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
            lblJudul = new Label();
            lblInfo = new Label();
            lblUsername = new Label();
            tbUsername = new TextBox();
            btnLogin = new Button();
            btnClose = new Button();
            lnkPengaturan = new LinkLabel();
            SuspendLayout();
            //
            // lblJudul
            //
            lblJudul.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblJudul.ForeColor = Color.FromArgb(21, 101, 192);
            lblJudul.Location = new Point(0, 20);
            lblJudul.Name = "lblJudul";
            lblJudul.Size = new Size(380, 40);
            lblJudul.TabIndex = 0;
            lblJudul.Text = "SWASANA";
            lblJudul.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblInfo
            //
            lblInfo.ForeColor = Color.DimGray;
            lblInfo.Location = new Point(0, 62);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(380, 20);
            lblInfo.TabIndex = 1;
            lblInfo.Text = "Username baru otomatis didaftarkan.";
            lblInfo.TextAlign = ContentAlignment.MiddleCenter;
            //
            // lblUsername
            //
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(40, 103);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(66, 15);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Username :";
            //
            // tbUsername
            //
            tbUsername.Location = new Point(120, 100);
            tbUsername.MaxLength = 50;
            tbUsername.Name = "tbUsername";
            tbUsername.Size = new Size(220, 23);
            tbUsername.TabIndex = 3;
            //
            // btnLogin
            //
            btnLogin.Location = new Point(120, 140);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(105, 32);
            btnLogin.TabIndex = 4;
            btnLogin.Text = "Masuk";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            //
            // btnClose
            //
            btnClose.Location = new Point(235, 140);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(105, 32);
            btnClose.TabIndex = 5;
            btnClose.Text = "Keluar";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            //
            // lnkPengaturan
            //
            lnkPengaturan.AutoSize = true;
            lnkPengaturan.Location = new Point(120, 190);
            lnkPengaturan.Name = "lnkPengaturan";
            lnkPengaturan.Size = new Size(120, 15);
            lnkPengaturan.TabIndex = 6;
            lnkPengaturan.TabStop = true;
            lnkPengaturan.Text = "Pengaturan Database";
            lnkPengaturan.LinkClicked += lnkPengaturan_LinkClicked;
            //
            // FormLogin
            //
            AcceptButton = btnLogin;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(380, 225);
            Controls.Add(lnkPengaturan);
            Controls.Add(btnClose);
            Controls.Add(btnLogin);
            Controls.Add(tbUsername);
            Controls.Add(lblUsername);
            Controls.Add(lblInfo);
            Controls.Add(lblJudul);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SWASANA - Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblJudul;
        private Label lblInfo;
        private Label lblUsername;
        private TextBox tbUsername;
        private Button btnLogin;
        private Button btnClose;
        private LinkLabel lnkPengaturan;
    }
}
