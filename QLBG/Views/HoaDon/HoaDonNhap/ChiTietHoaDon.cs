using QLBG.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBG.Views.HoaDon.HoaDonNhap
{
    public partial class ChiTietHoaDon : Form
    {
        private int soHDN;
        private HoaDonNhapDAL hoaDonNhapDAL;
        private ChiTietHoaDonNhapDAL chiTietHoaDonDAL;

        public EventHandler OnDeleted;

        public ChiTietHoaDon(int soHDN)
        {
            InitializeComponent();
            RoundCorners(this, 40);
            this.soHDN=soHDN;
            hoaDonNhapDAL = new HoaDonNhapDAL();
            chiTietHoaDonDAL = new ChiTietHoaDonNhapDAL();
        }

        private void ChiTietHoaDon_Load(object sender, EventArgs e)
        {
            InitDGV();
            LoadData();
        }

        private void LoadData()
        {
            var dataHD = hoaDonNhapDAL.GetHoaDonNhapById(soHDN);
            DataTable dataCTHD = chiTietHoaDonDAL.GetChiTietHoaDonNhapBySoHDN(soHDN);

            SHDLb.Text = SHDHeaderLb.Text = dataHD["SoHDN"].ToString();
            lbTenNCC.Text = dataHD["TenNCC"].ToString();
            lbDiaChi.Text = dataHD["DiaChi"].ToString();
            lbSDT.Text = dataHD["DienThoai"].ToString();
            lbNgayNhap.Text = dataHD["NgayNhap"].ToString();
            llbMaNv.Text = dataHD["MaNV"].ToString();
            lbTenNv.Text = dataHD["TenNV"].ToString();
            lbTongTien.Text = lbTongKet.Text = dataHD["TongTien"].ToString();


            if (dataCTHD != null && dataCTHD.Rows.Count > 0)
            foreach (DataRow row in dataCTHD.Rows)
            {
                dgvSanPham.Rows.Add(row["MaHang"], row["TenHangHoa"], row["SoLuong"], row["DonGia"], row["GiamGia"], row["ThanhTien"]);
            }
        }

        private void InitDGV()
        {
            dgvSanPham.Columns.Clear();
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaHang", HeaderText = "Mã Hàng" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenHangHoa", HeaderText = "Tên Hàng Hóa" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoLuong", HeaderText = "Số Lượng" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonGiaNhap", HeaderText = "Giá Nhập" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "GiamGia", HeaderText = "Giảm Giá" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "ThanhTien", HeaderText = "Thành Tiền" });
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

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void lbSDT_Click(object sender, EventArgs e)
        {

        }

        private void btnXuat_Click(object sender, EventArgs e)
        {

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            hoaDonNhapDAL.DeleteHoaDonNhap(soHDN);
            OnDeleted?.Invoke(this, EventArgs.Empty);
            this.Close();
        }
    }
}
