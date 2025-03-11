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
    public partial class frmQuanLyNhanVien : Form
    {
        private enum TrangThai { None, Them, Sua, XOA }
        private static TrangThai trangThaiHienTai = TrangThai.None;

        string connString = ConfigurationManager.ConnectionStrings["db_btlQuanLiBanHang"].ConnectionString;
        public frmQuanLyNhanVien()
        {
            InitializeComponent();
        }
        private void frmQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            LoadData();
            ChuyenSangNhapLieu(false);
        }

        public void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_LayDanhSachNhanVien", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvDsNhanVien.DataSource = dt;
                    dgvDsNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    // Ẩn cột "Mã nhân viên" và "Mật khẩu"
                    dgvDsNhanVien.Columns["iMaNV"].Visible = false;
                    dgvDsNhanVien.Columns["sMatKhau"].Visible = false;

                    // Đổi tên các cột
                    dgvDsNhanVien.Columns["sHoTen"].HeaderText = "Họ và Tên";
                    dgvDsNhanVien.Columns["sDienThoai"].HeaderText = "Số điện thoại";
                    dgvDsNhanVien.Columns["sCCCD"].HeaderText = "Căn cước công dân";
                    dgvDsNhanVien.Columns["sDiaChi"].HeaderText = "Địa chỉ";
                    dgvDsNhanVien.Columns["bGioiTinh"].HeaderText = "Giới tính";
                    dgvDsNhanVien.Columns["dNgaySinh"].HeaderText = "Ngày sinh";
                    dgvDsNhanVien.Columns["dNgayVaoLam"].HeaderText = "Ngày vào làm";

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải danh sách nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }



        private void btnTim_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_TimNhanVienNangCao", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Kiểm tra rỗng trước khi thêm tham số
                    if (!string.IsNullOrWhiteSpace(txtHoTen.Text))
                        cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());

                    if (!string.IsNullOrWhiteSpace(txtSdt.Text))
                        cmd.Parameters.AddWithValue("@DienThoai", txtSdt.Text.Trim());

                    if (!string.IsNullOrWhiteSpace(txtCCCD.Text))
                        cmd.Parameters.AddWithValue("@CCCD", txtCCCD.Text.Trim());

                    if (!string.IsNullOrWhiteSpace(txtDiaChi.Text))
                        cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text.Trim());

                    // Xác định giới tính (0 = Nữ, 1 = Nam)
                    if (rdoNam.Checked)
                        cmd.Parameters.AddWithValue("@GioiTinh", 1);
                    else if (rdoNu.Checked)
                        cmd.Parameters.AddWithValue("@GioiTinh", 0);

                    // Xử lý ngày sinh
                    if (mtbNgaySinh.MaskCompleted)
                    {
                        if (DateTime.TryParseExact(mtbNgaySinh.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime ngaySinh))
                        {
                            cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                        }
                        else
                        {
                            MessageBox.Show("Ngày sinh không hợp lệ. Vui lòng nhập lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Xử lý ngày vào làm
                    if (mtbNgayVaoLam.MaskCompleted)
                    {
                        if (DateTime.TryParseExact(mtbNgayVaoLam.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime ngayVaoLam))
                        {
                            cmd.Parameters.AddWithValue("@NgayVaoLam", ngayVaoLam);
                        }
                        else
                        {
                            MessageBox.Show("Ngày vào làm không hợp lệ. Vui lòng nhập lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Thực thi lệnh tìm kiếm
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvDsNhanVien.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            trangThaiHienTai = TrangThai.Them;
            ChuyenSangNhapLieu(true);

        }

        private void ChuyenSangNhapLieu(bool b)
        {
            btnHuy.Enabled = b;
            btnLuu.Enabled = b;

            if (trangThaiHienTai == TrangThai.Them)
            {
                lblTrangThai.Text = "Nhập dữ liệu Nhân viên mới :";
            }  else if(trangThaiHienTai == TrangThai.Sua)
            {
                lblTrangThai.Text = "Chọn Nhân viên cần sửa :";
            } else if(trangThaiHienTai == TrangThai.XOA)
            {
                lblTrangThai.Text = "Chọn Nhân viên cần xóa :";
            }   else
            {
                lblTrangThai.Text = "Nhập thông tin nhân viên :";
            }    
                
        }
        private int LayMaNhanVienMoi()
        {
            int maNV = 1; // Mặc định nếu không có nhân viên nào

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(iMaNV), 0) + 1 FROM tblNhanvien", conn);
                    maNV = (int)cmd.ExecuteScalar(); // Lấy giá trị từ SQL
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy mã nhân viên mới: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return maNV;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = trangThaiHienTai == TrangThai.Them ? "sp_ThemNhanVien" : "sp_CapNhatNhanVien";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Lấy dữ liệu
                    string hoTen = txtHoTen.Text.Trim();
                    string dienThoai = txtSdt.Text.Trim();
                    string cccd = txtCCCD.Text.Trim();
                    string diaChi = txtDiaChi.Text.Trim();
                    string matKhau = "123456";
                    int gioiTinh = rdoNam.Checked ? 1 : 0;

                    // Kiểm tra ràng buộc dữ liệu
                    if (string.IsNullOrWhiteSpace(hoTen) || string.IsNullOrWhiteSpace(dienThoai) || string.IsNullOrWhiteSpace(cccd))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!dienThoai.All(char.IsDigit) || dienThoai.Length < 10 || dienThoai.Length > 11)
                    {
                        MessageBox.Show("Số điện thoại không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!cccd.All(char.IsDigit) || cccd.Length != 12)
                    {
                        MessageBox.Show("CCCD phải có 12 số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!DateTime.TryParseExact(mtbNgaySinh.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime ngaySinh))
                    {
                        MessageBox.Show("Ngày sinh không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!DateTime.TryParseExact(mtbNgayVaoLam.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime ngayVaoLam))
                    {
                        MessageBox.Show("Ngày vào làm không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Gán tham số cho Stored Procedure
                    if (trangThaiHienTai == TrangThai.Them)
                    {
                        int maNV = LayMaNhanVienMoi();
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        cmd.Parameters.AddWithValue("@MatKhau", matKhau);
                    }
                    else if (trangThaiHienTai == TrangThai.Sua)
                    {
                        int maNV = Convert.ToInt32(dgvDsNhanVien.SelectedRows[0].Cells["iMaNV"].Value);
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                    }

                    cmd.Parameters.AddWithValue("@HoTen", hoTen);
                    cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                    cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                    cmd.Parameters.AddWithValue("@CCCD", cccd);
                    cmd.Parameters.AddWithValue("@DienThoai", dienThoai);
                    cmd.Parameters.AddWithValue("@NgayVaoLam", ngayVaoLam);
                    cmd.Parameters.AddWithValue("@DiaChi", diaChi);

                    // Thực thi
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        string msg = trangThaiHienTai == TrangThai.Them ? "Thêm nhân viên thành công!" : "Cập nhật nhân viên thành công!";
                        MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Thao tác thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            ChuyenSangNhapLieu(false);
        }


        private void btnSua_Click(object sender, EventArgs e)
        {
            trangThaiHienTai = TrangThai.Sua;
            ChuyenSangNhapLieu(true);
        }

    
        private void dgvDsNhanVien_SelectionChanged(object sender, EventArgs e)
        {
            if (trangThaiHienTai == TrangThai.Sua || trangThaiHienTai == TrangThai.XOA)
            {
                if (dgvDsNhanVien.SelectedRows.Count > 0)
                {
                    DataGridViewRow row = dgvDsNhanVien.SelectedRows[0];

                    txtHoTen.Text = row.Cells["sHoTen"].Value.ToString();
                    txtSdt.Text = row.Cells["sDienThoai"].Value.ToString();
                    txtCCCD.Text = row.Cells["sCCCD"].Value.ToString();
                    txtDiaChi.Text = row.Cells["sDiaChi"].Value.ToString();

                    bool gioiTinh = Convert.ToBoolean(row.Cells["bGioiTinh"].Value);
                    rdoNam.Checked = gioiTinh;
                    rdoNu.Checked = !gioiTinh;

                    DateTime ngaySinh = Convert.ToDateTime(row.Cells["dNgaySinh"].Value);
                    mtbNgaySinh.Text = ngaySinh.ToString("dd/MM/yyyy");

                    DateTime ngayVaoLam = Convert.ToDateTime(row.Cells["dNgayVaoLam"].Value);
                    mtbNgayVaoLam.Text = ngayVaoLam.ToString("dd/MM/yyyy");

                }
            }    
            
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            trangThaiHienTai = TrangThai.XOA;
            ChuyenSangNhapLieu(true);




           
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            trangThaiHienTai = TrangThai.None;
            ChuyenSangNhapLieu(false);

            // Xóa nội dung các TextBox
            txtHoTen.Clear();
            txtSdt.Clear();
            txtCCCD.Clear();
            txtDiaChi.Clear();
            mtbNgaySinh.Clear();
            mtbNgayVaoLam.Clear();

            // Bỏ chọn giới tính
            rdoNam.Checked = false;
            rdoNu.Checked = false;

            // Tải lại dữ liệu
            LoadData();
        }
    }
}
