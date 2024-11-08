using QLBG.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace QLBG.DAL
{
    public class ChiTietHoaDonNhapDAL
    {
        public DataTable GetAllChiTietHoaDonNhap()
        {
            string query = "SELECT SoHDN, MaHang, SoLuong, DonGia, GiamGia, ThanhTien FROM ChiTietHoaDonNhap";

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);

            return dataTable;
        }

        public DataRow GetChiTietHoaDonNhapById(int soHDN, int maHang)
        {
            string query = "SELECT SoHDN, MaHang, SoLuong, DonGia, GiamGia, ThanhTien FROM ChiTietHoaDonNhap WHERE SoHDN = @SoHDN AND MaHang = @MaHang";
            SqlParameter[] parameters = {
                new SqlParameter("@SoHDN", soHDN),
                new SqlParameter("@MaHang", maHang)
            };

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
               return dataTable.Rows[0];
            }

            return null;
        }

        public bool InsertChiTietHoaDonNhap(int SoHDN, int MaHang, int SoLuong, decimal DonGia, decimal GiamGia, decimal ThanhTien)
        {
            string query = "INSERT INTO ChiTietHoaDonNhap (SoHDN, MaHang, SoLuong, DonGia, GiamGia, ThanhTien) VALUES (@SoHDN, @MaHang, @SoLuong, @DonGia, @GiamGia, @ThanhTien)";
            SqlParameter[] parameters = {
                new SqlParameter("@SoHDN", SoHDN),
                new SqlParameter("@MaHang", MaHang),
                new SqlParameter("@SoLuong", SoLuong),
                new SqlParameter("@DonGia", DonGia),
                new SqlParameter("@GiamGia", GiamGia),
                new SqlParameter("@ThanhTien", ThanhTien)
            };

            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateChiTietHoaDonNhap(int SoHDN, int MaHang, int SoLuong, decimal DonGia, decimal GiamGia, decimal ThanhTien)
        {
            string query = "UPDATE ChiTietHoaDonNhap SET SoLuong = @SoLuong, DonGia = @DonGia, GiamGia = @GiamGia, ThanhTien = @ThanhTien WHERE SoHDN = @SoHDN AND MaHang = @MaHang";
            SqlParameter[] parameters = {
                new SqlParameter("@SoLuong", SoLuong),
                new SqlParameter("@DonGia", DonGia),
                new SqlParameter("@GiamGia", GiamGia),
                new SqlParameter("@ThanhTien", ThanhTien),
                new SqlParameter("@SoHDN", SoHDN),
                new SqlParameter("@MaHang", MaHang)
            };

            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteChiTietHoaDonNhap(int soHDN, int maHang)
        {
            string query = "DELETE FROM ChiTietHoaDonNhap WHERE SoHDN = @SoHDN AND MaHang = @MaHang";
            SqlParameter[] parameters = {
                new SqlParameter("@SoHDN", soHDN),
                new SqlParameter("@MaHang", maHang)
            };

            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        internal DataTable GetChiTietHoaDonNhapBySoHDN(int soHDN)
        {
            string query = "SELECT DMHangHoa.MaHang, DMHangHoa.TenHangHoa, ChiTietHoaDonNhap.SoLuong, ChiTietHoaDonNhap.DonGia, " +
                            "ChiTietHoaDonNhap.GiamGia, ChiTietHoaDonNhap.ThanhTien " +
                            "FROM ChiTietHoaDonNhap " +
                            "INNER JOIN DMHangHoa ON ChiTietHoaDonNhap.MaHang = DMHangHoa.MaHang " +
                            "WHERE SoHDN = @SoHDN";
            SqlParameter[] parameters = {
                new SqlParameter("@SoHDN", soHDN)
            };

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
                return dataTable;
            }

            return null;
        }

        
    }
}
