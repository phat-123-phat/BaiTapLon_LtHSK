
namespace Bài_Tập_lớn_LT_HSK
{
    partial class frmHoaDon
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvHoaDon = new System.Windows.Forms.DataGridView();
            this.btnChiTiet = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rboHoaDonNhap = new System.Windows.Forms.RadioButton();
            this.rboHoaDonBan = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDon)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvHoaDon
            // 
            this.dgvHoaDon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHoaDon.Location = new System.Drawing.Point(12, 65);
            this.dgvHoaDon.Name = "dgvHoaDon";
            this.dgvHoaDon.RowHeadersWidth = 51;
            this.dgvHoaDon.RowTemplate.Height = 24;
            this.dgvHoaDon.Size = new System.Drawing.Size(999, 355);
            this.dgvHoaDon.TabIndex = 0;
            this.dgvHoaDon.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHoaDon_CellContentClick);
            this.dgvHoaDon.SelectionChanged += new System.EventHandler(this.dgvHoaDon_SelectionChanged);
            // 
            // btnChiTiet
            // 
            this.btnChiTiet.Location = new System.Drawing.Point(443, 454);
            this.btnChiTiet.Name = "btnChiTiet";
            this.btnChiTiet.Size = new System.Drawing.Size(161, 44);
            this.btnChiTiet.TabIndex = 1;
            this.btnChiTiet.Text = "Chi tiết hóa đơn";
            this.btnChiTiet.UseVisualStyleBackColor = true;
            this.btnChiTiet.Click += new System.EventHandler(this.btnChiTiet_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Danh sách hóa đơn";
            // 
            // rboHoaDonNhap
            // 
            this.rboHoaDonNhap.AutoSize = true;
            this.rboHoaDonNhap.Location = new System.Drawing.Point(856, 34);
            this.rboHoaDonNhap.Name = "rboHoaDonNhap";
            this.rboHoaDonNhap.Size = new System.Drawing.Size(155, 21);
            this.rboHoaDonNhap.TabIndex = 3;
            this.rboHoaDonNhap.TabStop = true;
            this.rboHoaDonNhap.Text = "Hóa đơn nhập hàng";
            this.rboHoaDonNhap.UseVisualStyleBackColor = true;
            // 
            // rboHoaDonBan
            // 
            this.rboHoaDonBan.AutoSize = true;
            this.rboHoaDonBan.Location = new System.Drawing.Point(703, 36);
            this.rboHoaDonBan.Name = "rboHoaDonBan";
            this.rboHoaDonBan.Size = new System.Drawing.Size(147, 21);
            this.rboHoaDonBan.TabIndex = 4;
            this.rboHoaDonBan.TabStop = true;
            this.rboHoaDonBan.Text = "Hóa đơn bán hàng";
            this.rboHoaDonBan.UseVisualStyleBackColor = true;
            // 
            // frmHoaDon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1023, 541);
            this.Controls.Add(this.rboHoaDonBan);
            this.Controls.Add(this.rboHoaDonNhap);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnChiTiet);
            this.Controls.Add(this.dgvHoaDon);
            this.Name = "frmHoaDon";
            this.Text = "Hóa Đơn";
            this.Load += new System.EventHandler(this.frmHoaDon_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvHoaDon;
        private System.Windows.Forms.Button btnChiTiet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rboHoaDonNhap;
        private System.Windows.Forms.RadioButton rboHoaDonBan;
    }
}