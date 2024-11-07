using ClosedXML.Excel;
using Guna.UI2.WinForms;
using QLBG.DAL;
using QLBG.DTO;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QLBG.Views.SanPham
{
    public partial class pageNhaSX : UserControl
    {
        private NhaSanXuatDAL nhaSanXuatDAL;

        public pageNhaSX()
        {
            InitializeComponent();
            nhaSanXuatDAL = new NhaSanXuatDAL();
            LoadData();
            dgvDanhSach.CellClick += dgvCellClick;
        }

        public void LoadData()
        {
            List<NhaSanXuatDTO> nhaSanXuatList = nhaSanXuatDAL.GetAllNhaSanXuat();
            lbSoLuong.Text = nhaSanXuatList.Count.ToString();
            dgvDanhSach.Rows.Clear();
            foreach (var nhaSanXuat in nhaSanXuatList)
            {
                dgvDanhSach.Rows.Add(nhaSanXuat.MaNSX, nhaSanXuat.TenNSX);
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
                            var updatedNhaSanXuat = new NhaSanXuatDTO
                            {
                                MaNSX = Convert.ToInt32(row.Cells[0].Value),
                                TenNSX = result.Cells[1].Value.ToString()
                            };
                            if (!nhaSanXuatDAL.UpdateNhaSanXuat(updatedNhaSanXuat))
                            {
                                MessageBox.Show("Cập nhật thất bại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                LoadData();
                            }
                            break;
                        case frmThuocTinhSanPham.Mode.Delete:
                            int maNSX = Convert.ToInt32(row.Cells[0].Value);
                            if (nhaSanXuatDAL.DeleteNhaSanXuat(maNSX))
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
                    var newNhaSanXuat = new NhaSanXuatDTO
                    {
                        MaNSX = 0,
                        TenNSX = newRow.Cells[1].Value.ToString()
                    };
                    if (nhaSanXuatDAL.InsertNhaSanXuat(newNhaSanXuat))
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
                saveFileDialog.FileName = "NhaSanXuat.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("NhaSanXuat");

                        worksheet.Cell(1, 1).Value = "MaNSX";
                        worksheet.Cell(1, 2).Value = "TenNSX";

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
            List<NhaSanXuatDTO> nhaSanXuatList = nhaSanXuatDAL.GetAllNhaSanXuat();

            var filteredList = nhaSanXuatList.FindAll(nhaSanXuat =>
                nhaSanXuat.TenNSX.ToLower().Contains(searchText) ||
                nhaSanXuat.MaNSX.ToString().Contains(searchText));

            dgvDanhSach.Rows.Clear();
            foreach (var nhaSanXuat in filteredList)
            {
                dgvDanhSach.Rows.Add(nhaSanXuat.MaNSX, nhaSanXuat.TenNSX);
            }

            if (filteredList.Count == 0)
            {
                MessageBox.Show("Không tìm thấy kết quả nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
