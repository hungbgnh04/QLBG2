using DocumentFormat.OpenXml.Drawing;
using Guna.UI2.WinForms;
using QLBG.DAL;
using QLBG.DTO;
using QLBG.Views.ReportRDLC;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBG.Views.SanPham
{
    public partial class SanPham : UserControl
    {
        private ProductDAL productDAL = new ProductDAL();

        pgChatLieu pageChatLieu;
        pageNhaSX pageNhaSX;
        pageNuocSX pageNuocSX;
        pageKichCo pageKichCo;
        pageMau pageMau;
        pageLoai pageLoai;
        pageHinhDang pageHinhDang;
        pageDacDiem pageDacDiem;
        pageCongDung pageCongDung;

        private string selectedAttribute;
        private bool isAscending;
        private List<ProductDTO> currentProductList;
        private Dictionary<string, string> attributeMapping;

        public SanPham()
        {
            InitializeComponent();
            guna2TabControl1_SelectedIndexChanged(null, null);

            attributeMapping = new Dictionary<string, string>()
            {
                { "Mã hàng", "MaHang" },
                { "Tên hàng hóa", "TenHangHoa" },
                { "Đơn giá nhập", "DonGiaNhap" },
                { "Đơn giá bán", "DonGiaBan" },
                { "Số lượng", "SoLuong" },
                { "Thời gian bảo hành", "ThoiGianBaoHanh" },
                { "Loại", "TenLoai" },
                { "Kích thước", "TenKichThuoc" },
                { "Hình dạng", "TenHinhDang" },
                { "Chất liệu", "TenChatLieu" },
                { "Nước sản xuất", "TenNuocSX" },
                { "Đặc điểm", "TenDacDiem" },
                { "Màu sắc", "TenMau" },
                { "Công dụng", "TenCongDung" },
                { "Nhà sản xuất", "TenNSX" }
            };

            comboBoxSortByAttributeOfProduct.Items.AddRange(attributeMapping.Keys.ToArray());
            comboBoxSortByAttributeOfProduct.SelectedIndex = 0;

            comboBoxSortGreaterOrLess.Items.AddRange(new object[] { "Tăng dần", "Giảm dần" });
            comboBoxSortGreaterOrLess.SelectedIndex = 0;

            selectedAttribute = attributeMapping[comboBoxSortByAttributeOfProduct.SelectedItem.ToString()];
            isAscending = comboBoxSortGreaterOrLess.SelectedItem.ToString() == "Tăng dần";

            SortAndReloadProducts();

            pageChatLieu = new pgChatLieu();
            pgChatLieu.Controls.Add(pageChatLieu);
            pageChatLieu.Dock = DockStyle.Fill;

            pageNhaSX = new pageNhaSX();
            pgNhaSX.Controls.Add(pageNhaSX);
            pageNhaSX.Dock = DockStyle.Fill;

            pageNuocSX = new pageNuocSX();
            pgNuocSX.Controls.Add(pageNuocSX);
            pageNuocSX.Dock = DockStyle.Fill;

            pageKichCo = new pageKichCo();
            pgKichCo.Controls.Add(pageKichCo);
            pageKichCo.Dock = DockStyle.Fill;

            pageMau = new pageMau();
            pgMauSac.Controls.Add(pageMau);
            pageMau.Dock = DockStyle.Fill;

            pageLoai = new pageLoai();
            pgLoai.Controls.Add(pageLoai);
            pageLoai.Dock = DockStyle.Fill;

            pageHinhDang = new pageHinhDang();
            pgHinhDang.Controls.Add(pageHinhDang);
            pageHinhDang.Dock = DockStyle.Fill;

            pageDacDiem = new pageDacDiem();
            pgDacDiem.Controls.Add(pageDacDiem);
            pageDacDiem.Dock = DockStyle.Fill;

            pageCongDung = new pageCongDung();
            pgCongDung.Controls.Add(pageCongDung);
            pageCongDung.Dock = DockStyle.Fill;
        }

        private void LoadProducts()
        {
            SanPhamPanel.Controls.Clear();

            currentProductList = productDAL.GetAllProducts();
            foreach (var product in currentProductList)
            {
                TheSanPham theSanPham = new TheSanPham();
                theSanPham.LoadProductData(product);
                theSanPham.Click += theSanPham1_Click;
                SanPhamPanel.Controls.Add(theSanPham);
            }
        }

        private void SortAndReloadProducts()
        {
            if (currentProductList == null || currentProductList.Count == 0)
            {
                currentProductList = productDAL.GetAllProducts();
            }

            PropertyInfo propInfo = typeof(ProductDTO).GetProperty(selectedAttribute);
            if (propInfo != null)
            {
                if (isAscending)
                {
                    currentProductList = currentProductList.OrderBy(p => propInfo.GetValue(p, null)).ToList();
                }
                else
                {
                    currentProductList = currentProductList.OrderByDescending(p => propInfo.GetValue(p, null)).ToList();
                }
            }

            SanPhamPanel.Controls.Clear();

            foreach (var product in currentProductList)
            {
                TheSanPham theSanPham = new TheSanPham();
                theSanPham.LoadProductData(product);
                theSanPham.Click += theSanPham1_Click;
                SanPhamPanel.Controls.Add(theSanPham);
            }
        }


        private void ThemBtn_Click(object sender, EventArgs e)
        {
            using (var chiTietSanPham = new frmChiTietSanPham(0, frmChiTietSanPham.Mode.Add))
            {
                if (chiTietSanPham.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }

        private void TimBtn_Click(object sender, EventArgs e)
        {
            string searchValue = guna2TextBox1.Text.Trim();
            if (string.IsNullOrEmpty(searchValue))
            {
                LoadProducts();
                return;
            }

            var tokens = searchValue.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0)
            {
                LoadProducts();
                return;
            }

            List<ProductDTO> productList = productDAL.GetProductDetails();

            var filteredProducts = productList.Where(product =>
            {
                
                var productValues = new[]
                {
                    product.TenHangHoa?.ToLower(),
                    product.TenLoai?.ToLower(),
                    product.TenKichThuoc?.ToLower(),
                    product.TenHinhDang?.ToLower(),
                    product.TenChatLieu?.ToLower(),
                    product.TenNuocSX?.ToLower(),
                    product.TenDacDiem?.ToLower(),
                    product.TenMau?.ToLower(),
                    product.TenCongDung?.ToLower(),
                    product.TenNSX?.ToLower()
                };

                return tokens.Any(token => productValues.Any(value => value?.Contains(token) == true));
            }).ToList();

            if (!filteredProducts.Any())
            {
                MessageBox.Show("Không tìm thấy sản phẩm nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            currentProductList = filteredProducts;
            SortAndReloadProducts();
        }


        private void guna2TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TimBtn_Click(sender, e);
            }
        }

        private void pgChatLieu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chất liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void guna2TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var page = guna2TabControl1.SelectedTab;
            if (page == pgChatLieu)
            {
                pageChatLieu.LoadData();
            }
            else if (page == pgNhaSX)
            {
                pageNhaSX.LoadData();
            }
            else if (page == pgNuocSX)
            {
                pageNuocSX.LoadData();
            }
            else if (page == pgKichCo)
            {
                pageKichCo.LoadData();
            }
            else if (page == pgMauSac)
            {
                pageMau.LoadData();
            }
            else if (page == pgLoai)
            {
                pageLoai.LoadData();
            }
            else if (page == pgHinhDang)
            {
                pageHinhDang.LoadData();
            }
            else if (page == pgDacDiem)
            {
                pageDacDiem.LoadData();
            }
            else if (page == pgCongDung)
            {
                pageCongDung.LoadData();
            }
        }

        private void theSanPham1_Click(object sender, EventArgs e)
        {
            var theSP = (TheSanPham)sender;
            using (var chiTietSanPham = new frmChiTietSanPham(theSP.MaSP, frmChiTietSanPham.Mode.View))
            {
                if (chiTietSanPham.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            List<ProductDTO> productList = productDAL.GetProductDetails();
            DataTable dataTable = productDAL.ConvertToDataTable(productList);
            if (dataTable.Rows.Count > 0)
            {
                frmReport reportViewerForm = new frmReport();
                reportViewerForm.LoadReport(dataTable, "DMHangHoa", "ReportDMHangHoa.rdlc");
                reportViewerForm.Show();
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để hiển thị trong báo cáo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void comboBoxSortByAttributeOfProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            string displayedAttribute = comboBoxSortByAttributeOfProduct.SelectedItem.ToString();
            if (attributeMapping.ContainsKey(displayedAttribute))
            {
                selectedAttribute = attributeMapping[displayedAttribute];
            }
            else
            {
                selectedAttribute = "MaHang";
            }
            SortAndReloadProducts();
        }

        private void comboBoxSortGreaterOrLess_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedOrder = comboBoxSortGreaterOrLess.SelectedItem.ToString();
            isAscending = selectedOrder == "Tăng dần";
            SortAndReloadProducts();
        }
    }
}
