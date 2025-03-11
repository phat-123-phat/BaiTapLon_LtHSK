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
    public partial class frmMain : Form
    {
        string tenNhanVien;
        public frmMain(string tenNhanVien)
        {
            InitializeComponent();
            this.tenNhanVien = tenNhanVien;
            this.IsMdiContainer = true;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblTenNhanVien.Text = "Ca làm việc của " + tenNhanVien;
            lblThoiGian.Text = "Thời gian: " + DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
        }


        private void OpenChildForm(Form childForm)
        {
            foreach (Form form in this.MdiChildren)
            {
                form.Close();
            }

            childForm.MdiParent = this; // Đặt form con vào MDI container
            childForm.StartPosition = FormStartPosition.CenterScreen; // Hiển thị giữa màn hình
            childForm.Show();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmDanhMucHangHoa());
        }

        private void quảnLýKháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmQuanLyNhanVien());
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmHoaDon());
        }
    }
}
