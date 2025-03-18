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
    public partial class frmKhachHang : Form
    {
        string connString = ConfigurationManager.ConnectionStrings["db_btlQuanLiBanHang"]?.ConnectionString;
        public frmKhachHang()
        {
            InitializeComponent();
            LoadKhachHang();
        }

        private void frmKhachHang_Load(object sender, EventArgs e)
        {

        }

        private void LoadKhachHang()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_GetKhachHang", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Đổ dữ liệu vào ComboBox
                        cboKhachHang.DataSource = dt;
                        cboKhachHang.DisplayMember = "sHoTen"; // Hiển thị tên khách hàng
                        cboKhachHang.ValueMember = "iMaKH"; // Lưu mã khách hàng
                        cboKhachHang.SelectedIndex = -1; // Không chọn mặc định
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách khách hàng: " + ex.Message);
            }
        }

        private void btnInBaoCao_Click(object sender, EventArgs e)
        {
            if (cboKhachHang.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để in báo cáo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maKH = Convert.ToInt32(cboKhachHang.SelectedValue);
            frmXemBaoCao frm = new frmXemBaoCao(maKH);
            frm.ShowDialog();
        }
    }
}
