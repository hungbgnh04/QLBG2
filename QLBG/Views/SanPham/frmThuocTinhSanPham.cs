using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace QLBG.Views.SanPham
{
    public partial class frmThuocTinhSanPham : Form
    {
        public enum Mode
        {
            Add,
            Edit,
            View,
            Delete
        }

        public Mode mode { get; private set; }
        public DataGridViewRow row { get; private set; }

        public frmThuocTinhSanPham(DataGridViewRow row, Mode mode)
        {
            InitializeComponent();
            this.mode = mode;
            this.row = row;
            Init(row);

            RoundCorners(this, 40);
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

        private void Init(DataGridViewRow row)
        {
            switch (mode)
            {
                case Mode.Add:
                    HeaderLb.Text = "THÊM THUỘC TÍNH SẢN PHẨM";
                    MaLb.Visible = false;
                    txtMa.Visible = false;
                    txtTen.Enabled = true;
                    guna2Panel2.Visible = false;
                    ModifyBtnPanel.Visible = true;
                    ModifyBtnPanel.Location = ViewBtnPanel.Location;
                    ViewBtnPanel.Visible = false;
                    txtMa.Text = "";
                    txtTen.Text = "";
                    break;
                case Mode.Edit:
                    HeaderLb.Text = "SỬA THUỘC TÍNH SẢN PHẨM";
                    MaLb.Visible = false;
                    txtMa.Visible = false;
                    txtTen.Enabled = true;
                    guna2Panel2.Visible = false;
                    ModifyBtnPanel.Visible = true;
                    ModifyBtnPanel.Location = ViewBtnPanel.Location;
                    ViewBtnPanel.Visible = false;
                    txtMa.Text = row.Cells[0].Value.ToString();
                    txtTen.Text = row.Cells[1].Value.ToString();
                    break;
                case Mode.View:
                    HeaderLb.Text = "THÔNG TIN THUỘC TÍNH SẢN PHẨM";
                    MaLb.Visible = true;
                    txtMa.Visible = true;
                    txtTen.Enabled = false;
                    txtMa.Enabled = false;
                    ModifyBtnPanel.Visible = false;
                    ViewBtnPanel.Visible = true;
                    txtMa.Text = row.Cells[0].Value.ToString();
                    txtTen.Text = row.Cells[1].Value.ToString();
                    break;
            }
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (mode == Mode.Edit)
            {
                row.Cells[1].Value = txtTen.Text;
            }
            else if (mode == Mode.Add)
            {
                row.Cells[1].Value = txtTen.Text;
                row.Cells[0].Value = 0;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            mode = Mode.Delete;
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            mode = Mode.Edit;
            Init(row);
        }


    }
}
