﻿using iTextSharp.text.pdf;
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
            // Define the PDF file path
            string pdfFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"HoaDonNhap_{soHDN}.pdf");

            // Load a Unicode font that supports Vietnamese characters
            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "tahoma.ttf"); // or specify the path to your .ttf file
            BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font titleFont = new iTextSharp.text.Font(bf, 18, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font contentFont = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL);

            // Create a PDF document
            Document pdfDoc = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(pdfFilePath, FileMode.Create));
            pdfDoc.Open();

            // Title
            Paragraph title = new Paragraph("Chi Tiết Hóa Đơn Nhập", titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20f
            };
            pdfDoc.Add(title);

            // Invoice details
            pdfDoc.Add(new Paragraph($"Số HĐN: {SHDLb.Text}", contentFont));
            pdfDoc.Add(new Paragraph($"Nhà Cung Cấp: {lbTenNCC.Text}", contentFont));
            pdfDoc.Add(new Paragraph($"Địa Chỉ: {lbDiaChi.Text}", contentFont));
            pdfDoc.Add(new Paragraph($"SĐT: {lbSDT.Text}", contentFont));
            pdfDoc.Add(new Paragraph($"Ngày Nhập: {lbNgayNhap.Text}", contentFont));
            pdfDoc.Add(new Paragraph($"Mã NV: {llbMaNv.Text}", contentFont));
            pdfDoc.Add(new Paragraph($"Tên NV: {lbTenNv.Text}", contentFont));
            pdfDoc.Add(new Paragraph($"Tổng Tiền: {lbTongTien.Text}", contentFont));
            pdfDoc.Add(new Paragraph(" ")); // Line break

            // Create a table for the invoice details
            PdfPTable table = new PdfPTable(dgvSanPham.Columns.Count)
            {
                WidthPercentage = 100
            };

            // Add header cells
            foreach (DataGridViewColumn column in dgvSanPham.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, contentFont))
                {
                    BackgroundColor = new BaseColor(240, 240, 240)
                };
                table.AddCell(cell);
            }

            // Add rows
            foreach (DataGridViewRow row in dgvSanPham.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    table.AddCell(new Phrase(cell.Value?.ToString() ?? "", contentFont));
                }
            }

            pdfDoc.Add(table);

            // Close the PDF document
            pdfDoc.Close();
            writer.Close();

            MessageBox.Show("PDF đã được xuất thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Open the PDF after exporting
            if (File.Exists(pdfFilePath))
            {
                System.Diagnostics.Process.Start(pdfFilePath);
            }
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            hoaDonNhapDAL.DeleteHoaDonNhap(soHDN);
            OnDeleted?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        private void dgvSanPham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
