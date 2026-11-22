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
            pnlHeader = new Panel();
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
            lblPesan = new Label();
            btnSimpan = new Button();
            btnBatal = new Button();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            //
            // pnlHeader
            //
            pnlHeader.BackColor = Color.FromArgb(15, 76, 92);
            pnlHeader.Controls.Add(lblJudul);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(420, 60);
            pnlHeader.TabIndex = 0;
            //
            // lblJudul
            //
            lblJudul.AutoSize = true;
            lblJudul.Font = new Font("Bahnschrift SemiBold", 18F);
            lblJudul.ForeColor = Color.White;
            lblJudul.Location = new Point(22, 15);
            lblJudul.Name = "lblJudul";
            lblJudul.Size = new Size(190, 29);
            lblJudul.TabIndex = 0;
            lblJudul.Text = "Koneksi database";
            //
            // lblInfo
            //
            lblInfo.Font = new Font("Segoe UI", 9.75F);
            lblInfo.ForeColor = Color.FromArgb(94, 110, 122);
            lblInfo.Location = new Point(24, 72);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(372, 38);
            lblInfo.TabIndex = 1;
            lblInfo.Text = "PostgreSQL lokal atau online (misalnya Supabase). URI koneksi bisa langsung ditempel ke kolom Host.";
            //
            // lblHost
            //
            lblHost.AutoSize = true;
            lblHost.Font = new Font("Segoe UI Semibold", 9.75F);
            lblHost.ForeColor = Color.FromArgb(28, 43, 54);
            lblHost.Location = new Point(24, 124);
            lblHost.Name = "lblHost";
            lblHost.Size = new Size(34, 17);
            lblHost.TabIndex = 2;
            lblHost.Text = "Host";
            //
            // tbHost
            //
            tbHost.BorderStyle = BorderStyle.FixedSingle;
            tbHost.Font = new Font("Segoe UI", 10F);
            tbHost.Location = new Point(130, 120);
            tbHost.Name = "tbHost";
            tbHost.Size = new Size(266, 25);
            tbHost.TabIndex = 3;
            tbHost.Text = "localhost";
            //
            // lblPort
            //
            lblPort.AutoSize = true;
            lblPort.Font = new Font("Segoe UI Semibold", 9.75F);
            lblPort.ForeColor = Color.FromArgb(28, 43, 54);
            lblPort.Location = new Point(24, 159);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(31, 17);
            lblPort.TabIndex = 4;
            lblPort.Text = "Port";
            //
            // tbPort
            //
            tbPort.BorderStyle = BorderStyle.FixedSingle;
            tbPort.Font = new Font("Segoe UI", 10F);
            tbPort.Location = new Point(130, 155);
            tbPort.Name = "tbPort";
            tbPort.Size = new Size(90, 25);
            tbPort.TabIndex = 5;
            tbPort.Text = "5432";
            //
            // lblDatabase
            //
            lblDatabase.AutoSize = true;
            lblDatabase.Font = new Font("Segoe UI Semibold", 9.75F);
            lblDatabase.ForeColor = Color.FromArgb(28, 43, 54);
            lblDatabase.Location = new Point(24, 194);
            lblDatabase.Name = "lblDatabase";
            lblDatabase.Size = new Size(61, 17);
            lblDatabase.TabIndex = 6;
            lblDatabase.Text = "Database";
            //
            // tbDatabase
            //
            tbDatabase.BorderStyle = BorderStyle.FixedSingle;
            tbDatabase.Font = new Font("Segoe UI", 10F);
            tbDatabase.Location = new Point(130, 190);
            tbDatabase.Name = "tbDatabase";
            tbDatabase.Size = new Size(266, 25);
            tbDatabase.TabIndex = 7;
            tbDatabase.Text = "swasana_db";
            //
            // lblUsername
            //
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI Semibold", 9.75F);
            lblUsername.ForeColor = Color.FromArgb(28, 43, 54);
            lblUsername.Location = new Point(24, 229);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(66, 17);
            lblUsername.TabIndex = 8;
            lblUsername.Text = "Username";
            //
            // tbUsername
            //
            tbUsername.BorderStyle = BorderStyle.FixedSingle;
            tbUsername.Font = new Font("Segoe UI", 10F);
            tbUsername.Location = new Point(130, 225);
            tbUsername.Name = "tbUsername";
            tbUsername.Size = new Size(266, 25);
            tbUsername.TabIndex = 9;
            tbUsername.Text = "postgres";
            //
            // lblPassword
            //
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI Semibold", 9.75F);
            lblPassword.ForeColor = Color.FromArgb(28, 43, 54);
            lblPassword.Location = new Point(24, 264);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(64, 17);
            lblPassword.TabIndex = 10;
            lblPassword.Text = "Password";
            //
            // tbPassword
            //
            tbPassword.BorderStyle = BorderStyle.FixedSingle;
            tbPassword.Font = new Font("Segoe UI", 10F);
            tbPassword.Location = new Point(130, 260);
            tbPassword.Name = "tbPassword";
            tbPassword.Size = new Size(266, 25);
            tbPassword.TabIndex = 11;
            tbPassword.UseSystemPasswordChar = true;
            //
            // lblPesan
            //
            lblPesan.Font = new Font("Segoe UI", 9F);
            lblPesan.ForeColor = Color.FromArgb(198, 40, 40);
            lblPesan.Location = new Point(24, 294);
            lblPesan.Name = "lblPesan";
            lblPesan.Size = new Size(372, 34);
            lblPesan.TabIndex = 12;
            //
            // btnSimpan
            //
            btnSimpan.BackColor = Color.FromArgb(15, 76, 92);
            btnSimpan.Cursor = Cursors.Hand;
            btnSimpan.FlatAppearance.BorderSize = 0;
            btnSimpan.FlatStyle = FlatStyle.Flat;
            btnSimpan.Font = new Font("Segoe UI Semibold", 10F);
            btnSimpan.ForeColor = Color.White;
            btnSimpan.Location = new Point(186, 334);
            btnSimpan.Name = "btnSimpan";
            btnSimpan.Size = new Size(120, 34);
            btnSimpan.TabIndex = 13;
            btnSimpan.Text = "Sambungkan";
            btnSimpan.UseVisualStyleBackColor = false;
            btnSimpan.Click += btnSimpan_Click;
            //
            // btnBatal
            //
            btnBatal.BackColor = Color.White;
            btnBatal.Cursor = Cursors.Hand;
            btnBatal.FlatAppearance.BorderColor = Color.FromArgb(200, 212, 219);
            btnBatal.FlatStyle = FlatStyle.Flat;
            btnBatal.Font = new Font("Segoe UI", 9.75F);
            btnBatal.ForeColor = Color.FromArgb(15, 76, 92);
            btnBatal.Location = new Point(316, 334);
            btnBatal.Name = "btnBatal";
            btnBatal.Size = new Size(80, 34);
            btnBatal.TabIndex = 14;
            btnBatal.Text = "Batal";
            btnBatal.UseVisualStyleBackColor = false;
            btnBatal.Click += btnBatal_Click;
            //
            // FormKoneksi
            //
            AcceptButton = btnSimpan;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 248);
            ClientSize = new Size(420, 388);
            Controls.Add(btnBatal);
            Controls.Add(btnSimpan);
            Controls.Add(lblPesan);
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
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormKoneksi";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SWASANA - Koneksi database";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlHeader;
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
        private Label lblPesan;
        private Button btnSimpan;
        private Button btnBatal;
    }
}
