﻿using System;
using System.Data;
using System.Data.SqlClient;

namespace QLBG.DAL
{
    internal class HoaDonBanDAL
    {
        private readonly DatabaseManager dbManager;

        public HoaDonBanDAL()
        {
            dbManager = DatabaseManager.Instance;
        }

        // Thêm hóa đơn bán hàng mới
        public int ThemHoaDonBan(int maNV, int maKhach, DateTime ngayBan, decimal tongTien)
        {
            string query = @"
                INSERT INTO HoaDonBan (MaNV, MaKhach, NgayBan, TongTien)
                VALUES (@MaNV, @MaKhach, @NgayBan, @TongTien);
                SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@MaNV", maNV),
                new SqlParameter("@MaKhach", maKhach),
                new SqlParameter("@NgayBan", ngayBan),
                new SqlParameter("@TongTien", tongTien)
            };

            // Sử dụng SCOPE_IDENTITY() để lấy SoHDB của hóa đơn mới
            object result = dbManager.ExecuteScalar(query, parameters);
            return result != null ? Convert.ToInt32(result) : -1;
        }

        // Thêm chi tiết hóa đơn bán hàng
        public bool ThemChiTietHoaDon(int soHDB, int maHang, int soLuong, decimal giamGia, decimal thanhTien)
        {
            string query = @"
                INSERT INTO ChiTietHoaDonBan (SoHDB, MaHang, SoLuong, GiamGia, ThanhTien)
                VALUES (@SoHDB, @MaHang, @SoLuong, @GiamGia, @ThanhTien);";

            SqlParameter[] parameters = {
                new SqlParameter("@SoHDB", soHDB),
                new SqlParameter("@MaHang", maHang),
                new SqlParameter("@SoLuong", soLuong),
                new SqlParameter("@GiamGia", giamGia),
                new SqlParameter("@ThanhTien", thanhTien)
            };

            int rowsAffected = dbManager.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        // Lấy danh sách hóa đơn bán hàng
        public DataTable LayDanhSachHoaDon()
        {
            string query = @"
                SELECT 
                    HDB.SoHDB,
                    HDB.NgayBan,
                    HDB.TongTien,
                    NV.TenNV AS NhanVien,
                    KH.TenKhach AS KhachHang
                FROM HoaDonBan HDB
                INNER JOIN NhanVien NV ON HDB.MaNV = NV.MaNV
                INNER JOIN KhachHang KH ON HDB.MaKhach = KH.MaKhach";

            return dbManager.ExecuteDataTable(query, null);
        }

        // Lấy chi tiết của một hóa đơn bán hàng
        public DataTable LayChiTietHoaDon(int soHDB)
        {
            string query = @"
                SELECT 
                    CTHDB.SoHDB,
                    HH.TenHangHoa,
                    CTHDB.SoLuong,
                    CTHDB.GiamGia,
                    CTHDB.ThanhTien
                FROM ChiTietHoaDonBan CTHDB
                INNER JOIN DMHangHoa HH ON CTHDB.MaHang = HH.MaHang
                WHERE CTHDB.SoHDB = @SoHDB";

            SqlParameter[] parameters = {
                new SqlParameter("@SoHDB", soHDB)
            };

            return dbManager.ExecuteDataTable(query, parameters);
        }

        // Tính tổng doanh thu
        public decimal TinhTongDoanhThu(DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            string query = @"
                SELECT SUM(TongTien)
                FROM HoaDonBan
                WHERE NgayBan BETWEEN @NgayBatDau AND @NgayKetThuc";

            SqlParameter[] parameters = {
                new SqlParameter("@NgayBatDau", ngayBatDau),
                new SqlParameter("@NgayKetThuc", ngayKetThuc)
            };

            object result = dbManager.ExecuteScalar(query, parameters);
            return result != null ? Convert.ToDecimal(result) : 0;
        }

        public int GetInvoiceCountToday()
        {
            string query = @"SELECT COUNT(*) FROM HoaDonBan WHERE CAST(NgayBan AS DATE) = CAST(GETDATE() AS DATE)";
            object result = dbManager.ExecuteScalar(query);
            return result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        public decimal GetMonthlyRevenue(int month, int year)
        {
            string query = @"
                SELECT SUM(TongTien) 
                FROM HoaDonBan 
                WHERE MONTH(NgayBan) = @Month AND YEAR(NgayBan) = @Year";

            SqlParameter[] parameters = {
                new SqlParameter("@Month", month),
                new SqlParameter("@Year", year)
            };

            object result = dbManager.ExecuteScalar(query, parameters);
            return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
        }

        public int GetTotalProductCountInStock()
        {
            string query = "SELECT SUM(SoLuong) FROM DMHangHoa";
            object result = dbManager.ExecuteScalar(query);
            return result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        // Lấy doanh thu hàng tháng phân theo loại sản phẩm
        public DataTable GetMonthlySalesByProductType()
        {
            string query = @"
                SELECT 
                    L.TenLoai AS ProductType,
                    MONTH(HDB.NgayBan) AS Month,
                    SUM(CTHDB.ThanhTien) AS Revenue
                FROM 
                    ChiTietHoaDonBan CTHDB
                INNER JOIN 
                    HoaDonBan HDB ON CTHDB.SoHDB = HDB.SoHDB
                INNER JOIN 
                    DMHangHoa HH ON CTHDB.MaHang = HH.MaHang
                INNER JOIN 
                    Loai L ON HH.MaLoai = L.MaLoai
                GROUP BY 
                    L.TenLoai, MONTH(HDB.NgayBan)
                ORDER BY 
                    MONTH(HDB.NgayBan), L.TenLoai";

            return dbManager.ExecuteDataTable(query, null);
        }
    }
}