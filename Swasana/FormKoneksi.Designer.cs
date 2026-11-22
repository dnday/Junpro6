namespace Swasana
{
    partial class FormKoneksi
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
            lblHost = new Label();
            tbHost = new TextBox();
            lblPort = new Label();
            tbPort = new TextBox();
            lblDatabase = new Label();
            tbDatabase = new TextBox();
            lblUsername = new Label();
            tbUsername = new TextBox();
            lblPassword = new Label();
            tbPassword = new TextBox();
            btnSimpan = new Button();
            btnBatal = new Button();
            SuspendLayout();
            //
            // lblJudul
            //
            lblJudul.AutoSize = true;
            lblJudul.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblJudul.Location = new Point(20, 15);
            lblJudul.Name = "lblJudul";
            lblJudul.Size = new Size(205, 25);
            lblJudul.TabIndex = 0;
            lblJudul.Text = "Pengaturan Database";
            //
            // lblInfo
            //
            lblInfo.ForeColor = Color.DimGray;
            lblInfo.Location = new Point(22, 45);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(360, 34);
            lblInfo.TabIndex = 1;
            lblInfo.Text = "Isi data server PostgreSQL. Database dan tabel akan dibuat otomatis jika belum ada.";
            //
            // lblHost
            //
            lblHost.AutoSize = true;
            lblHost.Location = new Point(22, 93);
            lblHost.Name = "lblHost";
            lblHost.Size = new Size(38, 15);
            lblHost.TabIndex = 2;
            lblHost.Text = "Host :";
            //
            // tbHost
            //
            tbHost.Location = new Point(130, 90);
            tbHost.Name = "tbHost";
            tbHost.Size = new Size(250, 23);
            tbHost.TabIndex = 3;
            tbHost.Text = "localhost";
            //
            // lblPort
            //
            lblPort.AutoSize = true;
            lblPort.Location = new Point(22, 125);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(35, 15);
            lblPort.TabIndex = 4;
            lblPort.Text = "Port :";
            //
            // tbPort
            //
            tbPort.Location = new Point(130, 122);
            tbPort.Name = "tbPort";
            tbPort.Size = new Size(80, 23);
            tbPort.TabIndex = 5;
            tbPort.Text = "5432";
            //
            // lblDatabase
            //
            lblDatabase.AutoSize = true;
            lblDatabase.Location = new Point(22, 157);
            lblDatabase.Name = "lblDatabase";
            lblDatabase.Size = new Size(61, 15);
            lblDatabase.TabIndex = 6;
            lblDatabase.Text = "Database :";
            //
            // tbDatabase
            //
            tbDatabase.Location = new Point(130, 154);
            tbDatabase.Name = "tbDatabase";
            tbDatabase.Size = new Size(250, 23);
            tbDatabase.TabIndex = 7;
            tbDatabase.Text = "swasana_db";
            //
            // lblUsername
            //
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(22, 189);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(66, 15);
            lblUsername.TabIndex = 8;
            lblUsername.Text = "Username :";
            //
            // tbUsername
            //
            tbUsername.Location = new Point(130, 186);
            tbUsername.Name = "tbUsername";
            tbUsername.Size = new Size(250, 23);
            tbUsername.TabIndex = 9;
            tbUsername.Text = "postgres";
            //
            // lblPassword
            //
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(22, 221);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(63, 15);
            lblPassword.TabIndex = 10;
            lblPassword.Text = "Password :";
            //
            // tbPassword
            //
            tbPassword.Location = new Point(130, 218);
            tbPassword.Name = "tbPassword";
            tbPassword.Size = new Size(250, 23);
            tbPassword.TabIndex = 11;
            tbPassword.UseSystemPasswordChar = true;
            //
            // btnSimpan
            //
            btnSimpan.Location = new Point(130, 260);
            btnSimpan.Name = "btnSimpan";
            btnSimpan.Size = new Size(140, 32);
            btnSimpan.TabIndex = 12;
            btnSimpan.Text = "Tes && Simpan";
            btnSimpan.UseVisualStyleBackColor = true;
            btnSimpan.Click += btnSimpan_Click;
            //
            // btnBatal
            //
            btnBatal.Location = new Point(280, 260);
            btnBatal.Name = "btnBatal";
            btnBatal.Size = new Size(100, 32);
            btnBatal.TabIndex = 13;
            btnBatal.Text = "Batal";
            btnBatal.UseVisualStyleBackColor = true;
            btnBatal.Click += btnBatal_Click;
            //
            // FormKoneksi
            //
            AcceptButton = btnSimpan;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(404, 312);
            Controls.Add(btnBatal);
            Controls.Add(btnSimpan);
            Controls.Add(tbPassword);
            Controls.Add(lblPassword);
            Controls.Add(tbUsername);
            Controls.Add(lblUsername);
            Controls.Add(tbDatabase);
            Controls.Add(lblDatabase);
            Controls.Add(tbPort);
            Controls.Add(lblPort);
            Controls.Add(tbHost);
            Controls.Add(lblHost);
            Controls.Add(lblInfo);
            Controls.Add(lblJudul);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormKoneksi";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SWASANA - Pengaturan Database";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblJudul;
        private Label lblInfo;
        private Label lblHost;
        private TextBox tbHost;
        private Label lblPort;
        private TextBox tbPort;
        private Label lblDatabase;
        private TextBox tbDatabase;
        private Label lblUsername;
        private TextBox tbUsername;
        private Label lblPassword;
        private TextBox tbPassword;
        private Button btnSimpan;
        private Button btnBatal;
    }
}
