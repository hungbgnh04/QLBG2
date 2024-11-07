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
            string projectDirectory = Directory.GetParent(Application.StartupPath).Parent.FullName;
            string imagePath = Path.Combine(projectDirectory, @"Resources\ProductImages", product.Anh);
            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                PictureBoxAnh.Image = Image.FromFile(imagePath);
            }
            else
            {
                PictureBoxAnh.Image = Image.FromFile(Path.Combine(projectDirectory, @"Resources\ProductImages\default_product.png")); 
            }
            PictureBoxAnh.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private string GetManufacturerName(int maNSX)
        {
            NhaSanXuatDAL nhaSanXuatDAL = new NhaSanXuatDAL();
            var nhaSanXuat = nhaSanXuatDAL.GetNhaSanXuatById(maNSX);
            return nhaSanXuat?.TenNSX ?? "Unknown";
        }

        private void TheSanPham_Resize(object sender, EventArgs e)
        {
            TenLb.Width = HangLb.Width = (int)(this.Width * 0.893);
            HangLb.Location = new Point((this.Width - HangLb.Width) / 2, (int)(this.Height - HangLb.Height - 5));
            TenLb.Location = new Point((this.Width - HangLb.Width) / 2, HangLb.Height - TenLb.Height - 5);
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
