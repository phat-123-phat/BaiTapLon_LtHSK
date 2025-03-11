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
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
            lblThongBao.Text = "";
        }
        private void frmDangNhap_Load(object sender, EventArgs e)
        {

        }
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            
            string soDienThoai = txtSdt.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();

            // Kiểm tra rỗng
            if (string.IsNullOrEmpty(matKhau) || string.IsNullOrEmpty(soDienThoai))
            {
                lblThongBao.Text = "Vui lòng nhập số điện thoại và mật khẩu!";
                return;
            }

            // lấy kết nối
            string connString = ConfigurationManager.ConnectionStrings["db_btlQuanLiBanHang"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_NhanVien_DangNhap", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DienThoai", soDienThoai);
                        cmd.Parameters.AddWithValue("@MatKhau", matKhau);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read()) 
                        {
                            string tenNhanVien = reader["sHoTen"].ToString();
                            this.Hide(); // Ẩn form đăng nhập
                            frmMain mainForm = new frmMain(tenNhanVien);
                            mainForm.ShowDialog();
                            this.Show();
                            Refesh();
                        }
                        else
                        {
                            lblThongBao.Text = "Sai số điện thoại hoặc mật khẩu!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối: " + ex.Message);
                }
            }
        }
        // làm mới lại ô nhập liệu
        public void Refesh()
        {
            txtSdt.Text = "";
            txtMatKhau.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtSdt.Text = "0901123456";
            txtMatKhau.Text = "123456";
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.PasswordChar = chkHienMatKhau.Checked ? '\0' : '*';
        }
    }
}
