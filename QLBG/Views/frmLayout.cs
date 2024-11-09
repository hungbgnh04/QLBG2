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

            // Khởi tạo các form con
            homePage = new TrangChu();
            sanPham = new SanPham.SanPham();
            hoaDon = new HoaDon.HoaDon();
            nhanVien = new NhanVien.NhanVien();
            khachHang = new KhachHang.KhachHang();
            congViec = new CongViec.CongViec();
            nhaCungCap = new NhaCungCap.NhaCungCap();
            dbHelper = new DatabaseHelper();

            // Đặt các tooltip cho các nút
            ToolTip.SetToolTip(UserIcon, "Thông tin cá nhân");
            ToolTip.SetToolTip(HomeBtn, "Trang chủ");
            ToolTip.SetToolTip(BillBtn, "Hóa đơn");
            ToolTip.SetToolTip(ProductBtn, "Danh sách sản phẩm");
            ToolTip.SetToolTip(CustomerBtn, "Danh sách khách hàng");
            ToolTip.SetToolTip(EmployeeBtn, "Danh sách nhân viên");
            ToolTip.SetToolTip(SupplierBtn, "Danh sách nhà cung cấp");
            ToolTip.SetToolTip(LogoutBtn, "Đăng xuất");

            // Thiết lập Timer cho phiên tự động đăng xuất
            sessionTimer = new Timer();
            sessionTimer.Interval = 1000 * 60 * 30; // 30 phút
            sessionTimer.Tick += SessionTimeout;
            sessionTimer.Start();
            //load icon user
            LoadUserIcon();

            //ko phai Admin thi ko vao dc nhan vien va cong viec
            if (!Session.QuyenAdmin)
            {
                EmployeeBtn.Visible = false;
                JobBtn.Visible = false;
            }
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            // Mở trang chủ khi form được tải
            HomeBtn_Click(HomeBtn, e);
        }

        private void ShowControl(Control control)
        {
            // Hiển thị control trong panel cha
            panelParent.Controls.Clear();
            control.Dock = DockStyle.Fill;
            panelParent.Controls.Add(control);
            control.BringToFront();
        }

        private void moveEffect(object sender)
        {
            // Tạo hiệu ứng di chuyển cho nút đang chọn
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
            // Cho phép di chuyển cửa sổ bằng cách kéo tiêu đề
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
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
            homePage.HomePage_Load(null, null);
            HomeLabel.Text = "Trang chủ";
        }

        private void LoadUserIcon()
        {
            // Kiểm tra nếu MaNV trong Session hợp lệ
            if (Session.MaNV <= 0)
            {
                MessageBox.Show("Không tìm thấy thông tin đăng nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Lấy thông tin nhân viên từ Database dựa trên MaNV trong Session
            DataRow employee = dbHelper.GetEmployeeByMaNV(Session.MaNV);
            if (employee != null)
            {
                // Lấy tên ảnh từ cơ sở dữ liệu
                string imageName = employee["Anh"] as string;

                // Xác định đường dẫn thư mục chứa ảnh
                string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imageDirectory = Path.Combine(projectDirectory, "Resources", "EmployeeImages");
                string imagePath = Path.Combine(imageDirectory, imageName ?? "");

                try
                {
                    // Kiểm tra nếu ảnh nhân viên tồn tại
                    if (!string.IsNullOrEmpty(imageName) && File.Exists(imagePath))
                    {
                        UserIcon.Image = Image.FromFile(imagePath);
                    }
                    else
                    {
                        // Sử dụng ảnh mặc định nếu không tìm thấy ảnh nhân viên
                        string defaultImagePath = Path.Combine(imageDirectory, "ic_user.png");
                        if (File.Exists(defaultImagePath))
                        {
                            UserIcon.Image = Image.FromFile(defaultImagePath);
                        }
                        else
                        {
                            // Nếu không có ảnh mặc định, sử dụng ảnh từ tài nguyên dự án
                            UserIcon.Image = Properties.Resources.eye; // Giả sử bạn có ảnh mặc định trong tài nguyên dự án
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể tải ảnh nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Sử dụng ảnh mặc định nếu có lỗi
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

            // Mở form ChiTietNhanVien khi nhấp vào UserIcon
            ChiTietNhanVien chiTietNhanVienForm = new ChiTietNhanVien(Session.MaNV.ToString());

            // Đăng ký sự kiện FormClosed để tải lại ảnh khi form ChiTietNhanVien đóng
            chiTietNhanVienForm.FormClosed += (s, args) => LoadUserIcon();

            chiTietNhanVienForm.ShowDialog();
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
            // Đăng xuất và trở về form đăng nhập
            Session.ClearAuthentication(); // Xóa thông tin phiên
            LoginForm loginForm = new LoginForm(); // Tạo form đăng nhập
            loginForm.Show();                    // Hiển thị form đăng nhập
            this.Close();                        // Đóng form hiện tại (frmLayout)
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

        private void SessionTimeout(object sender, EventArgs e)
        {
            // Tự động đăng xuất sau khi hết thời gian
            MessageBox.Show("Phiên làm việc đã hết hạn. Vui lòng đăng nhập lại.");
            Session.ClearAuthentication(); // Xóa phiên
            this.Close();

            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }
    }
}
