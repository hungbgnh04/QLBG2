using DocumentFormat.OpenXml.Drawing;
using Guna.UI2.WinForms;
using QLBG.DAL;
using QLBG.DTO;
using System;
using System.Collections.Generic;
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

        public SanPham()
        {
            InitializeComponent();
            guna2TabControl1_SelectedIndexChanged(null, null);
            LoadProducts();

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
            List<ProductDTO> productList = productDAL.GetAllProducts();
            Console.WriteLine("Total products retrieved: " + productList.Count);
            foreach (var product in productList)
            {
                Console.WriteLine("Product ID: " + product.MaHang);
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
            string searchValue = guna2TextBox1.Text;
            if (string.IsNullOrEmpty(searchValue))
            {
                LoadProducts();
                return;
            }
            var tokens = searchValue.Split(' ');
            List<ProductDTO> productList = productDAL.GetAllProducts();
            foreach (var token in tokens)
            {
                if (string.IsNullOrEmpty(token))
                {
                    continue;
                }
                productList = productList.FindAll(product => product.TenHangHoa.ToLower().Contains(token.ToLower()));
            }
            if (productList.Count == 0)
            {
                MessageBox.Show("Không tìm thấy sản phẩm nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SanPhamPanel.Controls.Clear();
            foreach (var product in productList)
            {
                Console.WriteLine("Product ID: " + product.MaHang);
                TheSanPham theSanPham = new TheSanPham();
                theSanPham.LoadProductData(product);
                theSanPham.Click += theSanPham1_Click;
                SanPhamPanel.Controls.Add(theSanPham);
            }
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
    }
}
