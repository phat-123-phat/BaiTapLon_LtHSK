using CrystalDecisions.CrystalReports.Engine;
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
    public partial class frmXemBaoCao : Form
    {
        string connString = ConfigurationManager.ConnectionStrings["db_btlQuanLiBanHang"]?.ConnectionString;

        private int maKH;
        public frmXemBaoCao(int maKH)
        {
            InitializeComponent();
            this.maKH = maKH;
        }

        private void frmXemBaoCao_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ReportKhachHang_HoaDon", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaKH", maKH);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("Khách hàng chưa có hóa đơn nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                            return;
                        }

                        ReportDocument rpt = new ReportDocument();
                        rpt.Load(@"C:\Users\Admin\Desktop\Bài tập lớn - Hướng sự kiện\Project\Bài Tập lớn LT HSK\Bài Tập lớn LT HSK\rptKhachHangHoaDon.rpt");
                        rpt.SetDataSource(dt);
                        crystalReportViewer1.ReportSource = rpt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
    }
}
