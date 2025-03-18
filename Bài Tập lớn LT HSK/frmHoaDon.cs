using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bài_Tập_lớn_LT_HSK
{
    public partial class frmHoaDon : Form
    {
       

        string connString = ConfigurationManager.ConnectionStrings["db_btlQuanLiBanHang"].ConnectionString;
        bool Luu = false;
        public frmHoaDon()
        {
            InitializeComponent();
        }

        private void frmHoaDon_Load(object sender, EventArgs e)
        {
            LoadHoaDon();
           
            btnChiTiet.Enabled = false;
            btnTim.Enabled = false;
        }
        private void LoadHoaDon()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open(); // Mở kết nối

                string storedProcedure;

              
                    storedProcedure = "sp_GetHoaDon";
               

                using (SqlCommand cmd = new SqlCommand(storedProcedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure; // Đặt kiểu là stored procedure

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd)) // Truyền command vào adapter
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dgvHoaDon.DataSource = dt;
                    }
                }
            }
        }


        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.SelectedRows.Count > 0) // Kiểm tra có dòng nào được chọn không
            {
                int maHD = Convert.ToInt32(dgvHoaDon.SelectedRows[0].Cells[0].Value);

                // Mở form chi tiết hóa đơn và truyền mã hóa đơn
                frmChiTietHoaDon frm = new frmChiTietHoaDon(maHD);
                frm.MdiParent = this.MdiParent; // Đặt form con trong form cha
                frm.Show();
                this.Close();


            }
            else
            {

            }
        }

        private void dgvHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            LoadHoaDon();
        }

        private void dgvHoaDon_SelectionChanged(object sender, EventArgs e)
        {
            btnChiTiet.Enabled = dgvHoaDon.SelectedRows.Count > 0;
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

                    System.IO.File.AppendAllText("error_log.txt", DateTime.Now + ": " + ex.Message + Environment.NewLine);
                }
            }
        }


        private void btnTimHoaDon_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_TimKiemHoaDon", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Lấy giá trị từ các ô nhập liệu
                        string tenNhanVien = cboTenNV.Text.Trim();
                        string tenKhachHang = cboTenKH.Text.Trim();
                        string soDienThoai = txtSdtKH.Text.Trim();
                        DateTime? ngayBan = null;

                        if (!string.IsNullOrWhiteSpace(mtbNgayBan.Text) && DateTime.TryParse(mtbNgayBan.Text, out DateTime dateValue))
                        {
                            ngayBan = dateValue;
                        }

                        // Lấy giá trị của trạng thái thu tiền
                        bool? daThuTien = null;
                        if (ckbDaThanhToan.Checked)
                        {
                            daThuTien = true;
                        }    
                        else
                        {
                            daThuTien = false;
                        }
                            

                        // Thêm tham số vào Stored Procedure
                        cmd.Parameters.Add("@TenNhanVien", SqlDbType.NVarChar, 100).Value = string.IsNullOrEmpty(tenNhanVien) ? (object)DBNull.Value : tenNhanVien;
                        cmd.Parameters.Add("@TenKhachHang", SqlDbType.NVarChar, 100).Value = string.IsNullOrEmpty(tenKhachHang) ? (object)DBNull.Value : tenKhachHang;
                        cmd.Parameters.Add("@SoDienThoai", SqlDbType.NVarChar, 20).Value = string.IsNullOrEmpty(soDienThoai) ? (object)DBNull.Value : soDienThoai;
                        cmd.Parameters.Add("@NgayBan", SqlDbType.Date).Value = ngayBan.HasValue ? ngayBan.Value : (object)DBNull.Value;
                        cmd.Parameters.Add("@DaThuTien", SqlDbType.Bit).Value = daThuTien.HasValue ? daThuTien.Value : (object)DBNull.Value;

                        // Đổ dữ liệu vào DataGridView
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvHoaDon.DataSource = dt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    GhiLog("Lỗi tìm kiếm hóa đơn: " + ex.Message);
                }


            }
        }
        private void KiemTraDuLieuTimKiem()
        {
            bool b;
            // Nếu tất cả các ô nhập đều rỗng, disable button
                b = !(
                string.IsNullOrWhiteSpace(cboTenNV.Text) &&
                string.IsNullOrWhiteSpace(cboTenKH.Text) &&
                string.IsNullOrWhiteSpace(txtSdtKH.Text) &&
                string.IsNullOrWhiteSpace(mtbNgayBan.Text) &&
                !ckbDaThanhToan.Checked &&
                mtbNgayBan.MaskCompleted
            );

            btnTim.Enabled = b;
            Luu = b;
        }

        private void txtTenNV_TextChanged(object sender, EventArgs e)
        {
            KiemTraDuLieuTimKiem();
        }

        private void txtTenKH_TextChanged(object sender, EventArgs e)
        {
            KiemTraDuLieuTimKiem();
        }

        private void txtSdtKH_TextChanged(object sender, EventArgs e)
        {
            KiemTraDuLieuTimKiem();
        }

        private void mtbNgayBan_TextChanged(object sender, EventArgs e)
        {
            KiemTraDuLieuTimKiem();
        }

        private void rboDaThu_CheckedChanged(object sender, EventArgs e)
        {
            KiemTraDuLieuTimKiem();
        }

        private void rboChuaThu_CheckedChanged(object sender, EventArgs e)
        {
            KiemTraDuLieuTimKiem();
        }

        private void ckbDaThanhToan_CheckedChanged(object sender, EventArgs e)
        {
            KiemTraDuLieuTimKiem();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadDanhSachNhanVien();
            LoadDanhSachKhachHang();
            if (Luu)
            {
                try
                {
                    // Lấy dữ liệu từ các control
                    int maNV = (int)cboTenNV.SelectedValue; // Mã nhân viên được chọn
                    int maKH = (int)cboTenKH.SelectedValue; // Mã khách hàng được chọn
                    string ngayBanText = mtbNgayBan.Text;  // Ngày bán từ MaskedTextBox
                    bool daThuTien = ckbDaThanhToan.Checked; // Trạng thái thanh toán

                    // Kiểm tra và chuyển đổi ngày bán
                    DateTime? ngayBan = ChuyenDoiNgayBan(ngayBanText);
                    if (!ngayBan.HasValue)
                    {
                        MessageBox.Show("Ngày bán không hợp lệ. Vui lòng nhập theo định dạng dd/MM/yyyy.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Tạo giá trị mới cho iMaHDB
                    int newID = GenerateNewID();

                    // Thêm hóa đơn vào cơ sở dữ liệu
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("sp_ThemHoaDon", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Thêm các tham số vào stored procedure
                            cmd.Parameters.AddWithValue("@iMaHDB", newID); // Sử dụng giá trị ID mới
                            cmd.Parameters.AddWithValue("@MaNV", maNV);
                            cmd.Parameters.AddWithValue("@MaKH", maKH);
                            cmd.Parameters.AddWithValue("@NgayBan", ngayBan.Value);
                            cmd.Parameters.AddWithValue("@DaThuTien", daThuTien);

                            // Thực thi stored procedure
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Làm mới danh sách hóa đơn
                    

                    // Reset các control và nút
                    ResetForm();

                    LoadHoaDon();

                    SelectNewlyAddedInvoice(newID);
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi
                    GhiLog("Lỗi thêm hóa đơn: " + ex.Message);
                    MessageBox.Show("Lỗi khi thêm hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ResetForm()
        {
            // Reset các control
            cboTenNV.SelectedIndex = -1;
            cboTenKH.SelectedIndex = -1;
            mtbNgayBan.Clear();
            ckbDaThanhToan.Checked = false;

            // Reset nút
            button1.Text = "Thêm Hóa Đơn";
            button1.Enabled = true;
        }

        private void LoadDanhSachNhanVien()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetDanhSachNhanVien", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Đổ dữ liệu vào ComboBox
                        cboTenNV.DataSource = dt;
                        cboTenNV.DisplayMember = "sHoTen"; // Hiển thị tên nhân viên
                        cboTenNV.ValueMember = "iMaNV";   // Giá trị là mã nhân viên
                    }
                }
            }
        }

        private void LoadDanhSachKhachHang()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetDanhSachKhachHang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Đổ dữ liệu vào ComboBox
                        cboTenKH.DataSource = dt;
                        cboTenKH.DisplayMember = "sHoTen"; // Hiển thị tên khách hàng
                        cboTenKH.ValueMember = "iMaKH";   // Giá trị là mã khách hàng
                    }
                }
            }
        }

        private DateTime? ChuyenDoiNgayBan(string ngayBanText)
        {
            // Kiểm tra xem chuỗi nhập vào có đúng định dạng dd/MM/yyyy không
            if (DateTime.TryParseExact(ngayBanText, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ngayBan))
            {
                return ngayBan; // Trả về ngày hợp lệ
            }
            return null; // Trả về null nếu định dạng không hợp lệ
        }

        private int GenerateNewID()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(iMaHDB), 0) + 1 FROM tblHoadonban", conn))
                {
                    // Lấy giá trị lớn nhất hiện tại của iMaiIDB và tăng lên 1
                    return (int)cmd.ExecuteScalar();
                }
            }
        }
        private void SelectNewlyAddedInvoice(int newID)
        {
            // Duyệt qua các dòng trong DataGridView để tìm hóa đơn mới thêm
            foreach (DataGridViewRow row in dgvHoaDon.Rows)
            {
                // Lấy giá trị cột iMaHDB của dòng hiện tại
                int maHDB = Convert.ToInt32(row.Cells["Mã hóa đơn"].Value);

                // Nếu tìm thấy hóa đơn mới thêm
                if (maHDB == newID)
                {
                    // Chọn dòng đó
                    row.Selected = true;

                    // Cuộn đến dòng được chọn
                    dgvHoaDon.FirstDisplayedScrollingRowIndex = row.Index;

                    // Thoát khỏi vòng lặp
                    break;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có dòng nào được chọn không
            if (dgvHoaDon.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy mã hóa đơn được chọn
            int maHDB = Convert.ToInt32(dgvHoaDon.SelectedRows[0].Cells["Mã hóa đơn"].Value);

            // Xác nhận xóa
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return; // Hủy bỏ nếu người dùng chọn "No"
            }

            try
            {
                // Xóa hóa đơn từ cơ sở dữ liệu
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_XoaHoaDon", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm tham số mã hóa đơn
                        cmd.Parameters.AddWithValue("@MaHDB", maHDB);

                        // Thực thi stored procedure
                        cmd.ExecuteNonQuery();
                    }
                }

                // Thông báo thành công
                MessageBox.Show("Xóa hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Làm mới danh sách hóa đơn
                LoadHoaDon();
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                GhiLog("Lỗi xóa hóa đơn: " + ex.Message);
                MessageBox.Show("Lỗi khi xóa hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
