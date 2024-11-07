using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using QLBG.DAL;

namespace QLBG.Views.CongViec
{
    public partial class ThemCongViec : Form
    {
        private readonly CongViecDAL congViecDAL;

        public event EventHandler CongViecAdded;

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public ThemCongViec()
        {
            InitializeComponent();
            congViecDAL = new CongViecDAL();

            // Make form and buttons rounded
            RoundCorners(this, 60);
            RoundCorners(btnTao, 30);
            RoundCorners(btnThoat, 30);

            // Enable draggable functionality
            panelMain.MouseDown += DraggablePanel_MouseDown;
            panelMain.MouseMove += DraggablePanel_MouseMove;
            panelMain.MouseUp += DraggablePanel_MouseUp;
        }

        private void btnTao_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            string tenCV = textBoxTenCV.Text;
            decimal mucLuong = Convert.ToDecimal(textBoxMucLuong.Text);

            if (congViecDAL.AddCongViec(tenCV, mucLuong))
            {
                MessageBox.Show("Công việc đã được thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CongViecAdded?.Invoke(this, EventArgs.Empty);
                ClearInputFields();
                this.Close();
            }
            else
            {
                MessageBox.Show("Thêm công việc thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(textBoxTenCV.Text))
            {
                ShowWarning("Vui lòng nhập tên công việc.", textBoxTenCV);
                return false;
            }

            if (string.IsNullOrEmpty(textBoxMucLuong.Text) || !decimal.TryParse(textBoxMucLuong.Text, out _))
            {
                ShowWarning("Vui lòng nhập mức lương hợp lệ.", textBoxMucLuong);
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
            textBoxTenCV.Clear();
            textBoxMucLuong.Clear();
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
