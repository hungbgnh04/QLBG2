using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using QLBG.DAL;

namespace QLBG.Views.NhaCungCap
{
    public partial class NhaCungCap : UserControl
    {
        private readonly NhaCungCapDAL nhaCungCapDAL;

        public NhaCungCap()
        {
            InitializeComponent();
            nhaCungCapDAL = new NhaCungCapDAL();
            InitializeDataGridView();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadNhaCungCapData();

            comboBoxSortBy.Items.AddRange(new string[] { "MaNCC", "TenNCC", "DiaChi", "DienThoai" });
            comboBoxSortBy.SelectedIndex = 0;

            textBoxTenDeTimKiem.KeyDown += textBoxTenDeTimKiem_KeyDown;
            comboBoxSortBy.SelectedIndexChanged += ComboBoxSortBy_SelectedIndexChanged;
        }

        private void InitializeDataGridView()
        {
            guna2DataGridView1.Columns.Clear();
            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaNCC", HeaderText = "Mã NCC" });
            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenNCC", HeaderText = "Tên NCC" });
            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "DiaChi", HeaderText = "Địa Chỉ" });
            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "DienThoai", HeaderText = "Điện Thoại" });

            DataGridViewImageColumn viewImageColumn = new DataGridViewImageColumn
            {
                Name = "View",
                HeaderText = "Xem Chi Tiết",
                Image = global::QLBG.Properties.Resources.eye,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            guna2DataGridView1.Columns.Add(viewImageColumn);

            guna2DataGridView1.CellContentClick += guna2DataGridView1_CellContentClick;
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && guna2DataGridView1.Columns[e.ColumnIndex].Name == "View")
            {
                int maNCC = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["MaNCC"].Value);
                ChiTietNhaCungCap chiTietForm = new ChiTietNhaCungCap(maNCC);
                chiTietForm.NhaCungCapUpdated += (s, args) => LoadNhaCungCapData();
                chiTietForm.ShowDialog();
            }
        }

        private void LoadNhaCungCapData()
        {
            DataTable suppliers = nhaCungCapDAL.GetAllNhaCungCap();
            guna2DataGridView1.Rows.Clear();

            foreach (DataRow row in suppliers.Rows)
            {
                int rowIndex = guna2DataGridView1.Rows.Add();
                DataGridViewRow dgvRow = guna2DataGridView1.Rows[rowIndex];
                dgvRow.Cells["MaNCC"].Value = row["MaNCC"];
                dgvRow.Cells["TenNCC"].Value = row["TenNCC"];
                dgvRow.Cells["DiaChi"].Value = row["DiaChi"];
                dgvRow.Cells["DienThoai"].Value = row["DienThoai"];
            }

            lblSoLuongNhaCungCap.Text = $"{guna2DataGridView1.Rows.Count}";
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
                saveFileDialog.FileName = "SupplierData.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Suppliers");
                        for (int i = 0; i < guna2DataGridView1.Columns.Count; i++)
                        {
                            worksheet.Cell(1, i + 1).Value = guna2DataGridView1.Columns[i].HeaderText;
                        }

                        int excelRow = 2;
                        foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                        {
                            if (row.IsNewRow) continue;

                            for (int j = 0; j < guna2DataGridView1.Columns.Count; j++)
                            {
                                var cellValue = row.Cells[j].Value?.ToString() ?? string.Empty;
                                worksheet.Cell(excelRow, j + 1).Value = cellValue;
                            }
                            excelRow++;
                        }

                        workbook.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show("Xuất dữ liệu ra Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnTaoNhaCungCap_Click(object sender, EventArgs e)
        {
            ThemNhaCungCap taoNhaCungCapForm = new ThemNhaCungCap();
            taoNhaCungCapForm.NhaCungCapAdded += (s, args) => LoadNhaCungCapData();
            taoNhaCungCapForm.ShowDialog();
        }

        private void btnTimKiemTheoTen_Click(object sender, EventArgs e)
        {
            string searchName = textBoxTenDeTimKiem.Text.Trim().ToLower();

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                row.Visible = string.IsNullOrEmpty(searchName) ||
                              (row.Cells["TenNCC"].Value != null &&
                               row.Cells["TenNCC"].Value.ToString().ToLower().Contains(searchName));
            }
            lblSoLuongNhaCungCap.Text = $"{guna2DataGridView1.Rows.Cast<DataGridViewRow>().Count(r => r.Visible)}";
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
