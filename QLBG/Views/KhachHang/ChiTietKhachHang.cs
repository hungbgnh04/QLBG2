using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using QLBG.DAL;

namespace QLBG.Views.KhachHang
{
    public partial class ChiTietKhachHang : Form
    {
        private readonly KhachHangDAL khachHangDAL;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private readonly int maKhach;

        public event EventHandler KhachHangUpdated;

        public ChiTietKhachHang(int maKhach)
        {
            InitializeComponent();
            khachHangDAL = new KhachHangDAL();
            this.maKhach = maKhach;

            panelMain.MouseDown += DraggablePanel_MouseDown;
            panelMain.MouseMove += DraggablePanel_MouseMove;
            panelMain.MouseUp += DraggablePanel_MouseUp;

            RoundCorners(this, 60);
            RoundCorners(btnUpdate, 30);
            RoundCorners(btnXoa, 30);
            RoundCorners(btnThoat, 30);
        }

        private void ChiTietKhachHang_Load(object sender, EventArgs e)
        {
            LoadKhachHangDetails();
        }

        private void LoadKhachHangDetails()
        {
            DataRow customer = khachHangDAL.GetKhachHangById(maKhach);

            if (customer != null)
            {
                textBoxMaKhach.Text = customer["MaKhach"].ToString();
                textBoxTenKhach.Text = customer["TenKhach"].ToString();
                textBoxDiaChi.Text = customer["DiaChi"].ToString();
                textBoxDienThoai.Text = customer["DienThoai"].ToString();
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin khách hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            string tenKhach = textBoxTenKhach.Text;
            string diaChi = textBoxDiaChi.Text;
            string dienThoai = textBoxDienThoai.Text;

            bool isSuccess = khachHangDAL.UpdateKhachHang(maKhach, tenKhach, diaChi, dienThoai);

            if (isSuccess)
            {
                MessageBox.Show("Cập nhật thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                KhachHangUpdated?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
            else
            {
                MessageBox.Show("Cập nhật thông tin khách hàng thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                bool isSuccess = khachHangDAL.DeleteKhachHang(maKhach);
                if (isSuccess)
                {
                    MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    KhachHangUpdated?.Invoke(this, EventArgs.Empty);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Xóa khách hàng thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
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

            if (khachHangDAL.CheckKhachHangExist(textBoxDienThoai.Text, maKhach))
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
