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

        //public List<HoaDonNhapDTO> GetHoaDonNhapDetails()
        //{
        //    List<HoaDonNhapDTO> hoaDonNhapList = new List<HoaDonNhapDTO>();

        //    string query = @"
        //        SELECT 
        //            HoaDonNhap.SoHDN,
        //            HoaDonNhap.NgayNhap,
        //            HoaDonNhap.TongTien,
        //            NhanVien.TenNV AS TenNhanVien,
        //            NhaCungCap.TenNCC AS TenNhaCungCap,
        //            ChiTietHoaDonNhap.MaHang,
        //            DMHangHoa.TenHangHoa AS TenHangHoa,
        //            ChiTietHoaDonNhap.SoLuong,
        //            ChiTietHoaDonNhap.DonGia,
        //            ChiTietHoaDonNhap.GiamGia,
        //            ChiTietHoaDonNhap.ThanhTien
        //        FROM 
        //            HoaDonNhap
        //        JOIN 
        //            NhanVien ON HoaDonNhap.MaNV = NhanVien.MaNV
        //        JOIN 
        //            NhaCungCap ON HoaDonNhap.MaNCC = NhaCungCap.MaNCC
        //        JOIN 
        //            ChiTietHoaDonNhap ON HoaDonNhap.SoHDN = ChiTietHoaDonNhap.SoHDN
        //        JOIN 
        //            DMHangHoa ON ChiTietHoaDonNhap.MaHang = DMHangHoa.MaHang";

        //    DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);

        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        int soHDN = (int)row["SoHDN"];
        //        HoaDonNhapDTO hoaDonNhap = hoaDonNhapList.Find(h => h.SoHDN == soHDN);

        //        if (hoaDonNhap == null)
        //        {
        //            hoaDonNhap = new HoaDonNhapDTO
        //            {
        //                SoHDN = soHDN,
        //                NgayNhap = (DateTime)row["NgayNhap"],
        //                TongTien = (decimal)row["TongTien"],
        //                TenNhanVien = row["TenNhanVien"].ToString(),
        //                TenNhaCungCap = row["TenNhaCungCap"].ToString()
        //            };
        //            hoaDonNhapList.Add(hoaDonNhap);
        //        }

        //        ChiTietHoaDonNhapDTO chiTiet = new ChiTietHoaDonNhapDTO
        //        {
        //            MaHang = (int)row["MaHang"],
        //            TenHangHoa = row["TenHangHoa"].ToString(),
        //            SoLuong = (int)row["SoLuong"],
        //            DonGia = (decimal)row["DonGia"],
        //            GiamGia = (decimal)row["GiamGia"],
        //            ThanhTien = (decimal)row["ThanhTien"]
        //        };

        //        hoaDonNhap.ChiTietHoaDonNhap.Add(chiTiet);
        //    }

        //    return hoaDonNhapList;
        //}

        internal DataTable ConvertToDataTable(List<HoaDonNhapDTO> hoaDonNhapList)
        {
            DataTable dataTable = new DataTable();

            // Thêm các cột vào DataTable
            dataTable.Columns.Add("SoHDN", typeof(int));
            dataTable.Columns.Add("NgayNhap", typeof(DateTime));
            dataTable.Columns.Add("TenNhanVien", typeof(string));
            dataTable.Columns.Add("TenNhaCungCap", typeof(string));
            dataTable.Columns.Add("TongTien", typeof(decimal));

            // Điền dữ liệu từ List<HoaDonNhapDTO> vào DataTable
            foreach (var hoaDonNhap in hoaDonNhapList)
            {
                dataTable.Rows.Add(
                    hoaDonNhap.SoHDN,
                    hoaDonNhap.NgayNhap,
                    hoaDonNhap.TenNhanVien,
                    hoaDonNhap.TenNhaCungCap,
                    hoaDonNhap.TongTien
                );
            }

            return dataTable;
        }


        public List<HoaDonNhapDTO> GetHoaDonNhapSummary()
        {
            List<HoaDonNhapDTO> hoaDonNhapList = new List<HoaDonNhapDTO>();

            string query = @"
                SELECT 
                    hdn.SoHDN AS SoHoaDonNhap,
                    hdn.NgayNhap AS NgayNhap,
                    nv.TenNV AS TenNhanVien,
                    ncc.TenNCC AS TenNhaCungCap,
                    hdn.TongTien AS TongTien
                FROM 
                    HoaDonNhap hdn
                JOIN 
                    NhanVien nv ON hdn.MaNV = nv.MaNV
                JOIN 
                    NhaCungCap ncc ON hdn.MaNCC = ncc.MaNCC
                ORDER BY 
                    hdn.SoHDN";

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                HoaDonNhapDTO hoaDonNhap = new HoaDonNhapDTO
                {
                    SoHDN = (int)row["SoHoaDonNhap"],
                    NgayNhap = (DateTime)row["NgayNhap"],
                    TenNhanVien = row["TenNhanVien"].ToString(),
                    TenNhaCungCap = row["TenNhaCungCap"].ToString(),
                    TongTien = (decimal)row["TongTien"]
                };
                hoaDonNhapList.Add(hoaDonNhap);
            }

            return hoaDonNhapList;
        }

        public DataRow GetHoaDonNhapById(int soHDN)
        {
            string query = "SELECT HoaDonNhap.SoHDN, HoaDonNhap.NgayNhap, HoaDonNhap.TongTien, NhanVien.MaNV, NhanVien.TenNV, " +
                            "NhaCungCap.TenNCC, NhaCungCap.DiaChi, NhaCungCap.DienThoai " +
                            "FROM HoaDonNhap " +
                            "INNER JOIN NhanVien ON HoaDonNhap.MaNV = NhanVien.MaNV " +
                            "INNER JOIN NhaCungCap ON HoaDonNhap.MaNCC = NhaCungCap.MaNCC " +
                            "WHERE SoHDN = @SoHDN";

            SqlParameter parameter = new SqlParameter("@SoHDN", soHDN);

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query, parameter);

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows[0];
            }

            return null;
        }

        public int InsertHoaDonNhap(int MaNV, DateTime NgayNhap, int MaNCC, decimal TongTien)
        {
            string query = "INSERT INTO HoaDonNhap (MaNV, NgayNhap, MaNCC, TongTien) VALUES (@MaNV, @NgayNhap, @MaNCC, @TongTien); SELECT SCOPE_IDENTITY();";
            SqlParameter[] parameters = {
                new SqlParameter("@MaNV", MaNV),
                new SqlParameter("@NgayNhap", NgayNhap),
                new SqlParameter("@MaNCC", MaNCC),
                new SqlParameter("@TongTien", TongTien)
            };

            var result = DatabaseManager.Instance.ExecuteScalar(query, parameters);

            return result != null ? Convert.ToInt32(result) : -1;
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

        internal int GetSoHDN(int maNV, int maNCC, DateTime ngayNhap)
        {
            string query = "SELECT SoHDN FROM HoaDonNhap WHERE MaNV = @MaNV AND MaNCC = @MaNCC";
            SqlParameter[] parameters = {
                new SqlParameter("@MaNV", maNV),
                new SqlParameter("@MaNCC", maNCC),
                new SqlParameter("@NgayNhap", ngayNhap)
            };

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query, parameters);
            int result = -1;
            if (dataTable.Rows.Count > 0)
            {
                result = (int)dataTable.Rows[0]["SoHDN"];
            }
            return result;
        }
    }
}
