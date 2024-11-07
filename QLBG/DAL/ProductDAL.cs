using System.Data;
using System.Data.SqlClient;
using QLBG.DTO;
using QLBG;
using System.Collections.Generic;
using System;

namespace QLBG.DAL
{
    public class ProductDAL
    {

        public List<ProductDTO> GetAllProducts()
        {
            List<ProductDTO> productList = new List<ProductDTO>();
            string query = "SELECT * FROM DMHangHoa"; 

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                ProductDTO product = new ProductDTO
                {
                    MaHang = (int)row["MaHang"],
                    TenHangHoa = row["TenHangHoa"].ToString(),
                    MaLoai = row.Field<int>("MaLoai"),
                    MaKichThuoc = row.Field<int>("MaKichThuoc"),
                    MaHinhDang = row.Field<int>("MaHinhDang"),
                    MaChatLieu = row.Field<int>("MaChatLieu"),
                    MaNuocSX = row.Field<int>("MaNuocSX"),
                    MaDacDiem = row.Field<int>("MaDacDiem"),
                    MaMau = row.Field<int>("MaMau"),
                    MaCongDung = row.Field<int>("MaCongDung"),
                    MaNSX = row.Field<int>("MaNSX"),
                    SoLuong = row.Field<int>("SoLuong"),
                    DonGiaNhap = row.Field<decimal>("DonGiaNhap"),
                    DonGiaBan = row.Field<decimal>("DonGiaBan"),
                    ThoiGianBaoHanh = row.Field<int>("ThoiGianBaoHanh"),
                    Anh = row["Anh"].ToString(),
                    GhiChu = row["GhiChu"].ToString()
                };
                Console.WriteLine("Product ID: " + product.MaHang);
                productList.Add(product);
            }

            return productList;
        }

        public bool UpdateProduct(ProductDTO product)
        {
            string query = @"UPDATE DMHangHoa
                     SET TenHangHoa = @TenHangHoa,
                         MaLoai = @MaLoai,
                         MaKichThuoc = @MaKichThuoc,
                         MaHinhDang = @MaHinhDang,
                         MaChatLieu = @MaChatLieu,
                         MaNuocSX = @MaNuocSX,
                         MaDacDiem = @MaDacDiem,
                         MaMau = @MaMau,
                         MaCongDung = @MaCongDung,
                         MaNSX = @MaNSX,
                         SoLuong = @SoLuong,
                         DonGiaNhap = @DonGiaNhap,
                         DonGiaBan = @DonGiaBan,
                         ThoiGianBaoHanh = @ThoiGianBaoHanh,
                         Anh = @Anh,
                         GhiChu = @GhiChu
                     WHERE MaHang = @MaHang";

            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@TenHangHoa", product.TenHangHoa),
        new SqlParameter("@MaLoai", product.MaLoai),
        new SqlParameter("@MaKichThuoc", product.MaKichThuoc),
        new SqlParameter("@MaHinhDang", product.MaHinhDang),
        new SqlParameter("@MaChatLieu", product.MaChatLieu),
        new SqlParameter("@MaNuocSX", product.MaNuocSX),
        new SqlParameter("@MaDacDiem", product.MaDacDiem),
        new SqlParameter("@MaMau", product.MaMau),
        new SqlParameter("@MaCongDung", product.MaCongDung),
        new SqlParameter("@MaNSX", product.MaNSX),
        new SqlParameter("@SoLuong", product.SoLuong),
        new SqlParameter("@DonGiaNhap", product.DonGiaNhap),
        new SqlParameter("@DonGiaBan", product.DonGiaBan),
        new SqlParameter("@ThoiGianBaoHanh", product.ThoiGianBaoHanh),
        new SqlParameter("@Anh", (object)product.Anh ?? DBNull.Value),
        new SqlParameter("@GhiChu", product.GhiChu),
        new SqlParameter("@MaHang", product.MaHang)
            };

