using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using QLBG.DAL;

namespace QLBG.Views.NhanVien
{
    public partial class NhanVien : UserControl
    {
        private readonly DatabaseHelper dbHelper;

        public NhanVien()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            InitializeDataGridView();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RoundCorners(this, 60);
            LoadEmployeeData();

            lblSoLuongNhanVien.Text = $"{guna2DataGridView1.Rows.Count}";
            comboBoxSortBy.Items.AddRange(new string[]
            {
                "MaNV", "TenNV", "GioiTinh",
                "NgaySinh", "DienThoai", "DiaChi", "TenCV"
            });
            comboBoxSortBy.SelectedIndex = 0;

            textBoxTenDeTimKiem.KeyDown += textBoxTenDeTimKiem_KeyDown;
            comboBoxSortBy.SelectedIndexChanged += ComboBoxSortBy_SelectedIndexChanged;
        }

        private void RoundCorners(Control control, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(control.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(control.Width - radius, control.Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, control.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            control.Region = new Region(path);
        }

        private void InitializeDataGridView()
        {
            guna2DataGridView1.Columns.Clear();

            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaNV", HeaderText = "Mã Nhân Viên" });
            guna2DataGridView1.Columns.Add(new DataGridViewImageColumn
            {
                Name = "Anh",
                HeaderText = "Ảnh của nhân viên",
                ImageLayout = DataGridViewImageCellLayout.Zoom
            });
            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenNV", HeaderText = "Tên Nhân Viên" });
            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "GioiTinh", HeaderText = "Giới Tính" });
            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "NgaySinh", HeaderText = "Ngày Sinh" });
            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "DienThoai", HeaderText = "Điện Thoại" });
            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "DiaChi", HeaderText = "Địa Chỉ" });
            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenCV", HeaderText = "Công Việc" });

            // Thêm cột hình ảnh "Xem Chi Tiết"
            DataGridViewImageColumn viewImageColumn = new DataGridViewImageColumn
            {
                Name = "View",
                HeaderText = "Xem Chi Tiết thông tin",
                Image = global::QLBG.Properties.Resources.eye, // Đảm bảo hình ảnh được thêm vào tài nguyên dự án
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            guna2DataGridView1.Columns.Add(viewImageColumn);

            // Đăng ký sự kiện CellContentClick
            guna2DataGridView1.CellContentClick += guna2DataGridView1_CellContentClick;
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string columnName = guna2DataGridView1.Columns[e.ColumnIndex].Name;

                if (columnName == "View")
                {
                    // Lấy thông tin nhân viên từ hàng được nhấp
                    string maNV = guna2DataGridView1.Rows[e.RowIndex].Cells["MaNV"].Value?.ToString();
                    if (!string.IsNullOrEmpty(maNV))
                    {
                        // Hiển thị chi tiết nhân viên (ví dụ: mở một form chi tiết)
                        ChiTietNhanVien chiTietForm = new ChiTietNhanVien(maNV);
                        chiTietForm.EmployeeUpdated += (s, args) => LoadEmployeeData(); // Đăng ký sự kiện
                        chiTietForm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Mã nhân viên không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Xử lý các cột khác nếu cần
                    string cellValue = guna2DataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "null";

                    MessageBox.Show($"Clicked on column '{columnName}' at row {e.RowIndex}. Value: {cellValue}",
                                    "Cell Click Event",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
            }
        }


        private void LoadEmployeeData()
        {
            DataTable employees = dbHelper.GetEmployeesWithJob();
            guna2DataGridView1.Rows.Clear();

            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string imageDirectory = Path.Combine(projectDirectory, "Resources", "EmployeeImages");
            string defaultImagePath = Path.Combine(imageDirectory, "ic_user.png");

            foreach (DataRow row in employees.Rows)
            {
                int rowIndex = guna2DataGridView1.Rows.Add();
                DataGridViewRow dgvRow = guna2DataGridView1.Rows[rowIndex];

                dgvRow.Cells["MaNV"].Value = row["MaNV"].ToString();
                string imageName = row["Anh"].ToString();
                string imagePath = Path.Combine(imageDirectory, imageName);

                if (!string.IsNullOrEmpty(imageName) && File.Exists(imagePath))
                {
                    dgvRow.Cells["Anh"].Value = Image.FromFile(imagePath);
                }
                else if (File.Exists(defaultImagePath))
                {
                    dgvRow.Cells["Anh"].Value = Image.FromFile(defaultImagePath);
                }
                else
                {
                    dgvRow.Cells["Anh"].Value = DBNull.Value;
                }

                dgvRow.Cells["TenNV"].Value = row["TenNV"].ToString();
                dgvRow.Cells["GioiTinh"].Value = Convert.ToBoolean(row["GioiTinh"]) ? "Nam" : "Nữ";
                dgvRow.Cells["NgaySinh"].Value = Convert.ToDateTime(row["NgaySinh"]).ToString("dd/MM/yyyy");
                dgvRow.Cells["DienThoai"].Value = row["DienThoai"].ToString();
                dgvRow.Cells["DiaChi"].Value = row["DiaChi"].ToString();
                dgvRow.Cells["TenCV"].Value = row["TenCV"].ToString();
            }
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
                saveFileDialog.FileName = "EmployeeData.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Employees");

                        for (int i = 0; i < guna2DataGridView1.Columns.Count; i++)
                        {
                            // Bỏ qua cột hình ảnh "Anh" và cột "View"
                            if (guna2DataGridView1.Columns[i].Name == "Anh" || guna2DataGridView1.Columns[i].Name == "View")
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
                                if (guna2DataGridView1.Columns[j].Name == "Anh" || guna2DataGridView1.Columns[j].Name == "View")
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

        private void btnTaoNhanVien_Click(object sender, EventArgs e)
        {
            ThemNhanVien taoNhanVienForm = new ThemNhanVien();
            taoNhanVienForm.NhanVienAdded += (s, args) => LoadEmployeeData();
            taoNhanVienForm.ShowDialog();
        }

        private void btnTimKiemTheoTen_Click(object sender, EventArgs e)
        {
            string searchName = textBoxTenDeTimKiem.Text.Trim().ToLower();

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                row.Visible = string.IsNullOrEmpty(searchName) ||
                              (row.Cells["TenNV"].Value != null &&
                               row.Cells["TenNV"].Value.ToString().ToLower().Contains(searchName));
            }

            lblSoLuongNhanVien.Text = $"{guna2DataGridView1.Rows.Cast<DataGridViewRow>().Count(r => r.Visible)}";
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
