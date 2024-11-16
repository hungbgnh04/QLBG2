using iTextSharp.text.pdf;
using iTextSharp.text;
using QLBG.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLBG.DTO;
using QLBG.Helpers;

namespace QLBG.Views.HoaDon.HoaDonBan
{
    public partial class ChiTietHoaDon : Form
    {
        private int soHDB;
        private HoaDonBanDAL hoaDonBanDAL;
        private ChiTietHoaDonNhapDAL chiTietHoaDonDAL;
        private ProductDAL productDAL;

        public EventHandler OnDeleted;

        public ChiTietHoaDon(int soHDB)
        {
            InitializeComponent();
            RoundCorners(this, 40);
            this.soHDB = soHDB;
            hoaDonBanDAL = new HoaDonBanDAL();
            chiTietHoaDonDAL = new ChiTietHoaDonNhapDAL();
            productDAL = new ProductDAL();
        }

        private void ChiTietHoaDon_Load(object sender, EventArgs e)
        {
            InitDGV();
            LoadData();
        }

        private void LoadData()
        {
            var dataHD = hoaDonBanDAL.GetInvoice(soHDB);
            DataTable dataCTHD = hoaDonBanDAL.LayChiTietHoaDon(soHDB);

            SHDLb.Text = SHDHeaderLb.Text = dataHD["SoHDB"].ToString();
            lbTenKH.Text = dataHD["TenKhach"].ToString();
            lbDiaChi.Text = dataHD["DiaChi"].ToString();
            lbSDT.Text = dataHD["DienThoai"].ToString();
            lbNgayBan.Text = dataHD["NgayBan"].ToString();
            llbMaNv.Text = dataHD["MaNV"].ToString();
            lbTenNv.Text = dataHD["TenNV"].ToString();
            lbTongTien.Text = lbTongKet.Text = dataHD["TongTien"].ToString();

            if (dataCTHD != null && dataCTHD.Rows.Count > 0)
            {
                foreach (DataRow row in dataCTHD.Rows)
                {
                    dgvSanPham.Rows.Add(row["MaHang"], row["TenHangHoa"], row["SoLuong"], row["DonGia"], row["GiamGia"], row["ThanhTien"]);
                }
            }
        }

        private void InitDGV()
        {
            dgvSanPham.Columns.Clear();
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaHang", HeaderText = "Mã Hàng" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenHangHoa", HeaderText = "Tên Hàng Hóa" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoLuong", HeaderText = "Số Lượng" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "DonGiaNhap", HeaderText = "Giá Bán" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "GiamGia", HeaderText = "Giảm Giá" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "ThanhTien", HeaderText = "Thành Tiền" });
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

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void lbSDT_Click(object sender, EventArgs e)
        {

        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            string pdfFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"HoaDonBan_{soHDB}.pdf");

            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "tahoma.ttf");
            BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font titleFont = new iTextSharp.text.Font(bf, 18, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font contentFont = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL);

            Document pdfDoc = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(pdfFilePath, FileMode.Create));
            pdfDoc.Open();

            Paragraph title = new Paragraph("Chi Tiết Hóa Đơn Bán", titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20f
            };
            pdfDoc.Add(title);

            pdfDoc.Add(new Paragraph($"Số HĐB: {SHDLb.Text}", contentFont));
            pdfDoc.Add(new Paragraph($"Khách Hàng: {lbTenKH.Text}", contentFont));
            pdfDoc.Add(new Paragraph($"Địa Chỉ: {lbDiaChi.Text}", contentFont));
            pdfDoc.Add(new Paragraph($"SĐT: {lbSDT.Text}", contentFont));
            pdfDoc.Add(new Paragraph($"Ngày Bán: {lbNgayBan.Text}", contentFont));
            pdfDoc.Add(new Paragraph($"Mã NV: {llbMaNv.Text}", contentFont));
            pdfDoc.Add(new Paragraph($"Tên NV: {lbTenNv.Text}", contentFont));
            pdfDoc.Add(new Paragraph($"Tổng Tiền: {lbTongTien.Text}", contentFont));
            pdfDoc.Add(new Paragraph(" "));

            PdfPTable table = new PdfPTable(dgvSanPham.Columns.Count)
            {
                WidthPercentage = 100
            };

            foreach (DataGridViewColumn column in dgvSanPham.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, contentFont))
                {
                    BackgroundColor = new BaseColor(240, 240, 240)
                };
                table.AddCell(cell);
            }

            foreach (DataGridViewRow row in dgvSanPham.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    table.AddCell(new Phrase(cell.Value?.ToString() ?? "", contentFont));
                }
            }

            pdfDoc.Add(table);
            pdfDoc.Close();
            writer.Close();

            MessageBox.Show("PDF đã được xuất thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (File.Exists(pdfFilePath))
            {
                System.Diagnostics.Process.Start(pdfFilePath);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (Session.QuyenAdmin == false)
            {
                MessageBox.Show("Bạn không có quyền xóa hóa đơn này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn này?", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            DataTable chitiet = hoaDonBanDAL.LayChiTietHoaDon(soHDB);

            List<ProductDTO> products = new List<ProductDTO>();

            if (chitiet != null)
            {
                foreach (DataRow row in chitiet.Rows)
                {
                    int maHang = int.Parse(row["MaHang"].ToString());
                    int soLuong = int.Parse(row["SoLuong"].ToString());
                    var product = productDAL.GetProductById(maHang);
                    product.SoLuong += soLuong;
                    products.Add(product);
                }
            }

            if (hoaDonBanDAL.DeleteHoaDonBan(soHDB))
            {
                foreach(var p in products)
                {
                    productDAL.UpdateProduct(p);
                }
                OnDeleted?.Invoke(this, EventArgs.Empty);
                this.Close();

            }
            else
            {
                MessageBox.Show("Xóa không thành công");
            }
        }
    }
}
