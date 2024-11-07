using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using QLBG.DAL;

namespace QLBG.Views.KhachHang
{
    public partial class KhachHang : UserControl
    {
        private readonly KhachHangDAL khachHangDAL;

        public KhachHang()
        {
            InitializeComponent();
            khachHangDAL = new KhachHangDAL();
            InitializeDataGridView();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadKhachHangData();

            // Khởi tạo ComboBox cho sắp xếp
            comboBoxSortBy.Items.AddRange(new string[]
            {
                "MaKhach", "TenKhach", "DiaChi", "DienThoai"
            });
            comboBoxSortBy.SelectedIndex = 0;

            textBoxTenDeTimKiem.KeyDown += textBoxTenDeTimKiem_KeyDown;
            comboBoxSortBy.SelectedIndexChanged += ComboBoxSortBy_SelectedIndexChanged;
        }

        private void InitializeDataGridView()
        {
            guna2DataGridView1.Columns.Clear();

            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaKhach", HeaderText = "Mã Khách Hàng" });
            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenKhach", HeaderText = "Tên Khách Hàng" });
            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "DiaChi", HeaderText = "Địa Chỉ" });
            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "DienThoai", HeaderText = "Điện Thoại" });

            // Thêm cột hình ảnh "Xem Chi Tiết" với biểu tượng mắt
            DataGridViewImageColumn viewImageColumn = new DataGridViewImageColumn
            {
                Name = "View",
                HeaderText = "Xem Chi Tiết Thông TIn Khách Hàng",
                Image = global::QLBG.Properties.Resources.eye, // Biểu tượng mắt từ tài nguyên dự án
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            guna2DataGridView1.Columns.Add(viewImageColumn);

            guna2DataGridView1.CellContentClick += guna2DataGridView1_CellContentClick;
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string columnName = guna2DataGridView1.Columns[e.ColumnIndex].Name;

                if (columnName == "View")
                {
                    int maKhach = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["MaKhach"].Value);
                    ChiTietKhachHang chiTietForm = new ChiTietKhachHang(maKhach);
                    chiTietForm.KhachHangUpdated += (s, args) => LoadKhachHangData(); // Cập nhật lại danh sách sau khi chỉnh sửa
                    chiTietForm.ShowDialog();
                }
            }
        }


        private void LoadKhachHangData()
        {
            DataTable customers = khachHangDAL.GetAllKhachHang();
            guna2DataGridView1.Rows.Clear();

            foreach (DataRow row in customers.Rows)
            {
                int rowIndex = guna2DataGridView1.Rows.Add();
                DataGridViewRow dgvRow = guna2DataGridView1.Rows[rowIndex];

                dgvRow.Cells["MaKhach"].Value = row["MaKhach"];
                dgvRow.Cells["TenKhach"].Value = row["TenKhach"];
                dgvRow.Cells["DiaChi"].Value = row["DiaChi"];
                dgvRow.Cells["DienThoai"].Value = row["DienThoai"];
            }

            lblSoLuongKhachHang.Text = $"{guna2DataGridView1.Rows.Count}";
        }

        private void ComboBoxSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sortBy = comboBoxSortBy.SelectedItem.ToString();
            DataGridViewColumn columnToSort = guna2DataGridView1.Columns[sortBy];

            if (columnToSort != null)
            {
                guna2DataGridView1.Sort(columnToSort, System.ComponentModel.ListSortDirection.Ascending);
            }
            else
            {
                MessageBox.Show("Không tìm thấy cột để sắp xếp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Save an Excel File";
                saveFileDialog.FileName = "CustomerData.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Customers");

                        for (int i = 0; i < guna2DataGridView1.Columns.Count; i++)
                        {
                            if (guna2DataGridView1.Columns[i].Name == "View")
                                continue;

                            worksheet.Cell(1, i + 1).Value = guna2DataGridView1.Columns[i].HeaderText;
                        }

                        int excelRow = 2;
                        foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                        {
                            if (row.IsNewRow) continue;

                            int excelCol = 1;
                            for (int j = 0; j < guna2DataGridView1.Columns.Count; j++)
                            {
                                if (guna2DataGridView1.Columns[j].Name == "View")
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

        private void btnTaoKhachHang_Click(object sender, EventArgs e)
        {
            ThemKhachHang taoKhachHangForm = new ThemKhachHang();
            taoKhachHangForm.KhachHangAdded += (s, args) => LoadKhachHangData();
            taoKhachHangForm.ShowDialog();
        }

        private void btnTimKiemTheoTen_Click(object sender, EventArgs e)
        {
            string searchName = textBoxTenDeTimKiem.Text.Trim().ToLower();

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                row.Visible = string.IsNullOrEmpty(searchName) ||
                              (row.Cells["TenKhach"].Value != null &&
                               row.Cells["TenKhach"].Value.ToString().ToLower().Contains(searchName));
            }
            lblSoLuongKhachHang.Text = $"{guna2DataGridView1.Rows.Cast<DataGridViewRow>().Count(r => r.Visible)}";
        }

        private void textBoxTenDeTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTimKiemTheoTen_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
