using QLBG.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace QLBG.DAL
{
    public class HinhDangDAL
    {
        public List<HinhDangDTO> GetAllHinhDang()
        {
            List<HinhDangDTO> hinhDangList = new List<HinhDangDTO>();
            string query = "SELECT MaHinhDang, TenHinhDang FROM HinhDang";

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                HinhDangDTO hinhDang = new HinhDangDTO
                {
                    MaHinhDang = (int)row["MaHinhDang"],
                    TenHinhDang = row["TenHinhDang"].ToString()
                };
                hinhDangList.Add(hinhDang);
            }

            return hinhDangList;
        }

        public bool UpdateHinhDang(HinhDangDTO hinhDang)
        {
            string query = "UPDATE HinhDang SET TenHinhDang = @TenHinhDang WHERE MaHinhDang = @MaHinhDang";
            SqlParameter[] parameters = {
                new SqlParameter("@TenHinhDang", hinhDang.TenHinhDang),
                new SqlParameter("@MaHinhDang", hinhDang.MaHinhDang)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteHinhDang(int maHinhDang)
        {
            string query = "DELETE FROM HinhDang WHERE MaHinhDang = @MaHinhDang";
            SqlParameter[] parameters = {
                new SqlParameter("@MaHinhDang", maHinhDang)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool InsertHinhDang(HinhDangDTO newHinhDang)
        {
            string query = "INSERT INTO HinhDang (TenHinhDang) VALUES (@TenHinhDang);";
            SqlParameter[] parameters = {
                new SqlParameter("@TenHinhDang", newHinhDang.TenHinhDang)
            };

            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        internal HinhDangDTO GetHinhDangById(int maHinhDang)
        {
            string query = "SELECT MaHinhDang, TenHinhDang FROM HinhDang WHERE MaHinhDang = @MaHinhDang";
            SqlParameter parameter = new SqlParameter("@MaHinhDang", maHinhDang);

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query, parameter);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new HinhDangDTO
                {
                    MaHinhDang = (int)row["MaHinhDang"],
                    TenHinhDang = row["TenHinhDang"].ToString()
                };
            }

            return null;
        }
    }
}
