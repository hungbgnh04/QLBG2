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

        public List<ProductDTO> GetProductDetails()
        {
            List<ProductDTO> productList = new List<ProductDTO>();

            // Truy vấn SQL để lấy mã và tên của các thuộc tính liên quan
            string query = @"
        SELECT 
            DMHangHoa.MaHang,
            DMHangHoa.TenHangHoa,
            DMHangHoa.MaLoai,
            Loai.TenLoai,
            DMHangHoa.MaKichThuoc,
            KichThuoc.TenKichThuoc,
            DMHangHoa.MaHinhDang,
            HinhDang.TenHinhDang,
            DMHangHoa.MaChatLieu,
            ChatLieu.TenChatLieu,
            DMHangHoa.MaNuocSX,
            NuocSX.TenNuocSX,
            DMHangHoa.MaDacDiem,
            DacDiem.TenDacDiem,
            DMHangHoa.MaMau,
            MauSac.TenMau,
            DMHangHoa.MaCongDung,
            CongDung.TenCongDung,
            DMHangHoa.MaNSX,
            NhaSanXuat.TenNSX,
            DMHangHoa.SoLuong,
            DMHangHoa.DonGiaNhap,
            DMHangHoa.DonGiaBan,
            DMHangHoa.ThoiGianBaoHanh,
            DMHangHoa.Anh,
            DMHangHoa.GhiChu
        FROM 
            DMHangHoa
        LEFT JOIN 
            Loai ON DMHangHoa.MaLoai = Loai.MaLoai
        LEFT JOIN 
            KichThuoc ON DMHangHoa.MaKichThuoc = KichThuoc.MaKichThuoc
        LEFT JOIN 
            HinhDang ON DMHangHoa.MaHinhDang = HinhDang.MaHinhDang
        LEFT JOIN 
            ChatLieu ON DMHangHoa.MaChatLieu = ChatLieu.MaChatLieu
        LEFT JOIN 
            NuocSX ON DMHangHoa.MaNuocSX = NuocSX.MaNuocSX
        LEFT JOIN 
            DacDiem ON DMHangHoa.MaDacDiem = DacDiem.MaDacDiem
        LEFT JOIN 
            MauSac ON DMHangHoa.MaMau = MauSac.MaMau
        LEFT JOIN 
            CongDung ON DMHangHoa.MaCongDung = CongDung.MaCongDung
        LEFT JOIN 
            NhaSanXuat ON DMHangHoa.MaNSX = NhaSanXuat.MaNSX";
            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);
            foreach (DataRow row in dataTable.Rows)
            {
                ProductDTO product = new ProductDTO
                {
                    MaHang = (int)row["MaHang"],
                    TenHangHoa = row["TenHangHoa"].ToString(),
                    MaLoai = (int)row["MaLoai"],
                    TenLoai = row["TenLoai"].ToString(),
                    MaKichThuoc = (int)row["MaKichThuoc"],
                    TenKichThuoc = row["TenKichThuoc"].ToString(),
                    MaHinhDang = (int)row["MaHinhDang"],
                    TenHinhDang = row["TenHinhDang"].ToString(),
                    MaChatLieu = (int)row["MaChatLieu"],
                    TenChatLieu = row["TenChatLieu"].ToString(),
                    MaNuocSX = (int)row["MaNuocSX"],
                    TenNuocSX = row["TenNuocSX"].ToString(),
                    MaDacDiem = (int)row["MaDacDiem"],
                    TenDacDiem = row["TenDacDiem"].ToString(),
                    MaMau = (int)row["MaMau"],
                    TenMau = row["TenMau"].ToString(),
                    MaCongDung = (int)row["MaCongDung"],
                    TenCongDung = row["TenCongDung"].ToString(),
                    MaNSX = (int)row["MaNSX"],
                    TenNSX = row["TenNSX"].ToString(),
                    SoLuong = row.Field<int>("SoLuong"),
                    DonGiaNhap = row.Field<decimal>("DonGiaNhap"),
                    DonGiaBan = row.Field<decimal>("DonGiaBan"),
                    ThoiGianBaoHanh = row.Field<int>("ThoiGianBaoHanh"),
                    Anh = row["Anh"].ToString(),
                    GhiChu = row["GhiChu"].ToString()
                };

                productList.Add(product);
            }

            return productList;
        }

        internal DataTable ConvertToDataTable(List<ProductDTO> products)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("MaHang", typeof(int));
            dataTable.Columns.Add("TenHangHoa", typeof(string));
            dataTable.Columns.Add("TenLoai", typeof(string));
            dataTable.Columns.Add("TenKichThuoc", typeof(string));
            dataTable.Columns.Add("TenHinhDang", typeof(string));
            dataTable.Columns.Add("TenChatLieu", typeof(string));
            dataTable.Columns.Add("TenNuocSX", typeof(string));
            dataTable.Columns.Add("TenDacDiem", typeof(string));
            dataTable.Columns.Add("TenMau", typeof(string));
            dataTable.Columns.Add("TenCongDung", typeof(string));
            dataTable.Columns.Add("TenNSX", typeof(string));
            dataTable.Columns.Add("SoLuong", typeof(int));
            dataTable.Columns.Add("DonGiaNhap", typeof(decimal));
            dataTable.Columns.Add("DonGiaBan", typeof(decimal));
            dataTable.Columns.Add("ThoiGianBaoHanh", typeof(int));
            dataTable.Columns.Add("Anh", typeof(string));
            dataTable.Columns.Add("GhiChu", typeof(string));
            foreach (var product in products)
            {
                dataTable.Rows.Add(
                    product.MaHang,
                    product.TenHangHoa,
                    product.TenLoai ?? string.Empty,
                    product.TenKichThuoc ?? string.Empty,
                    product.TenHinhDang ?? string.Empty,
                    product.TenChatLieu ?? string.Empty,
                    product.TenNuocSX ?? string.Empty,
                    product.TenDacDiem ?? string.Empty,
                    product.TenMau ?? string.Empty,
                    product.TenCongDung ?? string.Empty,
                    product.TenNSX ?? string.Empty,
                    product.SoLuong,
                    product.DonGiaNhap,
                    product.DonGiaBan,
                    product.ThoiGianBaoHanh,
                    product.Anh ?? string.Empty,
                    product.GhiChu ?? string.Empty
                );
            }
            return dataTable;
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

        public DataTable GetProductWithAllAttribute()
        {
            string query = "SELECT DMHangHoa.MaHang, DMHangHoa.TenHangHoa, Loai.TenLoai, KichThuoc.TenKichThuoc, HinhDang.TenHinhDang, " +
                                "ChatLieu.TenChatLieu, NuocSX.TenNuocSX, DacDiem.TenDacDiem, MauSac.TenMau, CongDung.TenCongDung, " +
                                "NhaSanXuat.TenNSX, DMHangHoa.DonGiaBan " +
                            "FROM DMHangHoa " +
                                "INNER JOIN Loai ON DMHangHoa.MaLoai = Loai.MaLoai " +
                                "INNER JOIN KichThuoc ON DMHangHoa.MaKichThuoc = KichThuoc.MaKichThuoc " +
                                "INNER JOIN HinhDang ON DMHangHoa.MaHinhDang = HinhDang.MaHinhDang " +
                                "INNER JOIN ChatLieu ON DMHangHoa.MaChatLieu = ChatLieu.MaChatLieu " +
                                "INNER JOIN NuocSX ON DMHangHoa.MaNuocSX = NuocSX.MaNuocSX " +
                                "INNER JOIN DacDiem ON DMHangHoa.MaDacDiem = DacDiem.MaDacDiem " +
                                "INNER JOIN MauSac ON DMHangHoa.MaMau = MauSac.MaMau " +
                                "INNER JOIN CongDung ON DMHangHoa.MaCongDung = CongDung.MaCongDung " +
                                "INNER JOIN NhaSanXuat ON DMHangHoa.MaNSX = NhaSanXuat.MaNSX";
            return DatabaseManager.Instance.ExecuteQuery(query);
        }

    }
}
