using System;
using System.Data;
using System.Data.SqlClient;

namespace QLBG.DAL
{
    public class DatabaseHelper
    {
        private readonly DatabaseManager dbManager;

        public DatabaseHelper()
        {
            dbManager = DatabaseManager.Instance;
        }

        public bool CheckLogin(string email, string password)
        {
            string query = "SELECT COUNT(1) FROM NhanVien WHERE Email=@Email AND Password=@Password";
            SqlParameter[] parameters = {
                new SqlParameter("@Email", email),
                new SqlParameter("@Password", password)
            };
            object result = dbManager.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) == 1;
        }

        public bool CheckEmailExist(string email)
        {
            string query = "SELECT COUNT(1) FROM NhanVien WHERE Email=@Email";
            SqlParameter[] parameters = {
                new SqlParameter("@Email", email)
            };
            object result = dbManager.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) == 1;
        }

        public bool UpdatePassword(string email, string newPassword)
        {
            string query = "UPDATE NhanVien SET Password=@Password WHERE Email=@Email";
            SqlParameter[] parameters = {
                new SqlParameter("@Password", newPassword),
                new SqlParameter("@Email", email)
            };
            int rowsAffected = dbManager.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        public DataTable GetEmployeesWithJob()
        {
            string query = @"
                SELECT 
                    NV.MaNV,
                    NV.TenNV,
                    NV.GioiTinh,
                    NV.NgaySinh,
                    NV.DienThoai,
                    NV.QuyenAdmin,
                    NV.DiaChi,
                    NV.Anh,
                    CV.TenCV
                FROM 
                    NhanVien NV
                INNER JOIN 
                    CongViec CV ON NV.MaCV = CV.MaCV";
            return dbManager.ExecuteDataTable(query, null);
        }

        public bool AddEmployee(string tenNV, bool gioiTinh, DateTime ngaySinh, string dienThoai, string password, bool quyenAdmin, string email, string diaChi, string anh, int maCV)
        {
            string query = @"
                INSERT INTO NhanVien (TenNV, GioiTinh, NgaySinh, DienThoai, Password, QuyenAdmin, Email, DiaChi, Anh, MaCV)
                VALUES (@TenNV, @GioiTinh, @NgaySinh, @DienThoai, @Password, @QuyenAdmin, @Email, @DiaChi, @Anh, @MaCV)";

            SqlParameter[] parameters = {
                new SqlParameter("@TenNV", tenNV),
                new SqlParameter("@GioiTinh", gioiTinh),
                new SqlParameter("@NgaySinh", ngaySinh),
                new SqlParameter("@DienThoai", dienThoai),
                new SqlParameter("@Password", password),
                new SqlParameter("@QuyenAdmin", quyenAdmin),
                new SqlParameter("@Email", email),
                new SqlParameter("@DiaChi", diaChi),
                new SqlParameter("@Anh", anh ?? (object)DBNull.Value),
                new SqlParameter("@MaCV", maCV)
            };

            int rowsAffected = dbManager.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        public DataTable GetAllCongViec()
        {
            string query = "SELECT MaCV, TenCV FROM CongViec";
            return dbManager.ExecuteDataTable(query, null);
        }

        public DataRow GetEmployeeByMaNV(int maNV)
        {
            string query = "SELECT * FROM NhanVien WHERE MaNV=@MaNV";
            SqlParameter[] parameters = {
                new SqlParameter("@MaNV", maNV)
            };
            DataTable result = dbManager.ExecuteDataTable(query, parameters);
            return result.Rows.Count > 0 ? result.Rows[0] : null;
        }

        public bool UpdateEmployee(int maNV, string tenNV, bool gioiTinh, DateTime ngaySinh, string dienThoai, string email, string diaChi, string anh, int maCV, bool quyenAdmin, string password)
        {
            string query = @"
            UPDATE NhanVien 
            SET TenNV=@TenNV, GioiTinh=@GioiTinh, NgaySinh=@NgaySinh, DienThoai=@DienThoai, 
                Email=@Email, DiaChi=@DiaChi, Anh=@Anh, MaCV=@MaCV, QuyenAdmin=@QuyenAdmin, Password=@Password
            WHERE MaNV=@MaNV";

                SqlParameter[] parameters = {
            new SqlParameter("@MaNV", maNV),
            new SqlParameter("@TenNV", tenNV),
            new SqlParameter("@GioiTinh", gioiTinh),
            new SqlParameter("@NgaySinh", ngaySinh),
            new SqlParameter("@DienThoai", dienThoai),
            new SqlParameter("@Email", email),
            new SqlParameter("@DiaChi", diaChi),
            new SqlParameter("@Anh", anh ?? (object)DBNull.Value),
            new SqlParameter("@MaCV", maCV),
            new SqlParameter("@QuyenAdmin", quyenAdmin),
            new SqlParameter("@Password", password)
                };

            int rowsAffected = dbManager.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }


        public bool DeleteEmployee(int maNV)
        {
            string query = "DELETE FROM NhanVien WHERE MaNV=@MaNV";
            SqlParameter[] parameters = {
                new SqlParameter("@MaNV", maNV)
            };
            int rowsAffected = dbManager.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        public bool CheckEmailExistForUpdate(string email, int maNV)
        {
            string query = "SELECT COUNT(1) FROM NhanVien WHERE Email=@Email AND MaNV<>@MaNV";
            SqlParameter[] parameters = {
                new SqlParameter("@Email", email),
                new SqlParameter("@MaNV", maNV)
            };
            object result = dbManager.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) == 1;
        }

        public DataRow GetEmployeeByEmail(string email)
        {
            string query = "SELECT * FROM NhanVien WHERE Email=@Email";
            SqlParameter[] parameters = {
                new SqlParameter("@Email", email)
            };
            DataTable result = dbManager.ExecuteDataTable(query, parameters);
            return result.Rows.Count > 0 ? result.Rows[0] : null;
        }
    }
}
