using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace QLBG.DAL
{
    internal class NhaCungCapDAL
    {
        private readonly DatabaseManager dbManager;

        public NhaCungCapDAL()
        {
            dbManager = DatabaseManager.Instance;
        }

        /// <summary>
        /// Lấy tất cả nhà cung cấp.
        /// </summary>
        public DataTable GetAllNhaCungCap()
        {
            string query = "SELECT MaNCC, TenNCC, DiaChi, DienThoai FROM NhaCungCap";
            return dbManager.ExecuteDataTable(query, null);
        }

        /// <summary>
        /// Lấy thông tin nhà cung cấp theo mã.
        /// </summary>
        public DataRow GetNhaCungCapById(int maNCC)
        {
            string query = "SELECT MaNCC, TenNCC, DiaChi, DienThoai FROM NhaCungCap WHERE MaNCC = @MaNCC";
            SqlParameter[] parameters = {
                new SqlParameter("@MaNCC", maNCC)
            };
            DataTable dataTable = dbManager.ExecuteDataTable(query, parameters);
            return dataTable.Rows.Count > 0 ? dataTable.Rows[0] : null;
        }

        /// <summary>
        /// Thêm nhà cung cấp mới.
        /// </summary>
        public bool AddNhaCungCap(string tenNCC, string diaChi, string dienThoai)
        {
            string query = @"
                INSERT INTO NhaCungCap (TenNCC, DiaChi, DienThoai)
                VALUES (@TenNCC, @DiaChi, @DienThoai)";
            SqlParameter[] parameters = {
                new SqlParameter("@TenNCC", tenNCC),
                new SqlParameter("@DiaChi", diaChi),
                new SqlParameter("@DienThoai", dienThoai)
            };
            int rowsAffected = dbManager.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Cập nhật thông tin nhà cung cấp.
        /// </summary>
        public bool UpdateNhaCungCap(int maNCC, string tenNCC, string diaChi, string dienThoai)
        {
            string query = @"
                UPDATE NhaCungCap
                SET TenNCC = @TenNCC, DiaChi = @DiaChi, DienThoai = @DienThoai
                WHERE MaNCC = @MaNCC";
            SqlParameter[] parameters = {
                new SqlParameter("@MaNCC", maNCC),
                new SqlParameter("@TenNCC", tenNCC),
                new SqlParameter("@DiaChi", diaChi),
                new SqlParameter("@DienThoai", dienThoai)
            };
            int rowsAffected = dbManager.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Xóa nhà cung cấp.
        /// </summary>
        public bool DeleteNhaCungCap(int maNCC)
        {
            string query = "DELETE FROM NhaCungCap WHERE MaNCC = @MaNCC";
            SqlParameter[] parameters = {
                new SqlParameter("@MaNCC", maNCC)
            };
            int rowsAffected = dbManager.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Kiểm tra xem nhà cung cấp với số điện thoại đã tồn tại chưa.
        /// </summary>
        public bool CheckNhaCungCapExist(string dienThoai, int? maNCC = null)
        {
            string query = "SELECT COUNT(1) FROM NhaCungCap WHERE DienThoai = @DienThoai";
            if (maNCC != null)
            {
                query += " AND MaNCC != @MaNCC";
            }

            var parameters = new List<SqlParameter> {
                new SqlParameter("@DienThoai", dienThoai)
            };

            if (maNCC != null)
            {
                parameters.Add(new SqlParameter("@MaNCC", maNCC.Value));
            }

            object result = dbManager.ExecuteScalar(query, parameters.ToArray());
            return result != null && Convert.ToInt32(result) == 1;
        }
    }
}
