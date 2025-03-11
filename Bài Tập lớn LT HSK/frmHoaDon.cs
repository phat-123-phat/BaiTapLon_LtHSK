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
        }
        private void LoadHoaDon()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlDataAdapter da = new SqlDataAdapter("sp_GetHoaDon", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvHoaDon.DataSource = dt;
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
                MessageBox.Show("Vui lòng chọn một hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
