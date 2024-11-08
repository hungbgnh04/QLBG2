using QLBG.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBG.DAL
{
    internal class NhanVienDAL
    {
        public List<NhanVienDTO> GetNhanVienDetails()
        {
            List<NhanVienDTO> nhanVienList = new List<NhanVienDTO>();

            string query = @"SELECT NhanVien.MaNV, NhanVien.TenNV, NhanVien.GioiTinh, NhanVien.NgaySinh, NhanVien.DienThoai,
                             NhanVien.Password, NhanVien.QuyenAdmin, NhanVien.Email, NhanVien.DiaChi, NhanVien.Anh,
                             NhanVien.MaCV, CongViec.TenCV AS TenCongViec
                             FROM NhanVien
                             LEFT JOIN CongViec ON NhanVien.MaCV = CongViec.MaCV";
            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                NhanVienDTO nhanVien = new NhanVienDTO
                {
                    MaNV = (int)row["MaNV"],
                    TenNV = row["TenNV"].ToString(),
                    GioiTinh = (bool)row["GioiTinh"],
                    NgaySinh = (DateTime)row["NgaySinh"],
                    DienThoai = row["DienThoai"].ToString(),
                    Password = row["Password"].ToString(),
                    QuyenAdmin = (bool)row["QuyenAdmin"],
                    Email = row["Email"].ToString(),
                    DiaChi = row["DiaChi"].ToString(),
                    Anh = row["Anh"].ToString(),
                    MaCV = (int)row["MaCV"],
                    TenCongViec = row["TenCongViec"].ToString()
                };
                nhanVienList.Add(nhanVien);
            }

            return nhanVienList;
        }
        internal DataTable ConvertToDataTable(List<NhanVienDTO> nhanVienList)
        {
            DataTable dataTable = new DataTable();

            // Thêm các cột vào DataTable
            dataTable.Columns.Add("MaNV", typeof(int));
            dataTable.Columns.Add("TenNV", typeof(string));
            dataTable.Columns.Add("GioiTinh", typeof(bool));
            dataTable.Columns.Add("NgaySinh", typeof(DateTime));
            dataTable.Columns.Add("DienThoai", typeof(string));
            dataTable.Columns.Add("Password", typeof(string));
            dataTable.Columns.Add("QuyenAdmin", typeof(bool));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("DiaChi", typeof(string));
            dataTable.Columns.Add("Anh", typeof(string));
            dataTable.Columns.Add("MaCV", typeof(int));
            dataTable.Columns.Add("TenCongViec", typeof(string));

            // Điền dữ liệu từ List<NhanVienDTO> vào DataTable
            foreach (var nhanVien in nhanVienList)
            {
                dataTable.Rows.Add(nhanVien.MaNV, nhanVien.TenNV, nhanVien.GioiTinh, nhanVien.NgaySinh,
                                   nhanVien.DienThoai, nhanVien.Password, nhanVien.QuyenAdmin, nhanVien.Email,
                                   nhanVien.DiaChi, nhanVien.Anh, nhanVien.MaCV, nhanVien.TenCongViec);
            }

            return dataTable;
        }

    }
}
