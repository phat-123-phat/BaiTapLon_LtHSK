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
    public partial class frmChiTietHoaDon : Form
    {
        string connString = ConfigurationManager.ConnectionStrings["db_btlQuanLiBanHang"].ConnectionString;
        private int maHD; // Lưu mã hóa đơn
        public frmChiTietHoaDon(int maHD)
        {
            InitializeComponent();
            this.maHD = maHD;
            
        }

        private void frmChiTietHoaDon_Load(object sender, EventArgs e)
        {
            LoadThongTinHoaDon();
            LoadChiTietHoaDon();
            ChuyenSangNhapLieu(false);
            txtMaHD.ReadOnly = true;
            txtTongTien.ReadOnly = true;
        }
        private void LoadThongTinHoaDon()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {

                SqlCommand cmd = new SqlCommand("sp_GetHoaDonByID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@iMaHoaDon", maHD);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtMaHD.Text = reader["iMaHDB"].ToString();
                    txtTenKH.Text = reader["TenKH"].ToString();
                    txtSDT.Text = reader["sdtKH"].ToString();
                    if (Convert.ToBoolean(reader["GioiTinhKH"]))
                        rdoNam.Checked = true;
                    else
                        rdoNu.Checked = true;

                    txtDiaChi.Text = reader["DiaChiKH"].ToString();
                    txtTenNV.Text = reader["TenNV"].ToString();

                    DateTime ngayLap = Convert.ToDateTime(reader["dNgayBan"]);
                    mtbNgayLap.Text = ngayLap.ToString("dd/MM/yyyy");

                    chkDaThanhToan.Checked = Convert.ToBoolean(reader["bDaThuTien"]);

                }
                reader.Close();
            }
        }

        private void LoadChiTietHoaDon()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("sp_LoadHangHoa_HoaDon", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@iMaHoaDon", maHD);

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                // Kiểm tra nếu có dữ liệu tổng tiền
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    txtTongTien.Text = ds.Tables[1].Rows[0]["TongTien"].ToString();
                }
                else
                {
                    txtTongTien.Text = "0"; // Nếu không có dữ liệu
                }

                // Hiển thị danh sách hàng hóa
                dgvChiTietHoaDon.DataSource = ds.Tables[0];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChuyenSangNhapLieu(true);
            txtTenKH.Focus();
        }
        private void ChuyenSangNhapLieu(bool b)
        {
            txtTenKH.ReadOnly = !b;
            txtSDT.ReadOnly = !b;
            rdoNam.Enabled = b;
            rdoNu.Enabled = b;
            txtDiaChi.ReadOnly = !b;
            chkDaThanhToan.Enabled = b;
            txtTenNV.ReadOnly = !b;
            mtbNgayLap.ReadOnly = !b;


            btnLuu.Enabled = b;
            btnHuy.Enabled = b;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            LoadThongTinHoaDon();
            LoadChiTietHoaDon();
            ChuyenSangNhapLieu(false);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_UpdateHoaDon", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Lấy dữ liệu từ giao diện
                    cmd.Parameters.AddWithValue("@iMaHoaDon", maHD);
                    cmd.Parameters.AddWithValue("@TenKH", txtTenKH.Text);
                    cmd.Parameters.AddWithValue("@sdtKH", txtSDT.Text);
                    cmd.Parameters.AddWithValue("@GioiTinhKH", rdoNam.Checked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@DiaChiKH", txtDiaChi.Text);
                    cmd.Parameters.AddWithValue("@bDaThuTien", chkDaThanhToan.Checked);
                    cmd.Parameters.AddWithValue("@sTenNV", txtTenNV.Text);

                    // Chuyển đổi định dạng ngày tháng
                    DateTime ngayBan;
                    if (DateTime.TryParseExact(mtbNgayLap.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngayBan))
                    {
                        cmd.Parameters.AddWithValue("@dNgayBan", ngayBan);
                    }
                    

                    cmd.ExecuteNonQuery();
                  
                    LoadThongTinHoaDon();

                }
                catch (Exception ex)
                {
                    GhiLog("Lỗi: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmReport frmReport = new frmReport(maHD);
            frmReport.ShowDialog();
            
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

        private bool KiemTraDuLieu()
        {
            // Kiểm tra tên khách hàng không được để trống
            if (string.IsNullOrWhiteSpace(txtTenKH.Text))
                return false;
            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
                return false;
            // Kiểm tra số điện thoại (chỉ được nhập số, độ dài từ 10-11 số)
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSDT.Text, @"^\d{10,11}$"))
                return false;

            // Kiểm tra ngày lập hóa đơn hợp lệ
            DateTime ngayBan;
            if (!DateTime.TryParseExact(mtbNgayLap.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngayBan))
                return false;

            // Kiểm tra tên nhân viên không được để trống
            if (string.IsNullOrWhiteSpace(txtTenNV.Text))
                return false;

            // Nếu tất cả hợp lệ, trả về true
            return true;
        }

        private void txtTenKH_TextChanged(object sender, EventArgs e)
        {
            btnLuu.Enabled = KiemTraDuLieu();
        }

        private void txtSDT_TextChanged(object sender, EventArgs e)
        {
            btnLuu.Enabled = KiemTraDuLieu();
        }

        private void txtDiaChi_TextChanged(object sender, EventArgs e)
        {
            btnLuu.Enabled = KiemTraDuLieu();
        }

        private void mtbNgayLap_TextChanged(object sender, EventArgs e)
        {
            btnLuu.Enabled = KiemTraDuLieu();
        }

        private void txtTenNV_TextChanged(object sender, EventArgs e)
        {
            btnLuu.Enabled = KiemTraDuLieu();
        }
    }
}
