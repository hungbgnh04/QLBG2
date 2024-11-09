using DocumentFormat.OpenXml.Office2019.Excel.RichData;
using QLBG.DAL;
using QLBG.DTO;
using QLBG.Helpers;
using QLBG.Views.SanPham;
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
    public partial class frmTaoHoaDon : Form
    {

        private NhaCungCapDAL NCCDAL;
        private HoaDonNhapDAL HoaDonDAL;
        private ProductDAL SanPhamDAL;
        private ChiTietHoaDonNhapDAL ChiTietHoaDonDAL;

        public EventHandler HoaDonAdded;

        public frmTaoHoaDon()
        {
            InitializeComponent();
            RoundCorners(this, 40);
            NCCDAL = new NhaCungCapDAL();
            HoaDonDAL = new HoaDonNhapDAL();
            SanPhamDAL = new ProductDAL();
            ChiTietHoaDonDAL = new ChiTietHoaDonNhapDAL();
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

        private void frmTaoHoaDon_Load(object sender, EventArgs e)
        {
            InitDGV();
            LoadProduct();
            LoadNCC();
        }

        private void LoadNCC()
        {
            DataTable nhaCungCap = NCCDAL.GetAllNhaCungCap();
            foreach (DataRow row in nhaCungCap.Rows)
            {
                dgvTim.Rows.Add(row["MaNCC"], row["TenNCC"], row["DiaChi"], row["DienThoai"]);
            }
        }

        private void LoadProduct()
        {
            SanPhamPanel.Controls.Clear();
            List<ProductDTO> sanPham = SanPhamDAL.GetAllProducts();
            foreach (ProductDTO product in sanPham)
            {
                TheSanPham productItem = new TheSanPham();
                productItem.LoadProductData(product);
                productItem.Click += ProductItem_OnProductItemClick;
                productItem.FIllColor = Color.White;
                SanPhamPanel.Controls.Add(productItem);
            }
        }

        private void ProductItem_OnProductItemClick(object sender, EventArgs e)
        {
            int maSP = ((TheSanPham)sender).MaSP;

            if (dgvSanPham.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvSanPham.Rows)
                {
                    if (row.Cells["MaSP"].Value.ToString() == maSP.ToString())
                    {
                        int soLuong = int.Parse(row.Cells["SoLuong"].Value.ToString());
                        row.Cells["SoLuong"].Value = soLuong + 1;

                        decimal donGia = decimal.Parse(row.Cells["DonGia"].Value.ToString());
                        decimal giamGia = decimal.Parse(row.Cells["GiamGia"].Value.ToString());
                        row.Cells["ThanhTien"].Value = (soLuong + 1) * donGia * (1 - giamGia / 100);

                        return;
                    }
                }
            }

            ProductDTO product = SanPhamDAL.GetProductById(maSP);

            decimal thanhTien = product.DonGiaBan;
            dgvSanPham.Rows.Add(product.MaHang, product.TenHangHoa, 1, product.DonGiaNhap, 0, thanhTien);
        }

        private void InitDGV()
        {
            dgvTim.Columns.Clear();
            dgvTim.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaNCC", HeaderText = "Mã nhà cung cấp" });
            dgvTim.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenNCC", HeaderText = "Tên Nhà cung cấp" });
            dgvTim.Columns.Add(new DataGridViewTextBoxColumn { Name = "DiaChi", HeaderText = "Địa chỉ" });
            dgvTim.Columns.Add(new DataGridViewTextBoxColumn { Name = "DienThoai", HeaderText = "Số điện thoại" });

            dgvDanhSach.Columns.Clear();
            dgvDanhSach.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaNCC", HeaderText = "Mã nhà cung cấp" });
            dgvDanhSach.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenNCC", HeaderText = "Tên Nhà cung cấp" });
            dgvDanhSach.Columns.Add(new DataGridViewTextBoxColumn { Name = "DiaChi", HeaderText = "Địa chỉ" });
            dgvDanhSach.Columns.Add(new DataGridViewTextBoxColumn { Name = "DienThoai", HeaderText = "Số điện thoại" });

            dgvSanPham.ReadOnly = false;
            dgvSanPham.EditMode = DataGridViewEditMode.EditOnEnter;

            dgvSanPham.Columns.Clear();
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaSP", HeaderText = "Mã sản phẩm", ReadOnly = true });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenSP", HeaderText = "Tên sản phẩm", ReadOnly = true });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoLuong", HeaderText = "Số lượng", ReadOnly = false });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonGia", HeaderText = "Đơn giá", ReadOnly = true });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "GiamGia", HeaderText = "Giảm giá (%)", ReadOnly = false });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "ThanhTien", HeaderText = "Thành tiền", ReadOnly = true });

            DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn
            {
                Name = "Delete",
                HeaderText = "Xóa",
                Text = "Xóa",
                UseColumnTextForButtonValue = true
            };
            dgvSanPham.Columns.Add(deleteColumn);


            dgvSanPham.CellClick += dgvSanPham_CellClick;

            dgvSanPham.EditingControlShowing += dgvSanPham_EditingControlShowing;
            dgvSanPham.CellEndEdit += dgvSanPham_CellEndEdit;
        }

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvSanPham.Columns[e.ColumnIndex].Name == "Delete")
            {
                dgvSanPham.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void dgvSanPham_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvSanPham.CurrentCell.ColumnIndex == dgvSanPham.Columns["SoLuong"].Index ||
                dgvSanPham.CurrentCell.ColumnIndex == dgvSanPham.Columns["DonGia"].Index ||
                dgvSanPham.CurrentCell.ColumnIndex == dgvSanPham.Columns["GiamGia"].Index)
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.KeyPress -= TextBox_KeyPress;
                    textBox.KeyPress += TextBox_KeyPress;
                }
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            else
            {
                dgvSanPham_CellEndEdit(dgvSanPham, new DataGridViewCellEventArgs(dgvSanPham.CurrentCell.ColumnIndex, dgvSanPham.CurrentCell.RowIndex));
            }
        }

        private void dgvSanPham_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvSanPham.Columns["SoLuong"].Index ||
                e.ColumnIndex == dgvSanPham.Columns["DonGia"].Index ||
                e.ColumnIndex == dgvSanPham.Columns["GiamGia"].Index)
            {

                DataGridViewRow row = dgvSanPham.Rows[e.RowIndex];
                int soLuong = int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out int sl) ? sl : 0;

                if (soLuong < 0)
                {
                    MessageBox.Show("Số lượng không thể nhỏ hơn 0", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    row.Cells["SoLuong"].Value = 0;
                    return;
                }

                decimal donGia = decimal.TryParse(row.Cells["DonGia"].Value?.ToString(), out decimal dg) ? dg : 0;
                decimal giamGia = decimal.TryParse(row.Cells["GiamGia"].Value?.ToString(), out decimal gg) ? gg : 0;

                decimal thanhTien = soLuong * donGia * (1 - giamGia / 100);
                row.Cells["ThanhTien"].Value = thanhTien;

                UpdateTotalAmount();
            }
        }

        private void UpdateTotalAmount()
        {
            decimal totalAmount = 0;
            foreach (DataGridViewRow row in dgvSanPham.Rows)
            {
                if (row.Cells["ThanhTien"].Value != null)
                {
                    totalAmount += decimal.TryParse(row.Cells["ThanhTien"].Value.ToString(), out decimal thanhTien) ? thanhTien : 0;
                }
            }

        }


        private void HuyBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvTim_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgvDanhSach.Rows.Clear();
                dgvDanhSach.Rows.Add(dgvTim.Rows[e.RowIndex].Cells["MaNCC"].Value, dgvTim.Rows[e.RowIndex].Cells["TenNCC"].Value, dgvTim.Rows[e.RowIndex].Cells["DiaChi"].Value, dgvTim.Rows[e.RowIndex].Cells["DienThoai"].Value);
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string tenNCC = txtTim.Text;
            DataTable dt = NCCDAL.GetAllNhaCungCap();
            dgvTim.Rows.Clear();
            foreach (DataRow row in dt.Rows)
            {
                if (row["TenNCC"].ToString().Contains(tenNCC))
                {
                    dgvTim.Rows.Add(row["MaNCC"], row["TenNCC"], row["DiaChi"], row["DienThoai"]);
                }
            }
        }

        private void TimBtn_Click(object sender, EventArgs e)
        {
            DataTable dt = SanPhamDAL.GetProductWithAllAttribute();
            string search = txtTimSp.Text.Trim();
            var tokens = search.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            DataTable filteredDt = dt.Clone(); 

            foreach (DataRow row in dt.Rows)
            {
                bool match = tokens.All(token =>
                    row["TenHangHoa"].ToString().ToLower().Contains(token.ToLower()) ||
                    row["MaHang"].ToString().ToLower().Contains(token.ToLower()) ||
                    row["TenLoai"].ToString().ToLower().Contains(token.ToLower()) ||
                    row["TenKichThuoc"].ToString().ToLower().Contains(token.ToLower()) ||
                    row["TenHinhDang"].ToString().ToLower().Contains(token.ToLower()) ||
                    row["TenChatLieu"].ToString().ToLower().Contains(token.ToLower()) ||
                    row["TenNuocSX"].ToString().ToLower().Contains(token.ToLower()) ||
                    row["TenDacDiem"].ToString().ToLower().Contains(token.ToLower()) ||
                    row["TenMau"].ToString().ToLower().Contains(token.ToLower()) ||
                    row["TenCongDung"].ToString().ToLower().Contains(token.ToLower()) ||
                    row["TenNSX"].ToString().ToLower().Contains(token.ToLower())
                );

                if (match)
                {
                    filteredDt.ImportRow(row);
                }
            }

            SanPhamPanel.Controls.Clear();

            foreach (DataRow row in filteredDt.Rows)
            {
                TheSanPham productItem = new TheSanPham();
                productItem.TenLb.Text = row["TenHangHoa"].ToString();
                productItem.MaSP = int.Parse(row["MaHang"].ToString());
                productItem.HangLb.Text = row["TenNSX"].ToString();
                productItem.FIllColor = Color.White;
                productItem.Click += ProductItem_OnProductItemClick;

                SanPhamPanel.Controls.Add(productItem);
            }
        }

        private void btnTao_Click(object sender, EventArgs e)
        {
            if (dgvDanhSach.Rows.Count <= 0)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var maNCC = dgvDanhSach.Rows[0].Cells["MaNCC"].Value;
            var maNV = Session.MaNV;
            var ngayNhap = DateTime.Now;
            decimal tongTien = 0;

            if (dgvSanPham.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (maNCC == null)
            {
                MessageBox.Show("Vui lòng chọn nhà chung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvSanPham.Rows.Count > 0)
            {
                var listSP = new List<ProductDTO>();

                foreach (DataGridViewRow row in dgvSanPham.Rows)
                {
                    var maSP = row.Cells["MaSP"].Value;
                    var donGia = row.Cells["DonGia"].Value;
                    var giamGia = row.Cells["GiamGia"].Value;
                    var thanhTien = row.Cells["ThanhTien"].Value;

                    if (maSP == null || row.Cells["SoLuong"].Value == null || donGia == null || giamGia == null || thanhTien == null)
                    {
                        MessageBox.Show("Vui lòng nhập đủ thông tin sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out int sl) || sl <= 0)
                    {
                        MessageBox.Show("Số lượng sản phẩm không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var sp = SanPhamDAL.GetProductById(int.Parse(maSP.ToString()));
                    sp.SoLuong += sl;
                    listSP.Add(sp);

                    tongTien += decimal.Parse(thanhTien.ToString());
                }


                foreach (var sp in listSP)
                {
                    if (!SanPhamDAL.UpdateProduct(sp))
                    {
                        MessageBox.Show("Cập nhật sản phẩm lỗi");
                    }
                }

                int soHDN = HoaDonDAL.InsertHoaDonNhap(int.Parse(maNV.ToString()), ngayNhap, int.Parse(maNCC.ToString()), tongTien);
                if (soHDN <= 0)
                {
                    MessageBox.Show("Lỗi tạo hóa đơn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                foreach (DataGridViewRow row in dgvSanPham.Rows)
                {
                    var maSP = row.Cells["MaSP"].Value;
                    var giamGia = row.Cells["GiamGia"].Value;
                    var donGia = row.Cells["DonGia"].Value;
                    var thanhTien = row.Cells["ThanhTien"].Value;

                    if (!int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out int sl) || sl <= 0)
                    {
                        MessageBox.Show("Số lượng sản phẩm không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!ChiTietHoaDonDAL.InsertChiTietHoaDonNhap(soHDN, int.Parse(maSP.ToString()), sl, decimal.Parse(donGia.ToString()), decimal.Parse(giamGia.ToString()), decimal.Parse(thanhTien.ToString())))
                    {
                        MessageBox.Show("Tạo hóa đơn thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                MessageBox.Show("Tạo hóa đơn thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HoaDonAdded?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
        }
    }
}
