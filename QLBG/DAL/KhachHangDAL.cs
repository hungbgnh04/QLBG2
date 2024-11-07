using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace QLBG.DAL
{
    internal class KhachHangDAL
    {
        private readonly DatabaseManager dbManager;

        public KhachHangDAL()
        {
            dbManager = DatabaseManager.Instance;
        }

        /// <summary>
        /// Lấy tất cả khách hàng.
        /// </summary>
        public DataTable GetAllKhachHang()
        {
            string query = "SELECT MaKhach, TenKhach, DiaChi, DienThoai FROM KhachHang";
            return dbManager.ExecuteDataTable(query, null);
        }

        /// <summary>
        /// Lấy thông tin khách hàng theo mã khách hàng.
        /// </summary>
        public DataRow GetKhachHangById(int maKhach)
        {
            string query = "SELECT MaKhach, TenKhach, DiaChi, DienThoai FROM KhachHang WHERE MaKhach = @MaKhach";
            SqlParameter[] parameters = {
                new SqlParameter("@MaKhach", maKhach)
            };
            DataTable dataTable = dbManager.ExecuteDataTable(query, parameters);
            return dataTable.Rows.Count > 0 ? dataTable.Rows[0] : null;
        }

        /// <summary>
        /// Thêm khách hàng mới.
        /// </summary>
        public bool AddKhachHang(string tenKhach, string diaChi, string dienThoai)
        {
            string query = @"
                INSERT INTO KhachHang (TenKhach, DiaChi, DienThoai)
                VALUES (@TenKhach, @DiaChi, @DienThoai)";
            SqlParameter[] parameters = {
                new SqlParameter("@TenKhach", tenKhach),
                new SqlParameter("@DiaChi", diaChi),
                new SqlParameter("@DienThoai", dienThoai)
            };
            int rowsAffected = dbManager.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Cập nhật thông tin khách hàng.
        /// </summary>
        public bool UpdateKhachHang(int maKhach, string tenKhach, string diaChi, string dienThoai)
        {
            string query = @"
                UPDATE KhachHang
                SET TenKhach = @TenKhach, DiaChi = @DiaChi, DienThoai = @DienThoai
                WHERE MaKhach = @MaKhach";
            SqlParameter[] parameters = {
                new SqlParameter("@MaKhach", maKhach),
                new SqlParameter("@TenKhach", tenKhach),
                new SqlParameter("@DiaChi", diaChi),
                new SqlParameter("@DienThoai", dienThoai)
            };
            int rowsAffected = dbManager.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Xóa khách hàng.
        /// </summary>
        public bool DeleteKhachHang(int maKhach)
        {
            string query = "DELETE FROM KhachHang WHERE MaKhach = @MaKhach";
            SqlParameter[] parameters = {
                new SqlParameter("@MaKhach", maKhach)
            };
            int rowsAffected = dbManager.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Kiểm tra xem khách hàng với số điện thoại đã tồn tại chưa (ngoại trừ bản ghi hiện tại khi cập nhật).
        /// </summary>
        public bool CheckKhachHangExist(string dienThoai, int? maKhach = null)
        {
            string query = "SELECT COUNT(1) FROM KhachHang WHERE DienThoai = @DienThoai";
            if (maKhach != null)
            {
                query += " AND MaKhach != @MaKhach";
            }

            var parameters = new List<SqlParameter> {
                new SqlParameter("@DienThoai", dienThoai)
            };

            if (maKhach != null)
            {
                parameters.Add(new SqlParameter("@MaKhach", maKhach.Value));
            }

            object result = dbManager.ExecuteScalar(query, parameters.ToArray());
            return result != null && Convert.ToInt32(result) == 1;
        }
    }
}
