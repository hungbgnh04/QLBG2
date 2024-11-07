using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace QLBG.DAL
{
    public class CongViecDAL
    {
        private readonly DatabaseManager dbManager;

        public CongViecDAL()
        {
            dbManager = DatabaseManager.Instance;
        }

        /// <summary>
        /// Lấy tất cả công việc.
        /// </summary>
        public DataTable GetAllCongViec()
        {
            string query = "SELECT MaCV, TenCV, MucLuong FROM CongViec";
            return dbManager.ExecuteDataTable(query, null);
        }

        /// <summary>
        /// Lấy thông tin công việc theo mã công việc.
        /// </summary>
        public DataRow GetCongViecById(int maCV)
        {
            string query = "SELECT MaCV, TenCV, MucLuong FROM CongViec WHERE MaCV = @MaCV";
            SqlParameter[] parameters = {
                new SqlParameter("@MaCV", maCV)
            };
            DataTable dataTable = dbManager.ExecuteDataTable(query, parameters);
            return dataTable.Rows.Count > 0 ? dataTable.Rows[0] : null;
        }

        /// <summary>
        /// Thêm công việc mới.
        /// </summary>
        public bool AddCongViec(string tenCV, decimal mucLuong)
        {
            string query = "INSERT INTO CongViec (TenCV, MucLuong) VALUES (@TenCV, @MucLuong)";
            SqlParameter[] parameters = {
                new SqlParameter("@TenCV", tenCV),
                new SqlParameter("@MucLuong", mucLuong)
            };
            int rowsAffected = dbManager.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Cập nhật thông tin công việc.
        /// </summary>
        public bool UpdateCongViec(int maCV, string tenCV, decimal mucLuong)
        {
            string query = "UPDATE CongViec SET TenCV = @TenCV, MucLuong = @MucLuong WHERE MaCV = @MaCV";
            SqlParameter[] parameters = {
                new SqlParameter("@MaCV", maCV),
                new SqlParameter("@TenCV", tenCV),
                new SqlParameter("@MucLuong", mucLuong)
            };
            int rowsAffected = dbManager.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Xóa công việc theo mã công việc.
        /// </summary>
        public bool DeleteCongViec(int maCV)
        {
            string query = "DELETE FROM CongViec WHERE MaCV = @MaCV";
            SqlParameter[] parameters = {
                new SqlParameter("@MaCV", maCV)
            };
            int rowsAffected = dbManager.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Kiểm tra xem công việc với tên đã tồn tại chưa (dùng khi cập nhật).
        /// </summary>
        public bool CheckCongViecExist(string tenCV, int? maCV = null)
        {
            string query = "SELECT COUNT(1) FROM CongViec WHERE TenCV = @TenCV";
            if (maCV != null)
            {
                query += " AND MaCV != @MaCV";
            }

            var parameters = new List<SqlParameter> {
                new SqlParameter("@TenCV", tenCV)
            };

            if (maCV != null)
            {
                parameters.Add(new SqlParameter("@MaCV", maCV.Value));
            }

            object result = dbManager.ExecuteScalar(query, parameters.ToArray());
            return result != null && Convert.ToInt32(result) == 1;
        }
    }
}
