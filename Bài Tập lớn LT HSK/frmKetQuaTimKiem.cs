using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bài_Tập_lớn_LT_HSK
{
    public partial class frmKetQuaTimKiem : Form
    {
        private DataTable dtKetQua;

        public frmKetQuaTimKiem(DataTable dtKetQua)
        {
            InitializeComponent();
            this.dtKetQua = dtKetQua;
        }

        private void frmKetQuaTimKiem_Load(object sender, EventArgs e)
        {
            dgvKetQua.DataSource = dtKetQua;
            dgvKetQua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Ẩn cột "Mã nhân viên" và "Mật khẩu"
            dgvKetQua.Columns["iMaNV"].Visible = false;
            dgvKetQua.Columns["sMatKhau"].Visible = false;

            // Đổi tên các cột
            dgvKetQua.Columns["sHoTen"].HeaderText = "Họ và Tên";
            dgvKetQua.Columns["sDienThoai"].HeaderText = "Số điện thoại";
            dgvKetQua.Columns["sCCCD"].HeaderText = "Căn cước công dân";
            dgvKetQua.Columns["sDiaChi"].HeaderText = "Địa chỉ";
            dgvKetQua.Columns["bGioiTinh"].HeaderText = "Giới tính";
            dgvKetQua.Columns["dNgaySinh"].HeaderText = "Ngày sinh";
            dgvKetQua.Columns["dNgayVaoLam"].HeaderText = "Ngày vào làm";
        }
    }
}
