using ClosedXML.Excel;
using Guna.UI2.WinForms;
using QLBG.DAL;
using QLBG.DTO;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QLBG.Views.SanPham
{
    public partial class pageNuocSX : UserControl
    {
        private NuocSXDAL nuocSXDAL;

        public pageNuocSX()
        {
            InitializeComponent();
            nuocSXDAL = new NuocSXDAL();
            LoadData();
            dgvDanhSach.CellClick += dgvCellClick;
        }

        public void LoadData()
        {
            List<NuocSXDTO> nuocSXList = nuocSXDAL.GetAllNuocSX();
            lbSoLuong.Text = nuocSXList.Count.ToString();
            dgvDanhSach.Rows.Clear();
            foreach (var nuocSX in nuocSXList)
            {
                dgvDanhSach.Rows.Add(nuocSX.MaNuocSX, nuocSX.TenNuocSX);
            }
        }

        private void dgvCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var dgv = (Guna2DataGridView)sender;
            var row = dgv.Rows[e.RowIndex];

            using (var frmThuocTinh = new frmThuocTinhSanPham(row, frmThuocTinhSanPham.Mode.View))
            {
                if (frmThuocTinh.ShowDialog() == DialogResult.OK)
                {
                    var result = frmThuocTinh.row;
                    switch (frmThuocTinh.mode)
                    {
                        case frmThuocTinhSanPham.Mode.Edit:
                            row.Cells[1].Value = result.Cells[1].Value;
                            var updatedNuocSX = new NuocSXDTO
                            {
                                MaNuocSX = Convert.ToInt32(row.Cells[0].Value),
                                TenNuocSX = result.Cells[1].Value.ToString()
                            };
                            if (!nuocSXDAL.UpdateNuocSX(updatedNuocSX))
                            {
                                MessageBox.Show("Cập nhật thất bại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                LoadData();
                            }
                            break;
                        case frmThuocTinhSanPham.Mode.Delete:
                            int maNuocSX = Convert.ToInt32(row.Cells[0].Value);
                            if (nuocSXDAL.DeleteNuocSX(maNuocSX))
                            {
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Xóa thất bại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            break;
                        case frmThuocTinhSanPham.Mode.View:
                            break;
                    }
                }
            }
        }

        private void btnTao_Click(object sender, EventArgs e)
        {
            DataGridViewRow newRow = dgvDanhSach.Rows[dgvDanhSach.Rows.Add()];
            using (frmThuocTinhSanPham form = new frmThuocTinhSanPham(newRow, frmThuocTinhSanPham.Mode.Add))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    newRow = form.row;
                    var newNuocSX = new NuocSXDTO
                    {
                        MaNuocSX = 0,
                        TenNuocSX = newRow.Cells[1].Value.ToString()
                    };
                    if (nuocSXDAL.InsertNuocSX(newNuocSX))
                    {
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Thêm mới thất bại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Save an Excel File";
                saveFileDialog.FileName = "NuocSX.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("NuocSX");

                        worksheet.Cell(1, 1).Value = "MaNuocSX";
                        worksheet.Cell(1, 2).Value = "TenNuocSX";

                        int excelRow = 2;
                        foreach (DataGridViewRow row in dgvDanhSach.Rows)
                        {
                            if (row.IsNewRow) continue;

                            worksheet.Cell(excelRow, 1).Value = row.Cells[0].Value?.ToString();
                            worksheet.Cell(excelRow, 2).Value = row.Cells[1].Value?.ToString();
                            excelRow++;
                        }

                        workbook.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show("Xuất dữ liệu ra Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string searchText = txtTim.Text.Trim().ToLower();
            List<NuocSXDTO> nuocSXList = nuocSXDAL.GetAllNuocSX();

            var filteredList = nuocSXList.FindAll(nuocSX =>
                nuocSX.TenNuocSX.ToLower().Contains(searchText) ||
                nuocSX.MaNuocSX.ToString().Contains(searchText));

            dgvDanhSach.Rows.Clear();
            foreach (var nuocSX in filteredList)
            {
                dgvDanhSach.Rows.Add(nuocSX.MaNuocSX, nuocSX.TenNuocSX);
            }

            if (filteredList.Count == 0)
            {
                MessageBox.Show("Không tìm thấy kết quả nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