            int rowsAffected = DatabaseManager.Instance.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        public bool DeleteProduct(int maHang)
        {
            string query = "DELETE FROM DMHangHoa WHERE MaHang = @MaHang";
            SqlParameter parameter = new SqlParameter("@MaHang", maHang);
            int rowsAffected = DatabaseManager.Instance.ExecuteNonQuery(query, parameter);
            return rowsAffected > 0;
        }


        public ProductDTO GetProductById(int productId)
        {
            string query = "SELECT * FROM DMHangHoa WHERE MaHang = @MaHang";
            SqlParameter parameter = new SqlParameter("@MaHang", productId);

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query, parameter);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new ProductDTO
                {
                    MaHang = (int)row["MaHang"],
                    TenHangHoa = row["TenHangHoa"].ToString(),
                    MaLoai = (int)row["MaLoai"],
                    MaKichThuoc = (int)row["MaKichThuoc"],
                    MaHinhDang = (int)row["MaHinhDang"],
                    MaChatLieu = (int)row["MaChatLieu"],
                    MaNuocSX = (int)row["MaNuocSX"],
                    MaDacDiem = (int)row["MaDacDiem"],
                    MaMau = (int)row["MaMau"],
                    MaCongDung = (int)row["MaCongDung"],
                    MaNSX = (int)row["MaNSX"],
                    SoLuong = (int)row["SoLuong"],
                    DonGiaNhap = (decimal)row["DonGiaNhap"],
                    DonGiaBan = (decimal)row["DonGiaBan"],
                    ThoiGianBaoHanh = (int)row["ThoiGianBaoHanh"],
                    Anh = row["Anh"].ToString(),
                    GhiChu = row["GhiChu"].ToString()
                };
            }
            return null;
        }

        public bool InsertProduct(ProductDTO product)
        {
            string query = @"INSERT INTO DMHangHoa 
                     (TenHangHoa, MaLoai, MaKichThuoc, MaHinhDang, MaChatLieu, MaNuocSX, MaDacDiem, 
                      MaMau, MaCongDung, MaNSX, SoLuong, DonGiaNhap, DonGiaBan, ThoiGianBaoHanh, Anh, GhiChu) 
                     VALUES 
                     (@TenHangHoa, @MaLoai, @MaKichThuoc, @MaHinhDang, @MaChatLieu, @MaNuocSX, @MaDacDiem, 
                      @MaMau, @MaCongDung, @MaNSX, @SoLuong, @DonGiaNhap, @DonGiaBan, @ThoiGianBaoHanh, @Anh, @GhiChu)";

            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@TenHangHoa", product.TenHangHoa ?? (object)DBNull.Value),
        new SqlParameter("@MaLoai", product.MaLoai),
        new SqlParameter("@MaKichThuoc", product.MaKichThuoc),
        new SqlParameter("@MaHinhDang", product.MaHinhDang),
        new SqlParameter("@MaChatLieu", product.MaChatLieu),
        new SqlParameter("@MaNuocSX", product.MaNuocSX),
        new SqlParameter("@MaDacDiem", product.MaDacDiem),
        new SqlParameter("@MaMau", product.MaMau),
        new SqlParameter("@MaCongDung", product.MaCongDung),
        new SqlParameter("@MaNSX", product.MaNSX),
        new SqlParameter("@SoLuong", product.SoLuong),
        new SqlParameter("@DonGiaNhap", product.DonGiaNhap),
        new SqlParameter("@DonGiaBan", product.DonGiaBan),
        new SqlParameter("@ThoiGianBaoHanh", product.ThoiGianBaoHanh),
        new SqlParameter("@Anh", product.Anh ?? (object)DBNull.Value),
        new SqlParameter("@GhiChu", product.GhiChu ?? (object)DBNull.Value)
            };

            int result = DatabaseManager.Instance.ExecuteNonQuery(query, parameters);
            return result > 0;
        }
    }
}
