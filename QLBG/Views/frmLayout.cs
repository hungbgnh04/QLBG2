using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using QLBG.Helpers;
using QLBG.Views.Access;
using QLBG.DAL;
using QLBG.Views.NhanVien;
using System.Data;
using System.IO;
using System.Drawing.Drawing2D;

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

        private Timer sessionTimer;
        private DatabaseHelper dbHelper;

        public frmLayout()
        {
            InitializeComponent();

            RoundCorners(this, 40);

            homePage = new TrangChu();
            sanPham = new SanPham.SanPham();
            hoaDon = new HoaDon.HoaDon();
            nhanVien = new NhanVien.NhanVien();
            khachHang = new KhachHang.KhachHang();
            congViec = new CongViec.CongViec();
            nhaCungCap = new NhaCungCap.NhaCungCap();
            dbHelper = new DatabaseHelper();

            ToolTip.SetToolTip(UserIcon, "Thông tin cá nhân");
            ToolTip.SetToolTip(HomeBtn, "Trang chủ");
            ToolTip.SetToolTip(BillBtn, "Hóa đơn");
            ToolTip.SetToolTip(ProductBtn, "Danh sách sản phẩm");
            ToolTip.SetToolTip(CustomerBtn, "Danh sách khách hàng");
            ToolTip.SetToolTip(EmployeeBtn, "Danh sách nhân viên");
            ToolTip.SetToolTip(SupplierBtn, "Danh sách nhà cung cấp");
            ToolTip.SetToolTip(LogoutBtn, "Đăng xuất");
            ToolTip.SetToolTip(JobBtn, "Danh sách công việc");

            sessionTimer = new Timer();
            sessionTimer.Interval = 1000 * 60 * 30;
            sessionTimer.Tick += SessionTimeout;
            sessionTimer.Start();
            LoadUserIcon();

            if (!Session.QuyenAdmin)
            {
                EmployeeBtn.Visible = false;
                JobBtn.Visible = false;
            }
        }

        private void RoundCorners(Control control, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new System.Drawing.Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new System.Drawing.Rectangle(control.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new System.Drawing.Rectangle(control.Width - radius, control.Height - radius, radius, radius), 0, 90);
            path.AddArc(new System.Drawing.Rectangle(0, control.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            control.Region = new Region(path);
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

        private void EmployeeBtn_Click(object sender, EventArgs e)
        {
            moveEffect(sender);
            EmployeeBtn.Update();
            HomeLabel.Text = "Danh sách nhân viên";
            ShowControl(nhanVien);
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            moveEffect(sender);
            HomeBtn.Update();
            ShowControl(homePage);
            homePage.HomePage_Load(null, null);
            HomeLabel.Text = "Trang chủ";
        }

        private void LoadUserIcon()
        {
            if (Session.MaNV <= 0)
            {
                MessageBox.Show("Không tìm thấy thông tin đăng nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            nhanVien.LoadEmployeeData();

            DataRow employee = dbHelper.GetEmployeeByMaNV(Session.MaNV);
            if (employee != null)
            {
                string imageName = employee["Anh"] as string;

                string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imageDirectory = Path.Combine(projectDirectory, "Resources", "EmployeeImages");
                string imagePath = Path.Combine(imageDirectory, imageName ?? "");

                try
                {
                    if (!string.IsNullOrEmpty(imageName) && File.Exists(imagePath))
                    {
                        UserIcon.Image = Image.FromFile(imagePath);
                    }
                    else
                    {
                        string defaultImagePath = Path.Combine(imageDirectory, "ic_user.png");
                        if (File.Exists(defaultImagePath))
                        {
                            UserIcon.Image = Image.FromFile(defaultImagePath);
                        }
                        else
                        {
                            UserIcon.Image = Properties.Resources.eye;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể tải ảnh nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UserIcon.Image = Properties.Resources.eye;
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            ChiTietNhanVien chiTietNhanVienForm = new ChiTietNhanVien(Session.MaNV.ToString());

            chiTietNhanVienForm.FormClosed += (s, args) => LoadUserIcon();

            chiTietNhanVienForm.ShowDialog();
        }


        private void BillBtn_Click(object sender, EventArgs e)
        {
            moveEffect(sender);
            BillBtn.Update();
            HomeLabel.Text = "Hóa đơn";
            ShowControl(hoaDon);
        }

        private void ProductBtn_Click(object sender, EventArgs e)
        {
            moveEffect(sender);
            ProductBtn.Update();
            HomeLabel.Text = "Danh sách sản phẩm";
            ShowControl(sanPham);
        }

        private void CustomerBtn_Click(object sender, EventArgs e)
        {
            moveEffect(sender);
            CustomerBtn.Update();
            HomeLabel.Text = "Danh sách khách hàng";
            ShowControl(khachHang);
        }

        private void LogoutBtn_Click(object sender, EventArgs e)
        {
            Session.ClearAuthentication();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }

        private void JobBtn_Click(object sender, EventArgs e)
        {
            HomeLabel.Text = "Danh sách công việc";
            moveEffect(sender);
            JobBtn.Update();
            ShowControl(congViec);
        }

        private void SupplierBtn_Click(object sender, EventArgs e)
        {
            HomeLabel.Text = "Danh sách nhà cung cấp";
            moveEffect(sender);
            SupplierBtn.Update();
            ShowControl(nhaCungCap);
        }

        private void SessionTimeout(object sender, EventArgs e)
        {
            MessageBox.Show("Phiên làm việc đã hết hạn. Vui lòng đăng nhập lại.");
            Session.ClearAuthentication();
            this.Close();

            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

        private void CloseControl_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
