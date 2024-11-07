using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using QLBG.DAL;

namespace QLBG.Views.NhaCungCap
{
    public partial class ThemNhaCungCap : Form
    {
        private readonly NhaCungCapDAL nhaCungCapDAL;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public event EventHandler NhaCungCapAdded;

        public ThemNhaCungCap()
        {
            InitializeComponent();
            nhaCungCapDAL = new NhaCungCapDAL();

            // Set up draggable events
            panelMain.MouseDown += DraggablePanel_MouseDown;
            panelMain.MouseMove += DraggablePanel_MouseMove;
            panelMain.MouseUp += DraggablePanel_MouseUp;

            // Round corners for the form and buttons
            RoundCorners(this, 60);
            RoundCorners(btnTao, 30);
            RoundCorners(btnThoat, 30);
        }

        private void btnTao_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            string tenNCC = textBoxTenNCC.Text;
            string diaChi = textBoxDiaChi.Text;
            string dienThoai = textBoxDienThoai.Text;

            if (nhaCungCapDAL.AddNhaCungCap(tenNCC, diaChi, dienThoai))
            {
                MessageBox.Show("Nhà cung cấp đã được thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                NhaCungCapAdded?.Invoke(this, EventArgs.Empty);
                ClearInputFields();
                this.Close();
            }
            else
            {
                MessageBox.Show("Thêm nhà cung cấp thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(textBoxTenNCC.Text))
            {
                ShowWarning("Vui lòng nhập tên nhà cung cấp.", textBoxTenNCC);
                return false;
            }

            if (string.IsNullOrEmpty(textBoxDienThoai.Text))
            {
                ShowWarning("Vui lòng nhập số điện thoại.", textBoxDienThoai);
                return false;
            }

            if (nhaCungCapDAL.CheckNhaCungCapExist(textBoxDienThoai.Text))
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
            textBoxTenNCC.Clear();
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

        // Draggable form functionality
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
