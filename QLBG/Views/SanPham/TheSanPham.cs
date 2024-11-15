using QLBG.DAL;
using QLBG.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBG.Views.SanPham
{
    public partial class TheSanPham : UserControl
    {
        public int MaSP { get; set; }

        public Color FIllColor
        {
            get
            {
                return guna2ShadowPanel1.FillColor;
            }
            set
            {
                guna2ShadowPanel1.FillColor = value;
                this.Update();
            }
        }

        public TheSanPham()
        {
            InitializeComponent();
        }

        public void LoadProductData(ProductDTO product)
        {
            MaSP = product.MaHang;
            TenLb.Text = product.TenHangHoa;
            HangLb.Text = GetManufacturerName(product.MaNSX);
            PictureBoxAnh.Image = ImageManager.GetProductImage(product.Anh);
            PictureBoxAnh.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private string GetManufacturerName(int maNSX)
        {
            NhaSanXuatDAL nhaSanXuatDAL = new NhaSanXuatDAL();
            var nhaSanXuat = nhaSanXuatDAL.GetNhaSanXuatById(maNSX);
            return nhaSanXuat?.TenNSX ?? "Unknown";
        }
        private void TenLb_Click_1(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
