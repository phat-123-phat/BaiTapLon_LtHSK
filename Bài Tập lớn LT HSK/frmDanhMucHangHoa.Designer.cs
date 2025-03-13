
namespace Bài_Tập_lớn_LT_HSK
{
    partial class frmDanhMucHangHoa
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
            this.cboDanhMuc = new System.Windows.Forms.ComboBox();
            this.lblDanhMuc = new System.Windows.Forms.Label();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnTruyVan = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.dgvHangHoa = new System.Windows.Forms.DataGridView();
            this.btnHuy = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.txtDanhMuc = new System.Windows.Forms.TextBox();
            this.lblSuaThanh = new System.Windows.Forms.Label();
            this.lblErorr = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHangHoa)).BeginInit();
            this.SuspendLayout();
            // 
            // cboDanhMuc
            // 
            this.cboDanhMuc.FormattingEnabled = true;
            this.cboDanhMuc.Location = new System.Drawing.Point(201, 12);
            this.cboDanhMuc.Name = "cboDanhMuc";
            this.cboDanhMuc.Size = new System.Drawing.Size(229, 24);
            this.cboDanhMuc.TabIndex = 0;
            this.cboDanhMuc.SelectedIndexChanged += new System.EventHandler(this.cboDanhMuc_SelectedIndexChanged);
            // 
            // lblDanhMuc
            // 
            this.lblDanhMuc.AutoSize = true;
            this.lblDanhMuc.Location = new System.Drawing.Point(72, 19);
            this.lblDanhMuc.Name = "lblDanhMuc";
            this.lblDanhMuc.Size = new System.Drawing.Size(103, 17);
            this.lblDanhMuc.TabIndex = 1;
            this.lblDanhMuc.Text = "Tên danh mục ";
            // 
            // btnThem
            // 
            this.btnThem.Location = new System.Drawing.Point(201, 123);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(75, 23);
            this.btnThem.TabIndex = 2;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnSua
            // 
            this.btnSua.Location = new System.Drawing.Point(334, 123);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(75, 23);
            this.btnSua.TabIndex = 3;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnTruyVan
            // 
            this.btnTruyVan.Location = new System.Drawing.Point(75, 123);
            this.btnTruyVan.Name = "btnTruyVan";
            this.btnTruyVan.Size = new System.Drawing.Size(75, 23);
            this.btnTruyVan.TabIndex = 4;
            this.btnTruyVan.Text = "Truy vấn";
            this.btnTruyVan.UseVisualStyleBackColor = true;
            this.btnTruyVan.Click += new System.EventHandler(this.btnTruyVan_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.Location = new System.Drawing.Point(455, 123);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(75, 23);
            this.btnXoa.TabIndex = 5;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // dgvHangHoa
            // 
            this.dgvHangHoa.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHangHoa.Location = new System.Drawing.Point(32, 161);
            this.dgvHangHoa.Name = "dgvHangHoa";
            this.dgvHangHoa.RowHeadersWidth = 51;
            this.dgvHangHoa.RowTemplate.Height = 24;
            this.dgvHangHoa.Size = new System.Drawing.Size(787, 289);
            this.dgvHangHoa.TabIndex = 6;
            // 
            // btnHuy
            // 
            this.btnHuy.Location = new System.Drawing.Point(455, 42);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(75, 23);
            this.btnHuy.TabIndex = 7;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = true;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.Location = new System.Drawing.Point(455, 13);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(75, 23);
            this.btnLuu.TabIndex = 8;
            this.btnLuu.Text = "Lưu";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // txtDanhMuc
            // 
            this.txtDanhMuc.Location = new System.Drawing.Point(201, 57);
            this.txtDanhMuc.Name = "txtDanhMuc";
            this.txtDanhMuc.Size = new System.Drawing.Size(229, 22);
            this.txtDanhMuc.TabIndex = 9;
            this.txtDanhMuc.TextChanged += new System.EventHandler(this.txtDanhMuc_TextChanged);
            // 
            // lblSuaThanh
            // 
            this.lblSuaThanh.AutoSize = true;
            this.lblSuaThanh.Location = new System.Drawing.Point(75, 62);
            this.lblSuaThanh.Name = "lblSuaThanh";
            this.lblSuaThanh.Size = new System.Drawing.Size(46, 17);
            this.lblSuaThanh.TabIndex = 10;
            this.lblSuaThanh.Text = "label1";
            // 
            // lblErorr
            // 
            this.lblErorr.AutoSize = true;
            this.lblErorr.Location = new System.Drawing.Point(198, 83);
            this.lblErorr.Name = "lblErorr";
            this.lblErorr.Size = new System.Drawing.Size(46, 17);
            this.lblErorr.TabIndex = 11;
            this.lblErorr.Text = "label1";
            // 
            // frmDanhMucHangHoa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 481);
            this.Controls.Add(this.lblErorr);
            this.Controls.Add(this.lblSuaThanh);
            this.Controls.Add(this.txtDanhMuc);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.dgvHangHoa);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.btnTruyVan);
            this.Controls.Add(this.btnSua);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.lblDanhMuc);
            this.Controls.Add(this.cboDanhMuc);
            this.Name = "frmDanhMucHangHoa";
            this.Text = "Danh mục hàng hóa";
            this.Load += new System.EventHandler(this.frmDanhMucHangHoa_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHangHoa)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboDanhMuc;
        private System.Windows.Forms.Label lblDanhMuc;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnTruyVan;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.DataGridView dgvHangHoa;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.TextBox txtDanhMuc;
        private System.Windows.Forms.Label lblSuaThanh;
        private System.Windows.Forms.Label lblErorr;
    }
}