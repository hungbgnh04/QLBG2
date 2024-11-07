using ClosedXML.Excel;
using Guna.UI2.WinForms;
using QLBG.DAL;
using QLBG.DTO;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QLBG.Views.SanPham
{
    public partial class pageDacDiem : UserControl
    {
        private DacDiemDAL dacDiemDAL;

        public pageDacDiem()
        {
            InitializeComponent();
            dacDiemDAL = new DacDiemDAL();
            LoadData();
            dgvDanhSach.CellClick += dgvCellClick;
        }

        public void LoadData()
        {
            List<DacDiemDTO> dacDiemList = dacDiemDAL.GetAllDacDiem();
            lbSoLuong.Text = dacDiemList.Count.ToString();
            dgvDanhSach.Rows.Clear();
            foreach (var dacDiem in dacDiemList)
            {
                dgvDanhSach.Rows.Add(dacDiem.MaDacDiem, dacDiem.TenDacDiem);
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
                            var updatedDacDiem = new DacDiemDTO
                            {
                                MaDacDiem = Convert.ToInt32(row.Cells[0].Value),
                                TenDacDiem = result.Cells[1].Value.ToString()
                            };
                            if (!dacDiemDAL.UpdateDacDiem(updatedDacDiem))
                            {
                                MessageBox.Show("Cập nhật thất bại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                LoadData();
                            }
                            break;
                        case frmThuocTinhSanPham.Mode.Delete:
                            int maDacDiem = Convert.ToInt32(row.Cells[0].Value);
                            if (dacDiemDAL.DeleteDacDiem(maDacDiem))
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
                    var newDacDiem = new DacDiemDTO
                    {
                        MaDacDiem = 0,
                        TenDacDiem = newRow.Cells[1].Value.ToString()
                    };
                    if (dacDiemDAL.InsertDacDiem(newDacDiem))
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
                saveFileDialog.FileName = "DacDiem.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("DacDiem");

                        worksheet.Cell(1, 1).Value = "MaDacDiem";
                        worksheet.Cell(1, 2).Value = "TenDacDiem";

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
            List<DacDiemDTO> dacDiemList = dacDiemDAL.GetAllDacDiem();

            var filteredList = dacDiemList.FindAll(dacDiem =>
                dacDiem.TenDacDiem.ToLower().Contains(searchText) ||
                dacDiem.MaDacDiem.ToString().Contains(searchText));

            dgvDanhSach.Rows.Clear();
            foreach (var dacDiem in filteredList)
            {
                dgvDanhSach.Rows.Add(dacDiem.MaDacDiem, dacDiem.TenDacDiem);
            }

            if (filteredList.Count == 0)
            {
                MessageBox.Show("Không tìm thấy kết quả nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
