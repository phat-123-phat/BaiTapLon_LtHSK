using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bài_Tập_lớn_LT_HSK
{
    public partial class frmInBaoCao : Form
    {
        DataTable DataTable;
        private string hoTen, dienThoai, cccd, diaChi;
        private bool? gioiTinh;
        private DateTime? ngaySinh, ngayVaoLam;

        public frmInBaoCao(string hoTen, string dienThoai, string cccd, string diaChi, bool? gioiTinh, DateTime? ngaySinh, DateTime? ngayVaoLam, DataTable dt)
        {
            InitializeComponent();
            this.hoTen = hoTen;
            this.dienThoai = dienThoai;
            this.cccd = cccd;
            this.diaChi = diaChi;
            this.gioiTinh = gioiTinh;
            this.ngaySinh = ngaySinh;
            this.ngayVaoLam = ngayVaoLam;
            this.DataTable = dt;
        }

        private void frmInBaoCao_Load(object sender, EventArgs e)
        {
            ShowReport();
        }
        private void ShowReport()
        {
            // Khởi tạo report
            ReportDocument report = new ReportDocument();
            report.Load(@"C:\Users\Admin\Desktop\Bài tập lớn - Hướng sự kiện\Project\Bài Tập lớn LT HSK\Bài Tập lớn LT HSK\NhanVien.rpt");
            report.SetDataSource(DataTable);
            // Gán giá trị cho các tham số
            report.SetParameterValue("pHoTen", string.IsNullOrEmpty(hoTen) ? "Tất cả" : hoTen);
            report.SetParameterValue("pDienThoai", string.IsNullOrEmpty(dienThoai) ? "Tất cả" : dienThoai);
            report.SetParameterValue("pCCCD", string.IsNullOrEmpty(cccd) ? "Tất cả" : cccd);
            report.SetParameterValue("pDiaChi", string.IsNullOrEmpty(diaChi) ? "Tất cả" : diaChi);
            report.SetParameterValue("pGioiTinh", gioiTinh == null ? "Tất cả" : (gioiTinh.Value ? "Nam" : "Nữ"));
            report.SetParameterValue("pNgaySinh", ngaySinh?.ToString("dd/MM/yyyy") ?? "Tất cả");
            report.SetParameterValue("pNgayVaoLam", ngayVaoLam?.ToString("dd/MM/yyyy") ?? "Tất cả");

            
            // Gán report vào Crystal Report Viewer
            crystalReportViewer1.ReportSource = report;
            crystalReportViewer1.Refresh();
        }

    }
}
