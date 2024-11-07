using ClosedXML.Excel;
using Guna.UI2.WinForms;
using QLBG.DAL;
using QLBG.DTO;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QLBG.Views.SanPham
{
    public partial class pgChatLieu : UserControl
    {
        private ChatLieuDAL chatLieuDAL;

        public pgChatLieu()
        {
            InitializeComponent();
            chatLieuDAL = new ChatLieuDAL();
            LoadData();
            dgvDanhSach.CellClick += dgvCellClick;
        }

        public void LoadData()
        {
            List<ChatLieuDTO> chatLieuList = chatLieuDAL.GetAllChatLieu();
            lbSoLuong.Text = chatLieuList.Count.ToString();
            dgvDanhSach.Rows.Clear();
            foreach (var chatLieu in chatLieuList)
            {
                dgvDanhSach.Rows.Add(chatLieu.MaChatLieu, chatLieu.TenChatLieu);
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
                            var updatedChatLieu = new ChatLieuDTO
                            {
                                MaChatLieu = Convert.ToInt32(row.Cells[0].Value),
                                TenChatLieu = result.Cells[1].Value.ToString()
                            };
                            if (!chatLieuDAL.UpdateChatLieu(updatedChatLieu))
                            {
                                MessageBox.Show("Cập nhật thất bại.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                LoadData();
                            }
                            break;
                        case frmThuocTinhSanPham.Mode.Delete:
                            int maChatLieu = Convert.ToInt32(row.Cells[0].Value);
                            if (chatLieuDAL.DeleteChatLieu(maChatLieu))
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
                    var newChatLieu = new ChatLieuDTO
                    {
                        MaChatLieu = 0,
                        TenChatLieu = newRow.Cells[1].Value.ToString()
                    };
                    if (chatLieuDAL.InsertChatLieu(newChatLieu))
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
                saveFileDialog.FileName = "ChatLieu.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("ChatLieu");

                        int excelCol = 1;
                        for (int i = 0; i < dgvDanhSach.Columns.Count; i++)
                        {
                            if (dgvDanhSach.Columns[i].Name == "Anh" || dgvDanhSach.Columns[i].Name == "View")
                                continue;

                            worksheet.Cell(1, excelCol).Value = dgvDanhSach.Columns[i].HeaderText;
                            excelCol++;
                        }

                        int excelRow = 2;
                        foreach (DataGridViewRow row in dgvDanhSach.Rows)
                        {
                            if (row.IsNewRow) continue;

                            excelCol = 1;
                            for (int j = 0; j < dgvDanhSach.Columns.Count; j++)
                            {
                                if (dgvDanhSach.Columns[j].Name == "Anh" || dgvDanhSach.Columns[j].Name == "View")
                                    continue;

                                var cellValue = row.Cells[j].Value;
                                worksheet.Cell(excelRow, excelCol).Value = cellValue == null || cellValue is DBNull ? "" : cellValue.ToString();
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

        private void btnTim_Click(object sender, EventArgs e)
        {
            string searchText = txtTim.Text.Trim().ToLower();
            List<ChatLieuDTO> chatLieuList = chatLieuDAL.GetAllChatLieu();

            var filteredList = chatLieuList.FindAll(chatLieu =>
                chatLieu.TenChatLieu.ToLower().Contains(searchText) ||
                chatLieu.MaChatLieu.ToString().Contains(searchText));

            dgvDanhSach.Rows.Clear();
            foreach (var chatLieu in filteredList)
            {
                dgvDanhSach.Rows.Add(chatLieu.MaChatLieu, chatLieu.TenChatLieu);
            }

            if (filteredList.Count == 0)
            {
                MessageBox.Show("Không tìm thấy kết quả nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
