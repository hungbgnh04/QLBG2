using QLBG.DTO;
using System;
using System.Collections.Generic;
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

        public List<HoaDonBanDTO> GetHoaDonBanSummary()
        {
            List<HoaDonBanDTO> hoaDonBanList = new List<HoaDonBanDTO>();

            string query = @"
                SELECT 
                    hdb.SoHDB AS SoHoaDonBan,
                    hdb.NgayBan AS NgayBan,
                    nv.TenNV AS TenNhanVien,
                    kh.TenKhach AS TenKhachHang,
                    kh.DiaChi AS DiaChiKhachHang,
                    kh.DienThoai AS SDTKhachHang,
                    hdb.TongTien AS TongTien
                FROM 
                    HoaDonBan hdb
                JOIN 
                    NhanVien nv ON hdb.MaNV = nv.MaNV
                JOIN 
                    KhachHang kh ON hdb.MaKhach = kh.MaKhach
                ORDER BY 
                    hdb.SoHDB";

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                HoaDonBanDTO hoaDonBan = new HoaDonBanDTO
                {
                    SoHDB = (int)row["SoHoaDonBan"],
                    NgayBan = (DateTime)row["NgayBan"],
                    TenNhanVien = row["TenNhanVien"].ToString(),
                    TenKhachHang = row["TenKhachHang"].ToString(),
                    DiaChiKhachHang = row["DiaChiKhachHang"].ToString(),
                    SDTKhachHang = row["SDTKhachHang"].ToString(),
                    TongTien = (decimal)row["TongTien"]
                };
                hoaDonBanList.Add(hoaDonBan);
            }

            return hoaDonBanList;
        }

        internal DataTable ConvertToDataTable(List<HoaDonBanDTO> hoaDonBanList)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("SoHDB", typeof(int));
            dataTable.Columns.Add("NgayBan", typeof(DateTime));
            dataTable.Columns.Add("TenNhanVien", typeof(string));
            dataTable.Columns.Add("TenKhachHang", typeof(string));
            dataTable.Columns.Add("DiaChiKhachHang", typeof(string));
            dataTable.Columns.Add("SDTKhachHang", typeof(string));
            dataTable.Columns.Add("TongTien", typeof(decimal));
            foreach (var hoaDonBan in hoaDonBanList)
            {
                dataTable.Rows.Add(
                    hoaDonBan.SoHDB,
                    hoaDonBan.NgayBan,
                    hoaDonBan.TenNhanVien,
                    hoaDonBan.TenKhachHang,
                    hoaDonBan.DiaChiKhachHang,
                    hoaDonBan.SDTKhachHang,
                    hoaDonBan.TongTien
                );
            }

            return dataTable;
        }

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
                    HH.MaHang,
                    HH.DonGiaBan as DonGia,
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

        public DataTable GetAllInvoiceWithAttributeName()
        {
            string query = "SELECT HoaDonBan.SoHDB, NhanVien.TenNV, KhachHang.TenKhach, HoaDonBan.NgayBan, HoaDonBan.TongTien " +
                           "FROM HoaDonBan " +
                           "JOIN KhachHang ON HoaDonBan.MaKhach = KhachHang.MaKhach " +
                           "JOIN NhanVien ON HoaDonBan.MaNV = NhanVien.MaNV";

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);
            return dataTable;
        }

        public DataRow GetInvoice(int id)
        {
            string query = "SELECT HoaDonBan.SoHDB, HoaDonBan.NgayBan, HoaDonBan.TongTien, NhanVien.MaNV, NhanVien.TenNV, " +
                            "KhachHang.TenKhach, KhachHang.DiaChi, KhachHang.DienThoai " +
                            "FROM HoaDonBan " +
                            "INNER JOIN NhanVien ON HoaDonBan.MaNV = NhanVien.MaNV " +
                            "INNER JOIN KhachHang ON HoaDonBan.MaKhach = KhachHang.MaKhach " +
                            "WHERE SoHDB = @SoHDB";

            SqlParameter parameter = new SqlParameter("@SoHDB", id);

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query, parameter);

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0];
            }

            return null;
        }

        internal bool DeleteHoaDonBan(int soHDB)
        {
            string query = "DELETE FROM HoaDonBan WHERE SoHDB = @SoHDB";
            SqlParameter parameter = new SqlParameter("@SoHDB", soHDB);

            return DatabaseManager.Instance.ExecuteNonQuery(query, parameter) > 0;
        }
    }
}
