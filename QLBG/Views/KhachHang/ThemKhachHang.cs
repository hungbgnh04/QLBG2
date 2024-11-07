using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using QLBG.DAL;

namespace QLBG.Views.KhachHang
{
    public partial class ThemKhachHang : Form
    {
        private readonly KhachHangDAL khachHangDAL;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        // Sự kiện thông báo khi khách hàng mới được thêm
        public event EventHandler KhachHangAdded;

        public ThemKhachHang()
        {
            InitializeComponent();
            khachHangDAL = new KhachHangDAL();

            // Thiết lập sự kiện cho việc kéo thả form
            panelMain.MouseDown += DraggablePanel_MouseDown;
            panelMain.MouseMove += DraggablePanel_MouseMove;
            panelMain.MouseUp += DraggablePanel_MouseUp;

            // Làm tròn các góc của form và button
            RoundCorners(this, 60);
            RoundCorners(btnTao, 30);
            RoundCorners(btnThoat, 30);
        }

        private void btnTao_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            string tenKhach = textBoxTenKhach.Text;
            string diaChi = textBoxDiaChi.Text;
            string dienThoai = textBoxDienThoai.Text;

            if (khachHangDAL.AddKhachHang(tenKhach, diaChi, dienThoai))
            {
                MessageBox.Show("Khách hàng đã được thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                KhachHangAdded?.Invoke(this, EventArgs.Empty);
                ClearInputFields();
                this.Close();
            }
            else
            {
                MessageBox.Show("Thêm khách hàng thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(textBoxTenKhach.Text))
            {
                ShowWarning("Vui lòng nhập tên khách hàng.", textBoxTenKhach);
                return false;
            }

            if (string.IsNullOrEmpty(textBoxDienThoai.Text))
            {
                ShowWarning("Vui lòng nhập số điện thoại.", textBoxDienThoai);
                return false;
            }

            if (khachHangDAL.CheckKhachHangExist(textBoxDienThoai.Text))
            {
                ShowWarning("Số điện thoại đã tồn tại trong hệ thống. Vui lòng nhập số khác.", textBoxDienThoai);
                return false;
            }

            return true;
        }

        private void ShowWarning(string message, Control control)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
        }

        private void ClearInputFields()
        {
            textBoxTenKhach.Clear();
            textBoxDiaChi.Clear();
            textBoxDienThoai.Clear();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
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

        // Các hàm xử lý kéo thả form
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
