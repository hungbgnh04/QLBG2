using ClosedXML.Excel;
using Guna.UI2.WinForms;
using QLBG.DAL;
using QLBG.Views.HoaDon.HoaDonban;
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

namespace QLBG.Views.HoaDon.HoaDonBan
{
    public partial class pageHoaDonBan : UserControl
    {
        private HoaDonBanDAL HDNDAL;

        public pageHoaDonBan()
        {
            InitializeComponent();
            HDNDAL = new HoaDonBanDAL();
            InitDGV();
        }

        private void InitDGV()
        {
            dgvDanhSach.Columns.Clear();

            dgvDanhSach.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoHDB", HeaderText = "Số hóa đơn" });
            dgvDanhSach.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenNV", HeaderText = "Tên nhân viên" });
            dgvDanhSach.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenKhach", HeaderText = "Tên khách hàng" });
            dgvDanhSach.Columns.Add(new DataGridViewTextBoxColumn { Name = "NgayBan", HeaderText = "Ngày Bán" });
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
                        int SoHDB = Int32.Parse(dgvDanhSach.Rows[e.RowIndex].Cells["SoHDB"].Value.ToString());
                        ChiTietHoaDon frm = new ChiTietHoaDon(SoHDB);
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
                "SoHDN", "TenNV", "NgayBan",
                "TenKH", "TongTien"
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
                saveFileDialog.FileName = "HoaDonBanData.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("HoaDonBan");

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
            DataTable table = HDNDAL.GetAllInvoiceWithAttributeName();
            dgvDanhSach.Rows.Clear();

            foreach (DataRow row in table.Rows)
            {
                int rowIndex = dgvDanhSach.Rows.Add();
                DataGridViewRow dgvRow = dgvDanhSach.Rows[rowIndex];

                dgvRow.Cells["SoHDB"].Value = row["SoHDB"].ToString();
                dgvRow.Cells["TenNV"].Value = row["TenNV"].ToString();
                dgvRow.Cells["TenKhach"].Value = row["TenKhach"].ToString();
                dgvRow.Cells["NgayBan"].Value = Convert.ToDateTime(row["NgayBan"]).ToString("dd/MM/yyyy");
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
                saveFileDialog.FileName = "HoaDonBan.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("HDB");

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
