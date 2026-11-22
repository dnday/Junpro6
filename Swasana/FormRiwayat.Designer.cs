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
            DataGridViewCellStyle gayaHeader = new DataGridViewCellStyle();
            DataGridViewCellStyle gayaBaris = new DataGridViewCellStyle();
            pnlHeader = new Panel();
            lblJudul = new Label();
            lblCari = new Label();
            tbCari = new TextBox();
            btnCari = new Button();
            dgvRiwayat = new DataGridView();
            colIkon = new DataGridViewImageColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colTanggal = new DataGridViewTextBoxColumn();
            colLokasi = new DataGridViewTextBoxColumn();
            colSuhu = new DataGridViewTextBoxColumn();
            colHujan = new DataGridViewTextBoxColumn();
            colAngin = new DataGridViewTextBoxColumn();
            grafikTren = new GrafikTren();
            lblKosong = new Label();
            lblJumlah = new Label();
            btnKembali = new Button();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRiwayat).BeginInit();
            SuspendLayout();
            //
            // pnlHeader
            //
            pnlHeader.BackColor = Color.FromArgb(15, 76, 92);
            pnlHeader.Controls.Add(lblJudul);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(720, 60);
            pnlHeader.TabIndex = 0;
            //
            // lblJudul
            //
            lblJudul.AutoSize = true;
            lblJudul.Font = new Font("Bahnschrift SemiBold", 18F);
            lblJudul.ForeColor = Color.White;
            lblJudul.Location = new Point(22, 15);
            lblJudul.Name = "lblJudul";
            lblJudul.Size = new Size(152, 29);
            lblJudul.TabIndex = 0;
            lblJudul.Text = "Riwayat cuaca";
            //
            // lblCari
            //
            lblCari.AutoSize = true;
            lblCari.Font = new Font("Segoe UI Semibold", 9.75F);
            lblCari.ForeColor = Color.FromArgb(28, 43, 54);
            lblCari.Location = new Point(24, 78);
            lblCari.Name = "lblCari";
            lblCari.Size = new Size(76, 17);
            lblCari.TabIndex = 1;
            lblCari.Text = "Cari lokasi";
            //
            // tbCari
            //
            tbCari.BorderStyle = BorderStyle.FixedSingle;
            tbCari.Font = new Font("Segoe UI", 11F);
            tbCari.Location = new Point(24, 99);
            tbCari.Name = "tbCari";
            tbCari.PlaceholderText = "Nama lokasi";
            tbCari.Size = new Size(300, 27);
            tbCari.TabIndex = 2;
            //
            // btnCari
            //
            btnCari.BackColor = Color.FromArgb(15, 76, 92);
            btnCari.Cursor = Cursors.Hand;
            btnCari.FlatAppearance.BorderSize = 0;
            btnCari.FlatStyle = FlatStyle.Flat;
            btnCari.Font = new Font("Segoe UI Semibold", 10F);
            btnCari.ForeColor = Color.White;
            btnCari.Location = new Point(334, 98);
            btnCari.Name = "btnCari";
            btnCari.Size = new Size(90, 29);
            btnCari.TabIndex = 3;
            btnCari.Text = "Cari";
            btnCari.UseVisualStyleBackColor = false;
            btnCari.Click += btnCari_Click;
            //
            // dgvRiwayat
            //
            dgvRiwayat.AllowUserToAddRows = false;
            dgvRiwayat.AllowUserToDeleteRows = false;
            dgvRiwayat.AllowUserToResizeRows = false;
            dgvRiwayat.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvRiwayat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRiwayat.BackgroundColor = Color.White;
            dgvRiwayat.BorderStyle = BorderStyle.FixedSingle;
            dgvRiwayat.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRiwayat.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            gayaHeader.BackColor = Color.FromArgb(227, 235, 239);
            gayaHeader.Font = new Font("Segoe UI Semibold", 9.75F);
            gayaHeader.ForeColor = Color.FromArgb(28, 43, 54);
            gayaHeader.Padding = new Padding(4, 0, 0, 0);
            gayaHeader.SelectionBackColor = Color.FromArgb(227, 235, 239);
            gayaHeader.SelectionForeColor = Color.FromArgb(28, 43, 54);
            dgvRiwayat.ColumnHeadersDefaultCellStyle = gayaHeader;
            dgvRiwayat.ColumnHeadersHeight = 34;
            dgvRiwayat.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvRiwayat.Columns.AddRange(new DataGridViewColumn[] { colIkon, colStatus, colTanggal, colLokasi, colSuhu, colHujan, colAngin });
            gayaBaris.BackColor = Color.White;
            gayaBaris.Font = new Font("Segoe UI", 9.75F);
            gayaBaris.ForeColor = Color.FromArgb(28, 43, 54);
            gayaBaris.Padding = new Padding(4, 0, 0, 0);
            gayaBaris.SelectionBackColor = Color.FromArgb(214, 232, 238);
            gayaBaris.SelectionForeColor = Color.FromArgb(28, 43, 54);
            dgvRiwayat.DefaultCellStyle = gayaBaris;
            dgvRiwayat.EnableHeadersVisualStyles = false;
            dgvRiwayat.GridColor = Color.FromArgb(227, 235, 239);
            dgvRiwayat.Location = new Point(24, 342);
            dgvRiwayat.MultiSelect = false;
            dgvRiwayat.Name = "dgvRiwayat";
            dgvRiwayat.ReadOnly = true;
            dgvRiwayat.RowHeadersVisible = false;
            dgvRiwayat.RowTemplate.Height = 34;
            dgvRiwayat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRiwayat.Size = new Size(672, 238);
            dgvRiwayat.TabIndex = 4;
            //
            // colIkon
            //
            colIkon.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colIkon.HeaderText = "";
            colIkon.ImageLayout = DataGridViewImageCellLayout.Zoom;
            colIkon.Name = "colIkon";
            colIkon.ReadOnly = true;
            colIkon.Width = 36;
            //
            // colStatus
            //
            colStatus.FillWeight = 70F;
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            //
            // colTanggal
            //
            colTanggal.FillWeight = 110F;
            colTanggal.HeaderText = "Waktu";
            colTanggal.Name = "colTanggal";
            colTanggal.ReadOnly = true;
            //
            // colLokasi
            //
            colLokasi.FillWeight = 150F;
            colLokasi.HeaderText = "Lokasi";
            colLokasi.Name = "colLokasi";
            colLokasi.ReadOnly = true;
            //
            // colSuhu
            //
            colSuhu.FillWeight = 70F;
            colSuhu.HeaderText = "Suhu (°C)";
            colSuhu.Name = "colSuhu";
            colSuhu.ReadOnly = true;
            //
            // colHujan
            //
            colHujan.FillWeight = 90F;
            colHujan.HeaderText = "Hujan (mm/jam)";
            colHujan.Name = "colHujan";
            colHujan.ReadOnly = true;
            //
            // colAngin
            //
            colAngin.FillWeight = 90F;
            colAngin.HeaderText = "Angin (km/jam)";
            colAngin.Name = "colAngin";
            colAngin.ReadOnly = true;
            //
            // grafikTren
            //
            grafikTren.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grafikTren.Location = new Point(24, 140);
            grafikTren.Name = "grafikTren";
            grafikTren.Size = new Size(672, 190);
            grafikTren.TabIndex = 8;
            //
            // lblKosong
            //
            lblKosong.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblKosong.BackColor = Color.White;
            lblKosong.Font = new Font("Segoe UI", 9.75F);
            lblKosong.ForeColor = Color.FromArgb(94, 110, 122);
            lblKosong.Location = new Point(25, 392);
            lblKosong.Name = "lblKosong";
            lblKosong.Size = new Size(670, 60);
            lblKosong.TabIndex = 5;
            lblKosong.Text = "Belum ada riwayat. Buka Cek cuaca, hasilnya otomatis tersimpan di sini.";
            lblKosong.TextAlign = ContentAlignment.MiddleCenter;
            lblKosong.Visible = false;
            //
            // lblJumlah
            //
            lblJumlah.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblJumlah.AutoSize = true;
            lblJumlah.Font = new Font("Segoe UI", 9F);
            lblJumlah.ForeColor = Color.FromArgb(94, 110, 122);
            lblJumlah.Location = new Point(24, 601);
            lblJumlah.Name = "lblJumlah";
            lblJumlah.Size = new Size(41, 15);
            lblJumlah.TabIndex = 6;
            lblJumlah.Text = "0 data";
            //
            // btnKembali
            //
            btnKembali.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnKembali.BackColor = Color.White;
            btnKembali.Cursor = Cursors.Hand;
            btnKembali.FlatAppearance.BorderColor = Color.FromArgb(200, 212, 219);
            btnKembali.FlatStyle = FlatStyle.Flat;
            btnKembali.Font = new Font("Segoe UI", 9.75F);
            btnKembali.ForeColor = Color.FromArgb(15, 76, 92);
            btnKembali.Location = new Point(596, 592);
            btnKembali.Name = "btnKembali";
            btnKembali.Size = new Size(100, 32);
            btnKembali.TabIndex = 7;
            btnKembali.Text = "Kembali";
            btnKembali.UseVisualStyleBackColor = false;
            btnKembali.Click += btnKembali_Click;
            //
            // FormRiwayat
            //
            AcceptButton = btnCari;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 248);
            ClientSize = new Size(720, 640);
            Controls.Add(btnKembali);
            Controls.Add(lblJumlah);
            Controls.Add(lblKosong);
            Controls.Add(grafikTren);
            Controls.Add(dgvRiwayat);
            Controls.Add(btnCari);
            Controls.Add(tbCari);
            Controls.Add(lblCari);
            Controls.Add(pnlHeader);
            MinimumSize = new Size(600, 560);
            Name = "FormRiwayat";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SWASANA - Riwayat cuaca";
            Load += FormRiwayat_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRiwayat).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlHeader;
        private Label lblJudul;
        private Label lblCari;
        private TextBox tbCari;
        private Button btnCari;
        private DataGridView dgvRiwayat;
        private DataGridViewImageColumn colIkon;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colTanggal;
        private DataGridViewTextBoxColumn colLokasi;
        private DataGridViewTextBoxColumn colSuhu;
        private DataGridViewTextBoxColumn colHujan;
        private DataGridViewTextBoxColumn colAngin;
        private GrafikTren grafikTren;
        private Label lblKosong;
        private Label lblJumlah;
        private Button btnKembali;
    }
}
