using QLBG.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace QLBG.DAL
{
    public class CongDungDAL
    {
        public List<CongDungDTO> GetAllCongDung()
        {
            List<CongDungDTO> congDungList = new List<CongDungDTO>();
            string query = "SELECT MaCongDung, TenCongDung FROM CongDung";

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                CongDungDTO congDung = new CongDungDTO
                {
                    MaCongDung = (int)row["MaCongDung"],
                    TenCongDung = row["TenCongDung"].ToString()
                };
                congDungList.Add(congDung);
            }

            return congDungList;
        }

        public bool UpdateCongDung(CongDungDTO congDung)
        {
            string query = "UPDATE CongDung SET TenCongDung = @TenCongDung WHERE MaCongDung = @MaCongDung";
            SqlParameter[] parameters = {
                new SqlParameter("@TenCongDung", congDung.TenCongDung),
                new SqlParameter("@MaCongDung", congDung.MaCongDung)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteCongDung(int maCongDung)
        {
            string query = "DELETE FROM CongDung WHERE MaCongDung = @MaCongDung";
            SqlParameter[] parameters = {
                new SqlParameter("@MaCongDung", maCongDung)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool InsertCongDung(CongDungDTO newCongDung)
        {
            string query = "INSERT INTO CongDung (TenCongDung) VALUES (@TenCongDung);";
            SqlParameter[] parameters = {
                new SqlParameter("@TenCongDung", newCongDung.TenCongDung)
            };

            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        internal CongDungDTO GetCongDungById(int maCongDung)
        {
            string query = "SELECT MaCongDung, TenCongDung FROM CongDung WHERE MaCongDung = @MaCongDung";
            SqlParameter parameter = new SqlParameter("@MaCongDung", maCongDung);

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query, parameter);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new CongDungDTO
                {
                    MaCongDung = (int)row["MaCongDung"],
                    TenCongDung = row["TenCongDung"].ToString()
                };
            }

            return null;
        }
    }
}
