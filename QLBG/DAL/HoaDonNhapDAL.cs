using QLBG.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace QLBG.DAL
{
    public class HoaDonNhapDAL
    {
        public DataTable GetAllHoaDonNhap()
        {
            string query = "SELECT SoHDN, MaNV, NgayNhap, MaNCC, TongTien FROM HoaDonNhap";

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);

            return dataTable;
        }

        public DataRow GetHoaDonNhapById(int soHDN)
        {
            string query = "SELECT SoHDN, MaNV, NgayNhap, MaNCC, TongTien FROM HoaDonNhap WHERE SoHDN = @SoHDN";
            SqlParameter parameter = new SqlParameter("@SoHDN", soHDN);

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query, parameter);

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0];
            }

            return null;
        }

        public bool InsertHoaDonNhap(int MaNV, DateTime NgayNhap, int MaNCC, decimal TongTien)
        {
            string query = "INSERT INTO HoaDonNhap (MaNV, NgayNhap, MaNCC, TongTien) VALUES (@MaNV, @NgayNhap, @MaNCC, @TongTien)";
            SqlParameter[] parameters = {
                new SqlParameter("@MaNV", MaNV),
                new SqlParameter("@NgayNhap", NgayNhap),
                new SqlParameter("@MaNCC", MaNCC),
                new SqlParameter("@TongTien", TongTien)
            };

            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateHoaDonNhap(int SoHDN, int MaNV, DateTime NgayNhap, int MaNCC, decimal TongTien)
        {
            string query = "UPDATE HoaDonNhap SET MaNV = @MaNV, NgayNhap = @NgayNhap, MaNCC = @MaNCC, TongTien = @TongTien WHERE SoHDN = @SoHDN";
            SqlParameter[] parameters = {
                new SqlParameter("@MaNV", MaNV),
                new SqlParameter("@NgayNhap", NgayNhap),
                new SqlParameter("@MaNCC", MaNCC),
                new SqlParameter("@TongTien", TongTien),
                new SqlParameter("@SoHDN", SoHDN)
            };

            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteHoaDonNhap(int soHDN)
        {
            string query = "DELETE FROM HoaDonNhap WHERE SoHDN = @SoHDN";
            SqlParameter parameter = new SqlParameter("@SoHDN", soHDN);

            return DatabaseManager.Instance.ExecuteNonQuery(query, parameter) > 0;
        }

        public DataTable GetAllHoaDonNhapWithNCCAndNV()
        {
            string query = "SELECT HoaDonNhap.SoHDN, NhanVien.TenNV, NhaCungCap.TenNCC, HoaDonNhap.NgayNhap, HoaDonNhap.TongTien " +
                           "FROM HoaDonNhap " +
                           "JOIN NhaCungCap ON HoaDonNhap.MaNCC = NhaCungCap.MaNCC " +
                           "JOIN NhanVien ON HoaDonNhap.MaNV = NhanVien.MaNV";

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);
            return dataTable;
        }

        public DataTable SearchHoaDonNhap(string keyword)
        {
            string query = "SELECT SoHDN, MaNV, NgayNhap, MaNCC, TongTien FROM HoaDonNhap WHERE SoHDN LIKE @Keyword OR MaNV LIKE @Keyword OR NgayNhap LIKE @Keyword OR MaNCC LIKE @Keyword OR TongTien LIKE @Keyword";
            SqlParameter parameter = new SqlParameter("@Keyword", $"%{keyword}%");

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query, parameter);

            return dataTable;
        }
    }
}
