using ClosedXML.Excel;
using Guna.UI2.WinForms;
using QLBG.DAL;
using QLBG.DTO;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QLBG.Views.SanPham
{
    public partial class pageKichCo : UserControl
    {
        private KichThuocDAL kichThuocDAL;

        public pageKichCo()
        {
            InitializeComponent();
            kichThuocDAL = new KichThuocDAL();
            LoadData();
            dgvDanhSach.CellClick += dgvCellClick;
        }

        public void LoadData()
        {
            List<KichThuocDTO> kichThuocList = kichThuocDAL.GetAllKichThuoc();
            lbSoLuong.Text = kichThuocList.Count.ToString();
            dgvDanhSach.Rows.Clear();
            foreach (var kichThuoc in kichThuocList)
            {
                dgvDanhSach.Rows.Add(kichThuoc.MaKichThuoc, kichThuoc.TenKichThuoc);
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
                            var updatedKichThuoc = new KichThuocDTO
                            {
                                MaKichThuoc = Convert.ToInt32(row.Cells[0].Value),
                                TenKichThuoc = result.Cells[1].Value.ToString()
                            };
                            if (!kichThuocDAL.UpdateKichThuoc(updatedKichThuoc))
                            {
                                MessageBox.Show("Cập nhật thất bại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            } else
                            {
                                LoadData();
                            }
                            break;
                        case frmThuocTinhSanPham.Mode.Delete:
                            int maKichThuoc = Convert.ToInt32(row.Cells[0].Value);
                            if (kichThuocDAL.DeleteKichThuoc(maKichThuoc))
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
                    var newKichThuoc = new KichThuocDTO
                    {
                        MaKichThuoc = 0,
                        TenKichThuoc = newRow.Cells[1].Value.ToString()
                    };
                    if (kichThuocDAL.InsertKichThuoc(newKichThuoc))
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
                saveFileDialog.FileName = "KichThuoc.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("KichThuoc");

                        worksheet.Cell(1, 1).Value = "MaKichThuoc";
                        worksheet.Cell(1, 2).Value = "TenKichThuoc";

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
            List<KichThuocDTO> kichThuocList = kichThuocDAL.GetAllKichThuoc();

            var filteredList = kichThuocList.FindAll(kichThuoc =>
                kichThuoc.TenKichThuoc.ToLower().Contains(searchText) ||
                kichThuoc.MaKichThuoc.ToString().Contains(searchText));

            dgvDanhSach.Rows.Clear();
            foreach (var kichThuoc in filteredList)
            {
                dgvDanhSach.Rows.Add(kichThuoc.MaKichThuoc, kichThuoc.TenKichThuoc);
            }

            if (filteredList.Count == 0)
            {
                MessageBox.Show("Không tìm thấy kết quả nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
