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

        string connString = ConfigurationManager.ConnectionStrings["db_btlQuanLiBanHang"]?.ConnectionString;
        public frmQuanLyNhanVien()
        {
            InitializeComponent();
        }
        private void frmQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            LoadData();
            ChuyenSangNhapLieu(false);
            btnTim.Enabled = false;
            lblThongBao.Visible = false;

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
                    GhiLog("Lỗi khi tải danh sách nhân viên: " + ex.Message);
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
                        
                    }

                    // Xử lý ngày vào làm
                    if (mtbNgayVaoLam.MaskCompleted)
                    {
                        if (DateTime.TryParseExact(mtbNgayVaoLam.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime ngayVaoLam))
                        {
                            cmd.Parameters.AddWithValue("@NgayVaoLam", ngayVaoLam);
                        }
                        
                    }

                    // Thực thi lệnh tìm kiếm
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvDsNhanVien.DataSource = dt;

                    frmKetQuaTimKiem frmKetQua = new frmKetQuaTimKiem(dt);
                    frmKetQua.ShowDialog();


                }
                catch (Exception ex)
                {
                   GhiLog("Lỗi khi tìm kiếm nhân viên: " + ex.Message);
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            LoadData();
            Refesh();
            trangThaiHienTai = TrangThai.Them;
            ChuyenSangNhapLieu(true);
            btnLuu.Enabled = false;
            

        }

        private void ChuyenSangNhapLieu(bool b)
        {
            btnHuy.Enabled = b;
            btnLuu.Enabled = b;

            if (trangThaiHienTai == TrangThai.Them)
            {
                lblTrangThai.Text = "Nhập dữ liệu Nhân viên mới :";
                btnLuu.Text = "Xác nhận thêm";
            }  else if(trangThaiHienTai == TrangThai.Sua)
            {
                lblTrangThai.Text = "Chọn Nhân viên cần sửa :";
                btnLuu.Text = "Xác nhận sửa";
            } else if(trangThaiHienTai == TrangThai.XOA)
            {
                lblTrangThai.Text = "Chọn Nhân viên cần xóa :";
                btnLuu.Text = "Xác nhận xóa";
            }   else
            {
                lblTrangThai.Text = "Nhập thông tin nhân viên :";
            }    
                
        }
        private int LayMaNhanVienMoi()
        {
            int maNV = 1; 

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(iMaNV), 0) + 1 FROM tblNhanvien", conn);
                    maNV = (int)cmd.ExecuteScalar(); 
                }
                catch (Exception ex)
                {
                    GhiLog("Lỗi khi lấy mã nhân viên mới: " + ex.Message);
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
                    string query = "";

                    if (trangThaiHienTai == TrangThai.Them)
                        query = "sp_ThemNhanVien";
                    else if (trangThaiHienTai == TrangThai.Sua)
                        query = "sp_CapNhatNhanVien";
                    else if (trangThaiHienTai == TrangThai.XOA)
                        query = "sp_XoaNhanVienKhongLienKet";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        int maNV = 0;

                        if (trangThaiHienTai == TrangThai.Them)
                        {
                            // Lấy mã nhân viên mới
                            maNV = LayMaNhanVienMoi();
                            cmd.Parameters.AddWithValue("@MaNV", maNV);
                            cmd.Parameters.AddWithValue("@MatKhau", "123456");
                        }
                        else
                        {
    
                            maNV = Convert.ToInt32(dgvDsNhanVien.SelectedRows[0].Cells["iMaNV"].Value);
                            cmd.Parameters.AddWithValue("@MaNV", maNV);
                        }

                        if (trangThaiHienTai == TrangThai.XOA)
                        {
                           
                                int result = cmd.ExecuteNonQuery();
                            if (result > 0)
                            {
                                LoadData();
                            }
                                bool nhanVienConTonTai = dgvDsNhanVien.Rows
                                                                        .Cast<DataGridViewRow>()
                                                .Any(row => Convert.ToInt32(row.Cells["iMaNV"].Value) == maNV);

                                if (nhanVienConTonTai)
                                {
                                    lblThongBao.Text = "Nhân viên còn tồn tại trong hóa đơn !!!";
                                    lblThongBao.Visible = true;
                                }
                                else
                                {
                                    lblThongBao.Text = "Đã xóa nhân viên !";
                                    lblThongBao.Visible = true;
                                }    
 
                        }
                        else
                        {
                            // Lấy dữ liệu từ form
                            string hoTen = txtHoTen.Text.Trim();
                            string dienThoai = txtSdt.Text.Trim();
                            string cccd = txtCCCD.Text.Trim();
                            string diaChi = txtDiaChi.Text.Trim();
                            int gioiTinh = rdoNam.Checked ? 1 : 0;

                            // Kiểm tra ngày hợp lệ
                            if (!DateTime.TryParseExact(mtbNgaySinh.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime ngaySinh))
                            {
                                
                               
                            }
                            if (!DateTime.TryParseExact(mtbNgayVaoLam.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime ngayVaoLam))
                            {
                                
                                
                            }

                            cmd.Parameters.AddWithValue("@HoTen", hoTen);
                            cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                            cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                            cmd.Parameters.AddWithValue("@CCCD", cccd);
                            cmd.Parameters.AddWithValue("@DienThoai", dienThoai);
                            cmd.Parameters.AddWithValue("@NgayVaoLam", ngayVaoLam);
                            cmd.Parameters.AddWithValue("@DiaChi", diaChi);

                            int result = cmd.ExecuteNonQuery();
                            if (result > 0)
                            {
                                LoadData();
                                ChonNhanVienTrongDanhSach(maNV);
                            }
                           
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                    GhiLog("Lỗi: " + ex.Message);
                }
            }
            ChuyenSangNhapLieu(false);
           
        }



        private void ChonNhanVienTrongDanhSach(int maNV)
        {
            foreach (DataGridViewRow row in dgvDsNhanVien.Rows)
            {
                if (Convert.ToInt32(row.Cells["iMaNV"].Value) == maNV)
                {
                    row.Selected = true;
                    dgvDsNhanVien.CurrentCell = row.Cells[1]; // Chọn ô đầu tiên để tránh lỗi
                    break;
                }
            }
        }



        private void btnSua_Click(object sender, EventArgs e)
        {
            LoadData();
            Refesh();
            trangThaiHienTai = TrangThai.Sua;
            ChuyenSangNhapLieu(true);
            btnLuu.Enabled = false;
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

                    btnLuu.Enabled = true;
                }
            }
            else
            {
                btnLuu.Enabled = false;
            } 
                
            
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            LoadData();
            Refesh();
            trangThaiHienTai = TrangThai.XOA;
            ChuyenSangNhapLieu(true);
            btnLuu.Enabled = false;
            if (dgvDsNhanVien.SelectedRows.Count > 0)
            {
                btnLuu.Enabled = true;
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
           
            Refesh();
        }

        private void Refesh()
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

            btnLuu.Text = "Lưu";
            // Tải lại dữ liệu
            LoadData();
            lblThongBao.Visible = false;
        }

        private void GhiLog(string noiDung)
        {
            string connString = ConfigurationManager.ConnectionStrings["db_btlQuanLiBanHang"]?.ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_GhiLog", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NoiDung", noiDung);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Refesh();
                    System.IO.File.AppendAllText("error_log.txt", DateTime.Now + ": " + ex.Message + Environment.NewLine);
                }
            }
        }
        private void KiemTraDuLieu()
        {
            bool hopLe = true;

            if (string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(txtSdt.Text) ||
                string.IsNullOrWhiteSpace(txtCCCD.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text) ||
                !mtbNgaySinh.MaskCompleted ||
                !mtbNgayVaoLam.MaskCompleted ||
                (!rdoNam.Checked && !rdoNu.Checked))
            {
                hopLe = false;
            }

            // Kiểm tra định dạng số điện thoại
            if (!txtSdt.Text.All(char.IsDigit) || txtSdt.Text.Length < 10 || txtSdt.Text.Length > 11)
            {
                hopLe = false;
            }

            // Kiểm tra định dạng CCCD
            if (!txtCCCD.Text.All(char.IsDigit) || txtCCCD.Text.Length != 12)
            {
                hopLe = false;
            }

            // Kiểm tra ngày hợp lệ
            if (!DateTime.TryParseExact(mtbNgaySinh.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out _) ||
                !DateTime.TryParseExact(mtbNgayVaoLam.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out _))
            {
                hopLe = false;
            }

            btnLuu.Enabled = hopLe;
            
        }

        private void KiemTraTimKiem()
        {
             bool b= !string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                             !string.IsNullOrWhiteSpace(txtSdt.Text) ||
                             !string.IsNullOrWhiteSpace(txtCCCD.Text) ||
                             !string.IsNullOrWhiteSpace(txtDiaChi.Text) ||
                             mtbNgaySinh.MaskCompleted ||
                             mtbNgayVaoLam.MaskCompleted ||
                             (rdoNam.Checked || rdoNu.Checked);
            btnTim.Enabled = b;
            btnHuy.Enabled = b;
        }


        private void txtHoTen_TextChanged(object sender, EventArgs e) { KiemTraDuLieu(); KiemTraTimKiem(); }
        private void txtSdt_TextChanged(object sender, EventArgs e) { KiemTraDuLieu(); KiemTraTimKiem(); }
        private void txtCCCD_TextChanged(object sender, EventArgs e) { KiemTraDuLieu(); KiemTraTimKiem(); }
        private void txtDiaChi_TextChanged(object sender, EventArgs e) { KiemTraDuLieu(); KiemTraTimKiem(); }
        private void mtbNgaySinh_TextChanged(object sender, EventArgs e) { KiemTraDuLieu(); KiemTraTimKiem(); }
        private void mtbNgayVaoLam_TextChanged(object sender, EventArgs e) { KiemTraDuLieu(); KiemTraTimKiem(); }
        private void rdoNam_CheckedChanged(object sender, EventArgs e) { KiemTraDuLieu(); KiemTraTimKiem(); }
        private void rdoNu_CheckedChanged(object sender, EventArgs e) { KiemTraDuLieu(); KiemTraTimKiem(); }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable data = null;
            if (dgvDsNhanVien.DataSource != null)
            {
                 data = (DataTable)dgvDsNhanVien.DataSource;
             
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            string hoTen = txtHoTen.Text.Trim();
            string dienThoai = txtSdt.Text.Trim();
            string cccd = txtCCCD.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            bool? gioiTinh = rdoNam.Checked ? true : (rdoNu.Checked ? false : (bool?)null);
            DateTime? ngaySinh = ConvertMaskedTextBoxToDate(mtbNgaySinh.Text);
            DateTime? ngayVaoLam = ConvertMaskedTextBoxToDate(mtbNgayVaoLam.Text);

            // Mở Form báo cáo và truyền các giá trị tìm kiếm
            frmInBaoCao frm = new frmInBaoCao(hoTen, dienThoai, cccd, diaChi, gioiTinh, ngaySinh, ngayVaoLam, data);
            frm.ShowDialog();
        }
        private DateTime? ConvertMaskedTextBoxToDate(string input)
        {
            DateTime dateValue;
            if (DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dateValue))
            {
                return dateValue;
            }
            return null; // Trả về null nếu nhập không hợp lệ
        }
       

    }
}
