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
            btnDangNhap.Enabled = false; 
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {

        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string soDienThoai = txtSdt.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();

            string connString = ConfigurationManager.ConnectionStrings["db_btlQuanLiBanHang"]?.ConnectionString;
               
           
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
                            this.Hide();
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
                    GhiLog("Lỗi hệ thống: " + ex.Message);
                    lblThongBao.Text = "Lỗi , vui lòng thử lại sau!";
                    Refesh();
                }
            }
        }


        public void Refesh()
        {
            txtSdt.Text = "";
            txtMatKhau.Text = "";
            btnDangNhap.Enabled = false;
           
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


        private void KiemTraHopLe()
        {
            string soDienThoai = txtSdt.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();

            bool sdtHopLe = soDienThoai.Length >= 10 && soDienThoai.Length <= 12 && soDienThoai.All(char.IsDigit);
            bool matKhauHopLe = matKhau.Length >= 6;

            btnDangNhap.Enabled = sdtHopLe && matKhauHopLe;
        }


        private void txtSdt_TextChanged(object sender, EventArgs e)
        {
            KiemTraHopLe();
        }

        private void txtMatKhau_TextChanged(object sender, EventArgs e)
        {
            KiemTraHopLe();
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

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
