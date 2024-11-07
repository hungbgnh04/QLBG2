using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using QLBG.DAL;

namespace QLBG.Views.CongViec
{
    public partial class ChiTietCongViec : Form
    {
        private readonly CongViecDAL congViecDAL;
        private readonly int maCV;

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public event EventHandler CongViecUpdated;

        public ChiTietCongViec(int maCV)
        {
            InitializeComponent();
            congViecDAL = new CongViecDAL();
            this.maCV = maCV;

            RoundCorners(this, 60);
            RoundCorners(btnUpdate, 30);
            RoundCorners(btnXoa, 30);
            RoundCorners(btnThoat, 30);

            panelMain.MouseDown += DraggablePanel_MouseDown;
            panelMain.MouseMove += DraggablePanel_MouseMove;
            panelMain.MouseUp += DraggablePanel_MouseUp;
        }

        private void ChiTietCongViec_Load(object sender, EventArgs e)
        {
            LoadCongViecDetails();
        }

        private void LoadCongViecDetails()
        {
            DataRow job = congViecDAL.GetCongViecById(maCV);

            if (job != null)
            {
                textBoxMaCV.Text = job["MaCV"].ToString();
                textBoxTenCV.Text = job["TenCV"].ToString();
                textBoxMucLuong.Text = job["MucLuong"].ToString();
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin công việc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            string tenCV = textBoxTenCV.Text;
            decimal mucLuong = Convert.ToDecimal(textBoxMucLuong.Text);

            bool isSuccess = congViecDAL.UpdateCongViec(maCV, tenCV, mucLuong);

            if (isSuccess)
            {
                MessageBox.Show("Cập nhật thông tin công việc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CongViecUpdated?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
            else
            {
                MessageBox.Show("Cập nhật thông tin công việc thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa công việc này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                bool isSuccess = congViecDAL.DeleteCongViec(maCV);
                if (isSuccess)
                {
                    MessageBox.Show("Xóa công việc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CongViecUpdated?.Invoke(this, EventArgs.Empty);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Xóa công việc thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
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
