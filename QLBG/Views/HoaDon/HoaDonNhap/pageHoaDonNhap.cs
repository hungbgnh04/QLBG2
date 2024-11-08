using ClosedXML.Excel;
using Guna.UI2.WinForms;
using QLBG.DAL;
using QLBG.Views.NhanVien;
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

namespace QLBG.Views.HoaDon.HoaDonNhap
{
    public partial class pageHoaDonNhap : UserControl
    {
        private HoaDonNhapDAL HDNDAL;

        public pageHoaDonNhap()
        {
            InitializeComponent();
            HDNDAL = new HoaDonNhapDAL();
            InitDGV();
        }

        private void InitDGV()
        {
            dgvDanhSach.Columns.Clear();

            dgvDanhSach.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoHDN", HeaderText = "Số hóa đơn" });
            dgvDanhSach.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenNV", HeaderText = "Tên nhân viên" });
            dgvDanhSach.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenNCC", HeaderText = "Tên nhà cung cấp" });
            dgvDanhSach.Columns.Add(new DataGridViewTextBoxColumn { Name = "NgayNhap", HeaderText = "Ngày Nhập" });
            dgvDanhSach.Columns.Add(new DataGridViewTextBoxColumn { Name = "TongTien", HeaderText = "Tổng tiền" });

            DataGridViewImageColumn viewImageColumn = new DataGridViewImageColumn
            {
                Name = "View",
                HeaderText = "Xem Chi Tiết thông tin",
                Image = global::QLBG.Properties.Resources.eye,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            dgvDanhSach.Columns.Add(viewImageColumn);

            dgvDanhSach.CellContentClick += dgvDanhSach_CellContentClick;
        }

        private void dgvDanhSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string columnName = dgvDanhSach.Columns[e.ColumnIndex].Name;

                if (columnName == "View")
                {
                    try
                    {
                        int SoHDN = Int32.Parse(dgvDanhSach.Rows[e.RowIndex].Cells["SoHDN"].Value.ToString());
                        ChiTietHoaDon frm = new ChiTietHoaDon(SoHDN);
                        frm.OnDeleted += (s, args) => LoadData();
                        frm.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Số hóa đơn nhập không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadData();

            lblSoLuong.Text = $"{dgvDanhSach.Rows.Count}";
            comboBoxSortBy.Items.AddRange(new string[]
            {
                "SoHDN", "TenNV", "NgayNhap",
                "TenNCC", "TongTien"
            });
            comboBoxSortBy.SelectedIndex = 0;

            textBoxTimKiem.KeyDown += textBoxTimKiem_KeyDown;
            comboBoxSortBy.SelectedIndexChanged += ComboBoxSortBy_SelectedIndexChanged;
        }

        private void ComboBoxSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sortBy = comboBoxSortBy.SelectedItem.ToString();
            DataGridViewColumn columnToSort = dgvDanhSach.Columns[sortBy];

            if (columnToSort != null)
            {
                dgvDanhSach.Sort(columnToSort, System.ComponentModel.ListSortDirection.Ascending);
            }
            else
            {
                MessageBox.Show("Không tìm thấy cột để sắp xếp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Save an Excel File";
                saveFileDialog.FileName = "HoaDonNhapData.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("HoaDonNha");

                        for (int i = 0; i < dgvDanhSach.Columns.Count; i++)
                        {
                            if (dgvDanhSach.Columns[i].Name == "Anh" || dgvDanhSach.Columns[i].Name == "View")
                                continue;

                            worksheet.Cell(1, i + 1).Value = dgvDanhSach.Columns[i].HeaderText;
                        }

                        int excelRow = 2;
                        foreach (DataGridViewRow row in dgvDanhSach.Rows)
                        {
                            if (row.IsNewRow) continue;

                            int excelCol = 1;
                            for (int j = 0; j < dgvDanhSach.Columns.Count; j++)
                            {
                                if (dgvDanhSach.Columns[j].Name == "Anh" || dgvDanhSach.Columns[j].Name == "View")
                                    continue;

                                var cellValue = row.Cells[j].Value;
                                worksheet.Cell(excelRow, excelCol).Value = cellValue is DBNull ? "" : cellValue.ToString();
                                excelCol++;
                            }
                            excelRow++;
                        }

                        workbook.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show("Xuất dữ liệu ra Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        public void LoadData()
        {
            DataTable table = HDNDAL.GetAllHoaDonNhapWithNCCAndNV();
            dgvDanhSach.Rows.Clear();

            foreach (DataRow row in table.Rows)
            {
                int rowIndex = dgvDanhSach.Rows.Add();
                DataGridViewRow dgvRow = dgvDanhSach.Rows[rowIndex];

                dgvRow.Cells["SoHDN"].Value = row["SoHDN"].ToString();
                dgvRow.Cells["TenNV"].Value = row["TenNV"].ToString();
                dgvRow.Cells["TenNCC"].Value = row["TenNCC"].ToString();
                dgvRow.Cells["NgayNhap"].Value = Convert.ToDateTime(row["NgayNhap"]).ToString("dd/MM/yyyy");
                dgvRow.Cells["TongTien"].Value = row["TongTien"].ToString();
            }
        }

        private void btnTao_Click(object sender, EventArgs e)
        {
            frmTaoHoaDon frm = new frmTaoHoaDon();
            frm.HoaDonAdded += (s, args) => LoadData();
            frm.ShowDialog();
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Save an Excel File";
                saveFileDialog.FileName = "HoaDonNhap.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("HDN");

                        for (int i = 0; i < dgvDanhSach.Columns.Count; i++)
                        {
                            if (dgvDanhSach.Columns[i].Name == "Anh" || dgvDanhSach.Columns[i].Name == "View")
                                continue;

                            worksheet.Cell(1, i + 1).Value = dgvDanhSach.Columns[i].HeaderText;
                        }

                        int excelRow = 2;
                        foreach (DataGridViewRow row in dgvDanhSach.Rows)
                        {
                            if (row.IsNewRow) continue;

                            int excelCol = 1;
                            for (int j = 0; j < dgvDanhSach.Columns.Count; j++)
                            {
                                if (dgvDanhSach.Columns[j].Name == "Anh" || dgvDanhSach.Columns[j].Name == "View")
                                    continue;

                                var cellValue = row.Cells[j].Value;
                                worksheet.Cell(excelRow, excelCol).Value = cellValue is DBNull ? "" : cellValue.ToString();
                                excelCol++;
                            }
                            excelRow++;
                        }

                        workbook.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show("Xuất dữ liệu ra Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
    }
}
