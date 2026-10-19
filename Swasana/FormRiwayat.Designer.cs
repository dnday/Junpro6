namespace Swasana
{
    partial class FormRiwayat
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
            lblCari = new Label();
            tbCari = new TextBox();
            btnCari = new Button();
            dgvRiwayat = new DataGridView();
            colTanggal = new DataGridViewTextBoxColumn();
            colLokasi = new DataGridViewTextBoxColumn();
            colSuhu = new DataGridViewTextBoxColumn();
            colHujan = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            btnKembali = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvRiwayat).BeginInit();
            SuspendLayout();
            //
            // lblJudul
            //
            lblJudul.AutoSize = true;
            lblJudul.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblJudul.Location = new Point(20, 15);
            lblJudul.Name = "lblJudul";
            lblJudul.Size = new Size(170, 30);
            lblJudul.TabIndex = 0;
            lblJudul.Text = "Riwayat Cuaca";
            //
            // lblCari
            //
            lblCari.AutoSize = true;
            lblCari.Location = new Point(22, 63);
            lblCari.Name = "lblCari";
            lblCari.Size = new Size(76, 15);
            lblCari.TabIndex = 1;
            lblCari.Text = "Cari lokasi :";
            //
            // tbCari
            //
            tbCari.Location = new Point(110, 60);
            tbCari.Name = "tbCari";
            tbCari.Size = new Size(250, 23);
            tbCari.TabIndex = 2;
            //
            // btnCari
            //
            btnCari.Location = new Point(370, 59);
            btnCari.Name = "btnCari";
            btnCari.Size = new Size(80, 25);
            btnCari.TabIndex = 3;
            btnCari.Text = "Cari";
            btnCari.UseVisualStyleBackColor = true;
            btnCari.Click += btnCari_Click;
            //
            // dgvRiwayat
            //
            dgvRiwayat.AllowUserToAddRows = false;
            dgvRiwayat.AllowUserToDeleteRows = false;
            dgvRiwayat.BackgroundColor = Color.White;
            dgvRiwayat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRiwayat.Columns.AddRange(new DataGridViewColumn[] { colTanggal, colLokasi, colSuhu, colHujan, colStatus });
            dgvRiwayat.Location = new Point(22, 98);
            dgvRiwayat.Name = "dgvRiwayat";
            dgvRiwayat.ReadOnly = true;
            dgvRiwayat.RowHeadersVisible = false;
            dgvRiwayat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRiwayat.Size = new Size(560, 250);
            dgvRiwayat.TabIndex = 4;
            //
            // colTanggal
            //
            colTanggal.HeaderText = "Tanggal";
            colTanggal.Name = "colTanggal";
            colTanggal.ReadOnly = true;
            //
            // colLokasi
            //
            colLokasi.HeaderText = "Lokasi";
            colLokasi.Name = "colLokasi";
            colLokasi.ReadOnly = true;
            //
            // colSuhu
            //
            colSuhu.HeaderText = "Suhu (°C)";
            colSuhu.Name = "colSuhu";
            colSuhu.ReadOnly = true;
            //
            // colHujan
            //
            colHujan.HeaderText = "Hujan (mm/jam)";
            colHujan.Name = "colHujan";
            colHujan.ReadOnly = true;
            //
            // colStatus
            //
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            //
            // btnKembali
            //
            btnKembali.Location = new Point(22, 362);
            btnKembali.Name = "btnKembali";
            btnKembali.Size = new Size(100, 30);
            btnKembali.TabIndex = 5;
            btnKembali.Text = "Kembali";
            btnKembali.UseVisualStyleBackColor = true;
            btnKembali.Click += btnKembali_Click;
            //
            // FormRiwayat
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(604, 407);
            Controls.Add(btnKembali);
            Controls.Add(dgvRiwayat);
            Controls.Add(btnCari);
            Controls.Add(tbCari);
            Controls.Add(lblCari);
            Controls.Add(lblJudul);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormRiwayat";
            StartPosition = FormStartPosition.CenterParent;
            Text = "SWASANA - Riwayat Cuaca";
            Load += FormRiwayat_Load;
            ((System.ComponentModel.ISupportInitialize)dgvRiwayat).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblJudul;
        private Label lblCari;
        private TextBox tbCari;
        private Button btnCari;
        private DataGridView dgvRiwayat;
        private DataGridViewTextBoxColumn colTanggal;
        private DataGridViewTextBoxColumn colLokasi;
        private DataGridViewTextBoxColumn colSuhu;
        private DataGridViewTextBoxColumn colHujan;
        private DataGridViewTextBoxColumn colStatus;
        private Button btnKembali;
    }
}
