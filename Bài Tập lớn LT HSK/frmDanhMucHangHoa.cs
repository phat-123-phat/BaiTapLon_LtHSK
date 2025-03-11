using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bài_Tập_lớn_LT_HSK
{
    public partial class frmDanhMucHangHoa : Form
    {
        // Kiểu liệt kê để lấy trạng thái của hệ thống
        private enum TrangThai { None, Them, Sua, XOA }
        // Ban đầu là None
        private static TrangThai trangThaiHienTai = TrangThai.None;


        string connString = ConfigurationManager.ConnectionStrings["db_btlQuanLiBanHang"].ConnectionString;
        public frmDanhMucHangHoa()
        {
            InitializeComponent();
        }
        private void frmDanhMucHangHoa_Load(object sender, EventArgs e)
        {
            LoadDanhMuc();
            ChuyenSangNhapLieu(false);
        }
        private void LoadDanhMuc()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT * FROM tblNhomhang";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cboDanhMuc.DataSource = dt;
                dgvHangHoa.Columns["iMaHang"].Visible = false;
                dgvHangHoa.Columns["iMaNhom"].Visible = false;
                setNameColum();
                cboDanhMuc.DisplayMember = "sTenNhom";
                cboDanhMuc.ValueMember = "iMaNhom";
            }
        }

        private void LoadHangHoa(int maNhom)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlDataAdapter da = new SqlDataAdapter("sp_LayHangHoaTheoNhom", conn);

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@MaNhom", maNhom);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvHangHoa.DataSource = dt;
            }
        }

        private void cboDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHangHoa(GetIdNhomHang());
        }

        private int GetIdNhomHang()
        {
            int maNhom = 0;
            if (cboDanhMuc.SelectedItem is DataRowView row)
            {
                maNhom = Convert.ToInt32(row["iMaNhom"]);
            }
            return maNhom;
        }


        private void setNameColum()
        {
            dgvHangHoa.Columns["sTenHang"].HeaderText = "Tên Hàng";
            dgvHangHoa.Columns["sMauSac"].HeaderText = "Màu Sắc";
            dgvHangHoa.Columns["sKichThuoc"].HeaderText = "Kích Thước";
            dgvHangHoa.Columns["sDacTinhKT"].HeaderText = "Đặc Tính Kỹ Thuật";
        }

        private void btnTruyVan_Click(object sender, EventArgs e)
        {
            string tenNhom = cboDanhMuc.Text.Trim();
            if (string.IsNullOrEmpty(tenNhom))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục cần tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT iMaNhom FROM tblNhomhang WHERE sTenNhom LIKE @TenNhom";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenNhom", "%" + tenNhom + "%"); // Tìm gần đúng
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        int maNhom = Convert.ToInt32(result);
                        LoadHangHoa(maNhom);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy danh mục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            trangThaiHienTai = TrangThai.Them;
            ChuyenSangNhapLieu(true);
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            trangThaiHienTai = TrangThai.Sua;
            ChuyenSangNhapLieu(true);
        }

        private void ChuyenSangNhapLieu(bool b)
        {
            txtDanhMuc.Visible = b;
            txtDanhMuc.Enabled = b;
            txtDanhMuc.Text = "";
            btnLuu.Enabled = b;
            btnHuy.Enabled = b;
            lblSuaThanh.Text = "";
            lblErorr.Text = "";
            if (b)
            {
                if (trangThaiHienTai == TrangThai.Them)
                {
                    lblSuaThanh.Text = "Nhập danh mục :";
                    txtDanhMuc.Focus();
                }    
                else if(trangThaiHienTai == TrangThai.Sua)
                {
                    lblDanhMuc.Text = "Chọn danh mục :";
                    lblSuaThanh.Text = "Sửa thành :";
                    cboDanhMuc.DroppedDown = true;
                    txtDanhMuc.Focus();
                    if (cboDanhMuc.SelectedValue != null)
                    {
                        txtDanhMuc.Focus();
                    }    
                }  else if(trangThaiHienTai == TrangThai.XOA)
                {
                    lblDanhMuc.Text = "Chọn xóa :";
                    cboDanhMuc.DroppedDown = true;
                    txtDanhMuc.Visible = false;
                    txtDanhMuc.Enabled = false;
                    txtDanhMuc.Text = "";
                }    
            }
            else
            {
                lblDanhMuc.Text = "Tên danh mục :";
            }    
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            lblErorr.Text = "";
            string tenNhom = txtDanhMuc.Text.Trim();
            if (string.IsNullOrEmpty(tenNhom) && trangThaiHienTai != TrangThai.XOA)
            {
                lblErorr.Text = "không được để trống !!!";
                return;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "";
                SqlCommand cmd = new SqlCommand();

                if (trangThaiHienTai == TrangThai.Them)
                {
                    // Lấy mã nhóm lớn nhất hiện tại
                    string getMaxIdQuery = "SELECT ISNULL(MAX(iMaNhom), 0) + 1 FROM tblNhomhang";
                    SqlCommand cmdMaxId = new SqlCommand(getMaxIdQuery, conn);
                    int newId = (int)cmdMaxId.ExecuteScalar();

                    // Thêm danh mục mới với mã nhóm tự tăng8
                    query = "INSERT INTO tblNhomhang (iMaNhom, sTenNhom) VALUES (@MaNhom, @TenNhom)";
                    cmd.Parameters.AddWithValue("@MaNhom", newId);
                    cmd.Parameters.AddWithValue("@TenNhom", tenNhom);

                }
                else if (trangThaiHienTai == TrangThai.Sua)
                {
                    if (cboDanhMuc.SelectedValue == null)
                    {
                        MessageBox.Show("Vui lòng chọn danh mục cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    string tenMoi = txtDanhMuc.Text.Trim();
                    query = "UPDATE tblNhomhang SET sTenNhom = @TenMoi WHERE iMaNhom = @MaNhom";
                    cmd.Parameters.AddWithValue("@TenMoi", tenMoi);
                    cmd.Parameters.AddWithValue("@MaNhom", cboDanhMuc.SelectedValue);
                }
                else if(trangThaiHienTai == TrangThai.XOA)
                {
                    if (cboDanhMuc.SelectedValue == null)
                    {
                        MessageBox.Show("Vui lòng chọn danh mục cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    int maNhom = Convert.ToInt32(cboDanhMuc.SelectedValue);
                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa danh mục này không?",
                                          "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No) return;

                    string checkQuery = "SELECT COUNT(*) FROM tblHanghoa WHERE iMaNhom = @MaNhom";
                    SqlCommand cmdCheck = new SqlCommand(checkQuery, conn);
                    cmdCheck.Parameters.AddWithValue("@MaNhom", maNhom);
                    int count = (int)cmdCheck.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Không thể xóa! Danh mục này đang chứa hàng hóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    query = "DELETE FROM tblNhomhang WHERE iMaNhom = @MaNhom";
                    cmd.Parameters.AddWithValue("@MaNhom", maNhom);
                    MessageBox.Show("Xóa danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }    

                cmd.CommandText = query;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("Cập nhật danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadDanhMuc(); // Tải lại danh mục
            ChuyenSangNhapLieu(false);
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ChuyenSangNhapLieu(false);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            trangThaiHienTai = TrangThai.XOA;
            ChuyenSangNhapLieu(true);
        }
    }
}
