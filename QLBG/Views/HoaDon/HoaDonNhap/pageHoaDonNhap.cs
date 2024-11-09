using ClosedXML.Excel;
using Guna.UI2.WinForms;
using QLBG.DAL;
using QLBG.DTO;
using QLBG.Views.ReportRDLC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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

            // Cột Số Hóa Đơn
            DataGridViewTextBoxColumn soHDNColumn = new DataGridViewTextBoxColumn
            {
                Name = "SoHDN",
                HeaderText = "Số hóa đơn",
                ValueType = typeof(int)
            };
            dgvDanhSach.Columns.Add(soHDNColumn);

            // Cột Tên Nhân Viên
            DataGridViewTextBoxColumn tenNVColumn = new DataGridViewTextBoxColumn
            {
                Name = "TenNV",
                HeaderText = "Tên nhân viên",
                ValueType = typeof(string)
            };
            dgvDanhSach.Columns.Add(tenNVColumn);

            // Cột Tên Nhà Cung Cấp
            DataGridViewTextBoxColumn tenNCCColumn = new DataGridViewTextBoxColumn
            {
                Name = "TenNCC",
                HeaderText = "Tên nhà cung cấp",
                ValueType = typeof(string)
            };
            dgvDanhSach.Columns.Add(tenNCCColumn);

            // Cột Ngày Nhập
            DataGridViewTextBoxColumn ngayNhapColumn = new DataGridViewTextBoxColumn
            {
                Name = "NgayNhap",
                HeaderText = "Ngày Nhập",
                ValueType = typeof(DateTime)
            };
            dgvDanhSach.Columns.Add(ngayNhapColumn);

            // Cột Tổng Tiền
            DataGridViewTextBoxColumn tongTienColumn = new DataGridViewTextBoxColumn
            {
                Name = "TongTien",
                HeaderText = "Tổng tiền",
                ValueType = typeof(decimal)
            };
            dgvDanhSach.Columns.Add(tongTienColumn);

            // Cột View Chi Tiết
            DataGridViewImageColumn viewImageColumn = new DataGridViewImageColumn
            {
                Name = "View",
                HeaderText = "Xem Chi Tiết thông tin",
                Image = global::QLBG.Properties.Resources.eye,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            dgvDanhSach.Columns.Add(viewImageColumn);

            // Đăng ký sự kiện CellContentClick
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
                    catch (Exception)
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

            // Đảm bảo các mục trong ComboBox khớp với tên cột của DataGridView
            comboBoxSortBy.Items.AddRange(new string[]
            {
                "SoHDN", "TenNV", "NgayNhap", "TenNCC", "TongTien"
            });
            comboBoxSortBy.SelectedIndex = 0;

            // Đăng ký sự kiện TextChanged cho tìm kiếm
            textBoxTimKiem.TextChanged += textBoxTimKiem_TextChanged;
            // Đăng ký sự kiện SelectedIndexChanged cho sắp xếp
            comboBoxSortBy.SelectedIndexChanged += ComboBoxSortBy_SelectedIndexChanged;
        }

        private void ComboBoxSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sortBy = comboBoxSortBy.SelectedItem.ToString();
            DataGridViewColumn columnToSort = dgvDanhSach.Columns[sortBy];

            if (columnToSort != null)
            {
                // Xác định thứ tự sắp xếp hiện tại
                ListSortDirection direction = ListSortDirection.Ascending;
                if (dgvDanhSach.SortOrder == SortOrder.Ascending)
                {
                    direction = ListSortDirection.Descending;
                }

                dgvDanhSach.Sort(columnToSort, direction);
            }
            else
            {
                MessageBox.Show("Không tìm thấy cột để sắp xếp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxTimKiem_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = textBoxTimKiem.Text.Trim().ToLower();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                DataTable table = HDNDAL.GetAllHoaDonNhapWithNCCAndNV();
                var filteredRows = table.AsEnumerable().Where(row =>
                    row["SoHDN"].ToString().ToLower().Contains(searchTerm) ||
                    row["TenNV"].ToString().ToLower().Contains(searchTerm) ||
                    row["TenNCC"].ToString().ToLower().Contains(searchTerm) ||
                    row["NgayNhap"].ToString().ToLower().Contains(searchTerm) ||
                    row["TongTien"].ToString().ToLower().Contains(searchTerm));

                dgvDanhSach.Rows.Clear();

                foreach (var row in filteredRows)
                {
                    int rowIndex = dgvDanhSach.Rows.Add();
                    DataGridViewRow dgvRow = dgvDanhSach.Rows[rowIndex];

                    dgvRow.Cells["SoHDN"].Value = row["SoHDN"];
                    dgvRow.Cells["TenNV"].Value = row["TenNV"].ToString();
                    dgvRow.Cells["TenNCC"].Value = row["TenNCC"].ToString();
                    dgvRow.Cells["NgayNhap"].Value = Convert.ToDateTime(row["NgayNhap"]).ToString("dd/MM/yyyy");
                    dgvRow.Cells["TongTien"].Value = row["TongTien"];
                }
                lblSoLuong.Text = $"{dgvDanhSach.Rows.Count}";
            }
            else
            {
                LoadData();
            }
        }

        public void LoadData()
        {
            DataTable table = HDNDAL.GetAllHoaDonNhapWithNCCAndNV();
            dgvDanhSach.Rows.Clear();

            lblSoLuong.Text = $"{table.Rows.Count}";
            foreach (DataRow row in table.Rows)
            {
                int rowIndex = dgvDanhSach.Rows.Add();
                DataGridViewRow dgvRow = dgvDanhSach.Rows[rowIndex];

                dgvRow.Cells["SoHDN"].Value = row["SoHDN"];
                dgvRow.Cells["TenNV"].Value = row["TenNV"].ToString();
                dgvRow.Cells["TenNCC"].Value = row["TenNCC"].ToString();
                dgvRow.Cells["NgayNhap"].Value = Convert.ToDateTime(row["NgayNhap"]).ToString("dd/MM/yyyy");
                dgvRow.Cells["TongTien"].Value = row["TongTien"];
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

                        // Thêm tiêu đề cột
                        int excelColIndex = 1;
                        foreach (DataGridViewColumn column in dgvDanhSach.Columns)
                        {
                            if (column.Name == "View")
                                continue;

                            worksheet.Cell(1, excelColIndex).Value = column.HeaderText;
                            excelColIndex++;
                        }

                        // Thêm dữ liệu
                        int excelRow = 2;
                        foreach (DataGridViewRow row in dgvDanhSach.Rows)
                        {
                            if (row.IsNewRow) continue;

                            excelColIndex = 1;
                            foreach (DataGridViewColumn column in dgvDanhSach.Columns)
                            {
                                if (column.Name == "View")
                                    continue;

                                var cellValue = row.Cells[column.Name].Value;
                                worksheet.Cell(excelRow, excelColIndex).Value = cellValue is DBNull ? "" : cellValue.ToString();
                                excelColIndex++;
                            }
                            excelRow++;
                        }

                        workbook.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show("Xuất dữ liệu ra Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            HoaDonNhapDAL hoaDonNhapDAL = new HoaDonNhapDAL();
            List<HoaDonNhapDTO> hoaDonNhapList = hoaDonNhapDAL.GetHoaDonNhapSummary();
            DataTable dataTable = hoaDonNhapDAL.ConvertToDataTable(hoaDonNhapList);
            if (dataTable.Rows.Count > 0)
            {
                frmReport reportViewerForm = new frmReport();
                reportViewerForm.LoadReport(dataTable, "HoaDonNhap", "ReportHoaDonNhap.rdlc");
                reportViewerForm.Show();
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để hiển thị trong báo cáo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
