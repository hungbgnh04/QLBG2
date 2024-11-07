using ClosedXML.Excel;
using Guna.UI2.WinForms;
using QLBG.DAL;
using QLBG.DTO;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QLBG.Views.SanPham
{
    public partial class pageCongDung : UserControl
    {
        private CongDungDAL congDungDAL;

        public pageCongDung()
        {
            InitializeComponent();
            congDungDAL = new CongDungDAL();
            LoadData();
            dgvDanhSach.CellClick += dgvCellClick;
        }

        public void LoadData()
        {
            List<CongDungDTO> congDungList = congDungDAL.GetAllCongDung();
            lbSoLuong.Text = congDungList.Count.ToString();
            dgvDanhSach.Rows.Clear();
            foreach (var congDung in congDungList)
            {
                dgvDanhSach.Rows.Add(congDung.MaCongDung, congDung.TenCongDung);
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
                            var updatedCongDung = new CongDungDTO
                            {
                                MaCongDung = Convert.ToInt32(row.Cells[0].Value),
                                TenCongDung = result.Cells[1].Value.ToString()
                            };
                            if (!congDungDAL.UpdateCongDung(updatedCongDung))
                            {
                                MessageBox.Show("Cập nhật thất bại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                LoadData();
                            }
                            break;
                        case frmThuocTinhSanPham.Mode.Delete:
                            int maCongDung = Convert.ToInt32(row.Cells[0].Value);
                            if (congDungDAL.DeleteCongDung(maCongDung))
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
                    var newCongDung = new CongDungDTO
                    {
                        MaCongDung = 0,
                        TenCongDung = newRow.Cells[1].Value.ToString()
                    };
                    if (congDungDAL.InsertCongDung(newCongDung))
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
                saveFileDialog.FileName = "CongDung.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("CongDung");

                        worksheet.Cell(1, 1).Value = "MaCongDung";
                        worksheet.Cell(1, 2).Value = "TenCongDung";

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
            List<CongDungDTO> congDungList = congDungDAL.GetAllCongDung();

            var filteredList = congDungList.FindAll(congDung =>
                congDung.TenCongDung.ToLower().Contains(searchText) ||
                congDung.MaCongDung.ToString().Contains(searchText));

            dgvDanhSach.Rows.Clear();
            foreach (var congDung in filteredList)
            {
                dgvDanhSach.Rows.Add(congDung.MaCongDung, congDung.TenCongDung);
            }

            if (filteredList.Count == 0)
            {
                MessageBox.Show("Không tìm thấy kết quả nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
