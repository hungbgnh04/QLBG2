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

        public List<HoaDonNhapDTO> GetHoaDonNhapDetails()
        {
            List<HoaDonNhapDTO> hoaDonNhapList = new List<HoaDonNhapDTO>();

            string query = @"
                SELECT 
                    HoaDonNhap.SoHDN,
                    HoaDonNhap.NgayNhap,
                    HoaDonNhap.TongTien,
                    NhanVien.TenNV AS TenNhanVien,
                    NhaCungCap.TenNCC AS TenNhaCungCap,
                    ChiTietHoaDonNhap.MaHang,
                    DMHangHoa.TenHangHoa AS TenHangHoa,
                    ChiTietHoaDonNhap.SoLuong,
                    ChiTietHoaDonNhap.DonGia,
                    ChiTietHoaDonNhap.GiamGia,
                    ChiTietHoaDonNhap.ThanhTien
                FROM 
                    HoaDonNhap
                JOIN 
                    NhanVien ON HoaDonNhap.MaNV = NhanVien.MaNV
                JOIN 
                    NhaCungCap ON HoaDonNhap.MaNCC = NhaCungCap.MaNCC
                JOIN 
                    ChiTietHoaDonNhap ON HoaDonNhap.SoHDN = ChiTietHoaDonNhap.SoHDN
                JOIN 
                    DMHangHoa ON ChiTietHoaDonNhap.MaHang = DMHangHoa.MaHang";

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                int soHDN = (int)row["SoHDN"];
                HoaDonNhapDTO hoaDonNhap = hoaDonNhapList.Find(h => h.SoHDN == soHDN);

                if (hoaDonNhap == null)
                {
                    hoaDonNhap = new HoaDonNhapDTO
                    {
                        SoHDN = soHDN,
                        NgayNhap = (DateTime)row["NgayNhap"],
                        TongTien = (decimal)row["TongTien"],
                        TenNhanVien = row["TenNhanVien"].ToString(),
                        TenNhaCungCap = row["TenNhaCungCap"].ToString()
                    };
                    hoaDonNhapList.Add(hoaDonNhap);
                }

                ChiTietHoaDonNhapDTO chiTiet = new ChiTietHoaDonNhapDTO
                {
                    MaHang = (int)row["MaHang"],
                    TenHangHoa = row["TenHangHoa"].ToString(),
                    SoLuong = (int)row["SoLuong"],
                    DonGia = (decimal)row["DonGia"],
                    GiamGia = (decimal)row["GiamGia"],
                    ThanhTien = (decimal)row["ThanhTien"]
                };

                hoaDonNhap.ChiTietHoaDonNhap.Add(chiTiet);
            }

            return hoaDonNhapList;
        }

        internal DataTable ConvertToDataTable(List<HoaDonNhapDTO> hoaDonNhapList)
        {
            DataTable dataTable = new DataTable();

            // Thêm các cột vào DataTable
            dataTable.Columns.Add("SoHDN", typeof(int));
            dataTable.Columns.Add("NgayNhap", typeof(DateTime));
            dataTable.Columns.Add("TongTien", typeof(decimal));
            dataTable.Columns.Add("TenNhanVien", typeof(string));
            dataTable.Columns.Add("TenNhaCungCap", typeof(string));
            dataTable.Columns.Add("MaHang", typeof(int));
            dataTable.Columns.Add("TenHangHoa", typeof(string));
            dataTable.Columns.Add("SoLuong", typeof(int));
            dataTable.Columns.Add("DonGia", typeof(decimal));
            dataTable.Columns.Add("GiamGia", typeof(decimal));
            dataTable.Columns.Add("ThanhTien", typeof(decimal));

            // Điền dữ liệu từ List<HoaDonNhapDTO> vào DataTable
            foreach (var hoaDonNhap in hoaDonNhapList)
            {
                foreach (var chiTiet in hoaDonNhap.ChiTietHoaDonNhap)
                {
                    dataTable.Rows.Add(
                        hoaDonNhap.SoHDN,
                        hoaDonNhap.NgayNhap,
                        hoaDonNhap.TongTien,
                        hoaDonNhap.TenNhanVien,
                        hoaDonNhap.TenNhaCungCap,
                        chiTiet.MaHang,
                        chiTiet.TenHangHoa,
                        chiTiet.SoLuong,
                        chiTiet.DonGia,
                        chiTiet.GiamGia,
                        chiTiet.ThanhTien
                    );
                }
            }

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
