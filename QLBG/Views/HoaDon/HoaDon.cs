using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBG.Views.HoaDon
{
    public partial class HoaDon : UserControl
    {
        private HoaDonNhap.pageHoaDonNhap pgHDN;
        private HoaDonBan.pageHoaDonBan pgHDBan;

        public HoaDon()
        {
            InitializeComponent();

            pgHDN = new HoaDonNhap.pageHoaDonNhap();
            pgHoaDonNhap.Controls.Add(pgHDN);
            pgHDN.Dock = DockStyle.Fill;

            pgHDBan = new HoaDonBan.pageHoaDonBan();
            pgHoaDonBan.Controls.Add(pgHDBan);
            pgHDBan.Dock = DockStyle.Fill;
        }

        private void pgHoaDonBan_Click(object sender, EventArgs e)
        {

        }

        private void guna2TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2TabControl1.SelectedIndex == 1)
            {
                pgHDN.LoadData();
            }
            else
            {
                pgHDBan.LoadData();
            }
        }
    }
}
