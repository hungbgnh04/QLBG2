using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBG.Views
{
    public partial class frmLayout : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private TrangChu homePage;
        private SanPham.SanPham sanPham;
        private HoaDon.HoaDon hoaDon;
        private NhanVien.NhanVien nhanVien;
        private KhachHang.KhachHang khachHang;
        private CongViec.CongViec congViec;
        private NhaCungCap.NhaCungCap nhaCungCap;

        public frmLayout()
        {
            InitializeComponent();

            homePage = new TrangChu();
            sanPham = new SanPham.SanPham();
            hoaDon = new HoaDon.HoaDon();
            nhanVien = new NhanVien.NhanVien();
            khachHang = new KhachHang.KhachHang();
            congViec = new CongViec.CongViec();
            nhaCungCap = new NhaCungCap.NhaCungCap();
            //

            ToolTip.SetToolTip(UserIcon, "Thông tin cá nhân");
            ToolTip.SetToolTip(HomeBtn, "Trang chủ");
            ToolTip.SetToolTip(BillBtn, "Hóa đơn");
            ToolTip.SetToolTip(ProductBtn, "Danh sách sản phẩm");
            ToolTip.SetToolTip(CustomerBtn, "Danh sách khách hàng");
            ToolTip.SetToolTip(OverallBtn, "Tổng quan");
            ToolTip.SetToolTip(EmployeeBtn, "Danh sách nhân viên");
            ToolTip.SetToolTip(SupplierBtn, "Danh sách nhà cung cấp");
            ToolTip.SetToolTip(LogoutBtn, "Đăng xuất");
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            HomeBtn_Click(HomeBtn, e);
        }


        private void ShowControl(Control control)
        {
            panelParent.Controls.Clear();
            control.Dock = DockStyle.Fill;
            panelParent.Controls.Add(control);
            control.BringToFront();
        }

        private void moveEffect(object sender)
        {
            Control btn = (Control)sender;
            btnEffect.Location = new Point()
            {
                X = btnEffect.Location.X,
                Y = btn.Location.Y - (btnEffect.Height - btn.Height) / 2 + 1
            };
            btnEffect.BringToFront();
            btnEffect.Visible = true;
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void RevenueBtn_Click(object sender, EventArgs e)
        {
            moveEffect(sender);
            //ShowControl(tongquanForm);
            HomeLabel.Text = "Tổng quan";
        }

        private void EmployeeBtn_Click(object sender, EventArgs e)
        {
            moveEffect(sender);
            HomeLabel.Text = "Danh sách nhân viên";
            ShowControl(nhanVien);
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            moveEffect(sender);
            ShowControl(homePage);
            HomeLabel.Text = "Trang chủ";
        }

        private void UserIcon_Click(object sender, EventArgs e)
        {
            btnEffect.Visible = false;
            foreach (Control item in AsidePanel.Controls)
            {
                if (item is Guna2Button && item != sender)
                {
                    ((Guna2Button)item).Checked = false;
                }
            }
            HomeLabel.Text = "Thông tin cá nhân";
        }

        private void BillBtn_Click(object sender, EventArgs e)
        {
            moveEffect(sender);
            HomeLabel.Text = "Hóa đơn";
            ShowControl(hoaDon);
        }

        private void ProductBtn_Click(object sender, EventArgs e)
        {
            moveEffect(sender);
            HomeLabel.Text = "Danh sách sản phẩm";
            ShowControl(sanPham);

        }

        private void CustomerBtn_Click(object sender, EventArgs e)
        {
            moveEffect(sender);
            HomeLabel.Text = "Danh sách khách hàng";
            ShowControl(khachHang);
        }

        private void LogoutBtn_Click(object sender, EventArgs e)
        {

        }

        private void JobBtn_Click(object sender, EventArgs e)
        {
            HomeLabel.Text = "Danh sách công việc";
            moveEffect(sender);
            ShowControl(congViec);
        }

        private void SupplierBtn_Click(object sender, EventArgs e)
        {
            HomeLabel.Text = "Danh sách nhà cung cấp";
            moveEffect(sender);
            ShowControl(nhaCungCap);
        }
    }
}
