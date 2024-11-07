using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QLBG.DTO;

namespace QLBG.DAL
{
    public class LoaiDAL
    {
        public List<LoaiDTO> GetAllLoai()
        {
            List<LoaiDTO> loaiList = new List<LoaiDTO>();
            string query = "SELECT MaLoai, TenLoai FROM Loai";

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                LoaiDTO loai = new LoaiDTO
                {
                    MaLoai = (int)row["MaLoai"],
                    TenLoai = row["TenLoai"].ToString()
                };
                loaiList.Add(loai);
            }

            return loaiList;
        }

        public LoaiDTO GetLoaiById(int maLoai)
        {
            string query = "SELECT MaLoai, TenLoai FROM Loai WHERE MaLoai = @MaLoai";
            SqlParameter parameter = new SqlParameter("@MaLoai", maLoai);

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query, parameter);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new LoaiDTO
                {
                    MaLoai = (int)row["MaLoai"],
                    TenLoai = row["TenLoai"].ToString()
                };
            }
            return null;
        }

        public bool InsertLoai(LoaiDTO loai)
        {
            string query = "INSERT INTO Loai (TenLoai) VALUES (@TenLoai)";
            SqlParameter[] parameters = {
                new SqlParameter("@TenLoai", loai.TenLoai)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateLoai(LoaiDTO loai)
        {
            string query = "UPDATE Loai SET TenLoai = @TenLoai WHERE MaLoai = @MaLoai";
            SqlParameter[] parameters = {
                new SqlParameter("@TenLoai", loai.TenLoai),
                new SqlParameter("@MaLoai", loai.MaLoai)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteLoai(int maLoai)
        {
            string query = "DELETE FROM Loai WHERE MaLoai = @MaLoai";
            SqlParameter[] parameters = {
                new SqlParameter("@MaLoai", maLoai)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }
    }
}
