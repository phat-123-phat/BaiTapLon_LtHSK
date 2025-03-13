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


        string connString = ConfigurationManager.ConnectionStrings["db_btlQuanLiBanHang"]?.ConnectionString;
        public frmDanhMucHangHoa()
        {
            InitializeComponent();
        }
        private void frmDanhMucHangHoa_Load(object sender, EventArgs e)
        {
            LoadDanhMuc();
            ChuyenSangNhapLieu(false);
            btnLuu.Enabled = false;
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
            KiemTraDieuKienLuu();
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
                btnTruyVan.Enabled = false;
            }else
            {
                btnTruyVan.Enabled = true;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_TimMaNhom", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TenNhom", tenNhom);

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
                btnLuu.Enabled = false;
            }
            else
            {
                btnLuu.Enabled = true;
            }    

            int maNhomMoi = 0; // Lưu mã danh mục vừa thêm hoặc sửa

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (trangThaiHienTai == TrangThai.Them)
                    {
                        cmd.CommandText = "sp_ThemDanhMuc";
                        cmd.Parameters.AddWithValue("@TenNhom", tenNhom);

                        // Tham số OUTPUT để lấy mã nhóm mới
                        SqlParameter outputParam = new SqlParameter("@MaNhomMoi", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        cmd.ExecuteNonQuery();
                        maNhomMoi = (int)outputParam.Value;
                    }
                    else if (trangThaiHienTai == TrangThai.Sua)
                    {
                        if (cboDanhMuc.SelectedValue == null)
                        {
                            btnLuu.Enabled = false;
                        }
                        else
                        {
                            btnLuu.Enabled = true;
                        }    
                        maNhomMoi = Convert.ToInt32(cboDanhMuc.SelectedValue);
                        string tenMoi = txtDanhMuc.Text.Trim();

                        cmd.CommandText = "sp_SuaDanhMuc";
                        cmd.Parameters.AddWithValue("@MaNhom", maNhomMoi);
                        cmd.Parameters.AddWithValue("@TenMoi", tenMoi);

                        cmd.ExecuteNonQuery();
                    }
                }
            }


            // Tải lại danh mục
            LoadDanhMuc();

            // Chọn danh mục vừa thêm hoặc sửa
            if (maNhomMoi > 0)
            {
                cboDanhMuc.SelectedValue = maNhomMoi;
            }

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

        private void KiemTraDieuKienLuu()
        {
            if (trangThaiHienTai == TrangThai.Them)
            {
                btnLuu.Enabled = !string.IsNullOrWhiteSpace(txtDanhMuc.Text);
            }
            else if (trangThaiHienTai == TrangThai.Sua)
            {
                btnLuu.Enabled = cboDanhMuc.SelectedValue != null && !string.IsNullOrWhiteSpace(txtDanhMuc.Text);
            }
            else if (trangThaiHienTai == TrangThai.XOA)
            {
                btnLuu.Enabled = cboDanhMuc.SelectedValue != null;
            }
            else
            {
                btnLuu.Enabled = false;
            }
        }



        private void txtDanhMuc_TextChanged(object sender, EventArgs e)
        {
            KiemTraDieuKienLuu();
        }
    }
}
