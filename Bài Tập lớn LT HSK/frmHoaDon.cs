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
    public partial class frmHoaDon : Form
    {
        string connString = ConfigurationManager.ConnectionStrings["db_btlQuanLiBanHang"].ConnectionString;
        public frmHoaDon()
        {
            InitializeComponent();
        }

        private void frmHoaDon_Load(object sender, EventArgs e)
        {
            LoadHoaDon();
            rboHoaDonBan.Checked = true;

            // Gán sự kiện CheckedChanged cho RadioButton
            rboHoaDonBan.CheckedChanged += RadioButton_CheckedChanged;
            rboHoaDonNhap.CheckedChanged += RadioButton_CheckedChanged;

            btnChiTiet.Enabled = false;
        }
        private void LoadHoaDon()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open(); // Mở kết nối

                string storedProcedure;

                // Xác định stored procedure tương ứng
                if (rboHoaDonBan.Checked)
                {
                    storedProcedure = "sp_GetHoaDon";
                }
                else if (rboHoaDonNhap.Checked)
                {
                    storedProcedure = "sp_GetHoaDonNhap";
                }
                else
                {
                    storedProcedure = "sp_GetHoaDon";
                }

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
    }
}
