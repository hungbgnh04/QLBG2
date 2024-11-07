using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using QLBG.DAL;

namespace QLBG.Views.CongViec
{
    public partial class CongViec : UserControl
    {
        private readonly CongViecDAL congViecDAL;

        public CongViec()
        {
            InitializeComponent();
            congViecDAL = new CongViecDAL();
            InitializeDataGridView();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadCongViecData();

            // Initialize ComboBox for sorting
            comboBoxSortBy.Items.AddRange(new string[] { "MaCV", "TenCV", "MucLuong" });
            comboBoxSortBy.SelectedIndex = 0;

            textBoxTenDeTimKiem.KeyDown += textBoxTenDeTimKiem_KeyDown;
            comboBoxSortBy.SelectedIndexChanged += ComboBoxSortBy_SelectedIndexChanged;
        }

        private void InitializeDataGridView()
        {
            guna2DataGridView1.Columns.Clear();

            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaCV", HeaderText = "Mã Công Việc" });
            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenCV", HeaderText = "Tên Công Việc" });
            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "MucLuong", HeaderText = "Mức Lương" });

            // Add "View Details" column with an eye icon
            DataGridViewImageColumn viewImageColumn = new DataGridViewImageColumn
            {
                Name = "View",
                HeaderText = "Xem Chi Tiết Công Việc",
                Image = global::QLBG.Properties.Resources.eye, // Assuming an eye icon is available in resources
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
                    int maCV = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["MaCV"].Value);
                    ChiTietCongViec chiTietForm = new ChiTietCongViec(maCV);
                    chiTietForm.CongViecUpdated += (s, args) => LoadCongViecData();
                    chiTietForm.ShowDialog();
                }
            }
        }

        private void LoadCongViecData()
        {
            DataTable jobs = congViecDAL.GetAllCongViec();
            guna2DataGridView1.Rows.Clear();

            foreach (DataRow row in jobs.Rows)
            {
                int rowIndex = guna2DataGridView1.Rows.Add();
                DataGridViewRow dgvRow = guna2DataGridView1.Rows[rowIndex];

                dgvRow.Cells["MaCV"].Value = row["MaCV"];
                dgvRow.Cells["TenCV"].Value = row["TenCV"];
                dgvRow.Cells["MucLuong"].Value = row["MucLuong"];
            }

            lblSoLuongCongViec.Text = $"{guna2DataGridView1.Rows.Count}";
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
                saveFileDialog.FileName = "JobData.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Jobs");

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

        private void btnTaoCongViec_Click(object sender, EventArgs e)
        {
            ThemCongViec themCongViecForm = new ThemCongViec();
            themCongViecForm.CongViecAdded += (s, args) => LoadCongViecData();
            themCongViecForm.ShowDialog();
        }

        private void btnTimKiemTheoTen_Click(object sender, EventArgs e)
        {
            string searchName = textBoxTenDeTimKiem.Text.Trim().ToLower();

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                row.Visible = string.IsNullOrEmpty(searchName) ||
                              (row.Cells["TenCV"].Value != null &&
                               row.Cells["TenCV"].Value.ToString().ToLower().Contains(searchName));
            }
            lblSoLuongCongViec.Text = $"{guna2DataGridView1.Rows.Cast<DataGridViewRow>().Count(r => r.Visible)}";
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
