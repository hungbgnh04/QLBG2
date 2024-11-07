using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using QLBG.DAL;

namespace QLBG.Views.NhanVien
{
    public partial class ThemNhanVien : Form
    {
        private readonly DatabaseHelper dbHelper;
        private string selectedImagePath;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        // Sự kiện để thông báo khi nhân viên mới được thêm
        public event EventHandler NhanVienAdded;

        public ThemNhanVien()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();

            // Thiết lập sự kiện cho việc kéo thả form
            panelMain.MouseDown += DraggablePanel_MouseDown;
            panelMain.MouseMove += DraggablePanel_MouseMove;
            panelMain.MouseUp += DraggablePanel_MouseUp;

            // Làm tròn các góc của form và button
            RoundCorners(this, 60);
            RoundCorners(btnTao, 30);
            RoundCorners(btnThoat, 30);
        }

        private void ThemNhanVien_Load(object sender, EventArgs e)
        {
            comboBoxGioiTinh.Items.Clear();
            comboBoxGioiTinh.Items.Add("Nam");
            comboBoxGioiTinh.Items.Add("Nữ");
            comboBoxGioiTinh.SelectedIndex = -1;

            LoadCongViecData();
        }

        private void LoadCongViecData()
        {
            DataTable dtCongViec = dbHelper.GetAllCongViec();

            if (dtCongViec != null && dtCongViec.Rows.Count > 0)
            {
                comboBoxCongViec.DataSource = dtCongViec;
                comboBoxCongViec.DisplayMember = "TenCV";
                comboBoxCongViec.ValueMember = "MaCV";
                comboBoxCongViec.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Không có dữ liệu công việc để hiển thị.");
            }
        }

        private void btnChonAnhChoNhanVien_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedImagePath = openFileDialog.FileName;
                    pictureBoxAnhNhanVien.Image = Image.FromFile(selectedImagePath);
                }
            }
        }

        private void btnTao_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            string tenNV = textBoxTen.Text;
            bool gioiTinh = comboBoxGioiTinh.SelectedItem.ToString() == "Nam";
            DateTime ngaySinh = dateTimePickerNgaySinh.Value;
            string dienThoai = textBoxSDT.Text;
            string password = textBoxMatKhau.Text;
            bool quyenAdmin = checkBoxQuyenAdmin.Checked;
            string email = textBoxEmail.Text;
            string diaChi = textBoxDiaChi.Text;
            int maCV = (int)comboBoxCongViec.SelectedValue;

            string anh = SaveImageToDirectory(selectedImagePath);

            bool isSuccess = dbHelper.AddEmployee(tenNV, gioiTinh, ngaySinh, dienThoai, password, quyenAdmin, email, diaChi, anh, maCV);

            if (isSuccess)
            {
                MessageBox.Show("Nhân viên đã được thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                NhanVienAdded?.Invoke(this, EventArgs.Empty);
                ClearInputFields();
                this.Close();
            }
            else
            {
                MessageBox.Show("Thêm nhân viên thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(textBoxTen.Text))
            {
                ShowWarning("Vui lòng nhập tên nhân viên.", textBoxTen);
                return false;
            }

            if (comboBoxGioiTinh.SelectedIndex == -1)
            {
                ShowWarning("Vui lòng chọn giới tính.", comboBoxGioiTinh);
                return false;
            }

            if (string.IsNullOrEmpty(textBoxSDT.Text))
            {
                ShowWarning("Vui lòng nhập số điện thoại.", textBoxSDT);
                return false;
            }

            if (string.IsNullOrEmpty(textBoxMatKhau.Text))
            {
                ShowWarning("Vui lòng nhập mật khẩu.", textBoxMatKhau);
                return false;
            }

            if (string.IsNullOrEmpty(textBoxEmail.Text))
            {
                ShowWarning("Vui lòng nhập email.", textBoxEmail);
                return false;
            }

            if (comboBoxCongViec.SelectedIndex == -1)
            {
                ShowWarning("Vui lòng chọn công việc.", comboBoxCongViec);
                return false;
            }

            if (dbHelper.CheckEmailExist(textBoxEmail.Text))
            {
                ShowWarning("Email đã tồn tại trong hệ thống. Vui lòng nhập email khác.", textBoxEmail);
                return false;
            }

            return true;
        }

        private void ShowWarning(string message, Control control)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
        }

        private string SaveImageToDirectory(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return null;

            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string imageDirectory = Path.Combine(projectDirectory, "Resources", "EmployeeImages");

            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            string imageFileName = Path.GetFileName(imagePath);
            string targetPath = Path.Combine(imageDirectory, imageFileName);

            if (!File.Exists(targetPath))
            {
                File.Copy(imagePath, targetPath, true);
            }

            return imageFileName;
        }

        private void ClearInputFields()
        {
            textBoxTen.Clear();
            comboBoxGioiTinh.SelectedIndex = -1;
            dateTimePickerNgaySinh.Value = DateTime.Now;
            textBoxSDT.Clear();
            textBoxMatKhau.Clear();
            checkBoxQuyenAdmin.Checked = false;
            textBoxEmail.Clear();
            textBoxDiaChi.Clear();
            comboBoxCongViec.SelectedIndex = -1;
            pictureBoxAnhNhanVien.Image = null;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form
        }

        private void RoundCorners(Control control, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(control.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(control.Width - radius, control.Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, control.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            control.Region = new Region(path);
        }

        // Các hàm để xử lý kéo thả form
        private void DraggablePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
                this.Opacity = 0.8;
            }
        }

        private void DraggablePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void DraggablePanel_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            this.Opacity = 1;
        }
    }
}
