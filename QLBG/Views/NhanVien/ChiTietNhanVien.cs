using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using QLBG.DAL;

namespace QLBG.Views.NhanVien
{
    public partial class ChiTietNhanVien : Form
    {
        private readonly DatabaseHelper dbHelper;
        private string selectedImagePath;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private readonly int maNV;
        public event EventHandler EmployeeUpdated;
        public ChiTietNhanVien(string maNVString)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();

            // Chuyển đổi maNV từ chuỗi sang int
            if (int.TryParse(maNVString, out int parsedMaNV))
            {
                maNV = parsedMaNV;
            }
            else
            {
                MessageBox.Show("Mã nhân viên không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            // Thiết lập sự kiện cho việc kéo thả form
            panelMain.MouseDown += DraggablePanel_MouseDown;
            panelMain.MouseMove += DraggablePanel_MouseMove;
            panelMain.MouseUp += DraggablePanel_MouseUp;

            // Làm tròn các góc của form và button
            RoundCorners(this, 60);
            RoundCorners(btnUpdate, 30);
            RoundCorners(btnXoa, 30);
            RoundCorners(btnThoat, 30);
        }

        private void ChiTietNhanVien_Load(object sender, EventArgs e)
        {
            LoadComboBoxGioiTinh();
            LoadComboBoxCongViec();
            LoadEmployeeDetails();
        }

        private void LoadComboBoxGioiTinh()
        {
            comboBoxGioiTinh.Items.Clear();
            comboBoxGioiTinh.Items.Add("Nam");
            comboBoxGioiTinh.Items.Add("Nữ");
            comboBoxGioiTinh.SelectedIndex = -1; // Đặt mặc định là không có mục nào được chọn
        }

        private void LoadComboBoxCongViec()
        {
            DataTable dtCongViec = dbHelper.GetAllCongViec();
            if (dtCongViec != null && dtCongViec.Rows.Count > 0)
            {
                comboBoxCongViec.DataSource = dtCongViec;
                comboBoxCongViec.DisplayMember = "TenCV";  // Tên công việc
                comboBoxCongViec.ValueMember = "MaCV";      // Mã công việc (giá trị thực sự)
                comboBoxCongViec.SelectedIndex = -1;        // Đặt mặc định là không có mục nào được chọn
            }
            else
            {
                MessageBox.Show("Không có dữ liệu công việc để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadEmployeeDetails()
        {
            DataRow employee = dbHelper.GetEmployeeByMaNV(maNV);

            if (employee != null)
            {
                textBoxMaNV.Text = employee["MaNV"].ToString();
                textBoxTen.Text = employee["TenNV"].ToString();
                comboBoxGioiTinh.SelectedItem = Convert.ToBoolean(employee["GioiTinh"]) ? "Nam" : "Nữ";
                dateTimePickerNgaySinh.Value = Convert.ToDateTime(employee["NgaySinh"]);
                textBoxSDT.Text = employee["DienThoai"].ToString();
                textBoxEmail.Text = employee["Email"].ToString();
                textBoxDiaChi.Text = employee["DiaChi"].ToString();
                textBoxMatKhau.Text = employee["Password"].ToString();
                comboBoxCongViec.SelectedValue = Convert.ToInt32(employee["MaCV"]);
                checkBoxQuyenAdmin.Checked = Convert.ToBoolean(employee["QuyenAdmin"]);

                // Tải ảnh nhân viên
                string imageName = employee["Anh"].ToString();
                string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imageDirectory = Path.Combine(projectDirectory, "Resources", "EmployeeImages");
                string imagePath = Path.Combine(imageDirectory, imageName);

                // Kiểm tra nếu ảnh nhân viên tồn tại
                if (!string.IsNullOrEmpty(imageName) && File.Exists(imagePath))
                {
                    pictureBoxAnhNhanVien.Image = Image.FromFile(imagePath);
                    selectedImagePath = imagePath;
                }
                else
                {
                    // Sử dụng ảnh mặc định nếu không tìm thấy ảnh nhân viên
                    string defaultImagePath = Path.Combine(imageDirectory, "ic_user.png");
                    if (File.Exists(defaultImagePath))
                    {
                        pictureBoxAnhNhanVien.Image = Image.FromFile(defaultImagePath);
                    }
                    else
                    {
                        pictureBoxAnhNhanVien.Image = Properties.Resources.eye; // Nếu ảnh mặc định cũng không tồn tại
                    }
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            string tenNV = textBoxTen.Text;
            bool gioiTinh = comboBoxGioiTinh.SelectedItem.ToString() == "Nam";
            DateTime ngaySinh = dateTimePickerNgaySinh.Value;
            string dienThoai = textBoxSDT.Text;
            string email = textBoxEmail.Text;
            string diaChi = textBoxDiaChi.Text;
            int maCV = (int)comboBoxCongViec.SelectedValue;
            bool quyenAdmin = checkBoxQuyenAdmin.Checked;
            string password = textBoxMatKhau.Text;
            string anh = SaveImageToDirectory(selectedImagePath);

            // Lấy email hiện tại của nhân viên từ cơ sở dữ liệu
            DataRow employee = dbHelper.GetEmployeeByMaNV(maNV);
            if (employee == null)
            {
                MessageBox.Show("Không tìm thấy thông tin nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string currentEmail = employee["Email"].ToString();

            // Kiểm tra nếu email mới khác email hiện tại và đã tồn tại trong hệ thống
            if (email != currentEmail && dbHelper.CheckEmailExist(email))
            {
                MessageBox.Show("Email đã tồn tại trong hệ thống. Vui lòng nhập email khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tiến hành cập nhật nếu email không trùng hoặc là email hiện tại
            bool isSuccess = dbHelper.UpdateEmployee(maNV, tenNV, gioiTinh, ngaySinh, dienThoai, email, diaChi, anh, maCV, quyenAdmin, password);

            if (isSuccess)
            {
                MessageBox.Show("Cập nhật thông tin nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                EmployeeUpdated?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
            else
            {
                MessageBox.Show("Cập nhật thông tin nhân viên thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnXoa_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?",
                                     "Xác nhận xóa",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                bool isSuccess = dbHelper.DeleteEmployee(maNV);
                if (isSuccess)
                {
                    MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    EmployeeUpdated?.Invoke(this, EventArgs.Empty);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Xóa nhân viên thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
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

            // Kiểm tra mật khẩu không được bỏ trống
            if (string.IsNullOrEmpty(textBoxMatKhau.Text))
            {
                ShowWarning("Vui lòng nhập mật khẩu.", textBoxMatKhau);
                return false;
            }

            // Kiểm tra email trùng khi cập nhật (loại bỏ bản thân)
            if (dbHelper.CheckEmailExistForUpdate(textBoxEmail.Text, maNV))
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
