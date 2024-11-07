using QLBG.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace QLBG.DAL
{
    public class NuocSXDAL
    {
        public List<NuocSXDTO> GetAllNuocSX()
        {
            List<NuocSXDTO> nuocSXList = new List<NuocSXDTO>();
            string query = "SELECT MaNuocSX, TenNuocSX FROM NuocSX";

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                NuocSXDTO nuocSX = new NuocSXDTO
                {
                    MaNuocSX = (int)row["MaNuocSX"],
                    TenNuocSX = row["TenNuocSX"].ToString()
                };
                nuocSXList.Add(nuocSX);
            }

            return nuocSXList;
        }

        public bool UpdateNuocSX(NuocSXDTO nuocSX)
        {
            string query = "UPDATE NuocSX SET TenNuocSX = @TenNuocSX WHERE MaNuocSX = @MaNuocSX";
            SqlParameter[] parameters = {
                new SqlParameter("@TenNuocSX", nuocSX.TenNuocSX),
                new SqlParameter("@MaNuocSX", nuocSX.MaNuocSX)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteNuocSX(int maNuocSX)
        {
            string query = "DELETE FROM NuocSX WHERE MaNuocSX = @MaNuocSX";
            SqlParameter[] parameters = {
                new SqlParameter("@MaNuocSX", maNuocSX)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool InsertNuocSX(NuocSXDTO newNuocSX)
        {
            string query = "INSERT INTO NuocSX (TenNuocSX) VALUES (@TenNuocSX);";
            SqlParameter[] parameters = {
                new SqlParameter("@TenNuocSX", newNuocSX.TenNuocSX)
            };

            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        internal NuocSXDTO GetNuocSXById(int maNuocSX)
        {
            string query = "SELECT MaNuocSX, TenNuocSX FROM NuocSX WHERE MaNuocSX = @MaNuocSX";
            SqlParameter parameter = new SqlParameter("@MaNuocSX", maNuocSX);

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query, parameter);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new NuocSXDTO
                {
                    MaNuocSX = (int)row["MaNuocSX"],
                    TenNuocSX = row["TenNuocSX"].ToString()
                };
            }

            return null;
        }
    }
}
