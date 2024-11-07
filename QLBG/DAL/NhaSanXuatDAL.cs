using QLBG.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace QLBG.DAL
{
    public class NhaSanXuatDAL
    {
        public List<NhaSanXuatDTO> GetAllNhaSanXuat()
        {
            List<NhaSanXuatDTO> nhaSanXuatList = new List<NhaSanXuatDTO>();
            string query = "SELECT MaNSX, TenNSX FROM NhaSanXuat";

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                NhaSanXuatDTO nhaSanXuat = new NhaSanXuatDTO
                {
                    MaNSX = (int)row["MaNSX"],
                    TenNSX = row["TenNSX"].ToString()
                };
                nhaSanXuatList.Add(nhaSanXuat);
            }

            return nhaSanXuatList;
        }

        public bool UpdateNhaSanXuat(NhaSanXuatDTO nhaSanXuat)
        {
            string query = "UPDATE NhaSanXuat SET TenNSX = @TenNSX WHERE MaNSX = @MaNSX";
            SqlParameter[] parameters = {
                new SqlParameter("@TenNSX", nhaSanXuat.TenNSX),
                new SqlParameter("@MaNSX", nhaSanXuat.MaNSX)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteNhaSanXuat(int maNSX)
        {
            string query = "DELETE FROM NhaSanXuat WHERE MaNSX = @MaNSX";
            SqlParameter[] parameters = {
                new SqlParameter("@MaNSX", maNSX)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool InsertNhaSanXuat(NhaSanXuatDTO newNhaSanXuat)
        {
            string query = "INSERT INTO NhaSanXuat (TenNSX) VALUES (@TenNSX);";
            SqlParameter[] parameters = {
                new SqlParameter("@TenNSX", newNhaSanXuat.TenNSX)
            };

            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        internal NhaSanXuatDTO GetNhaSanXuatById(int maNSX)
        {
            string query = "SELECT MaNSX, TenNSX FROM NhaSanXuat WHERE MaNSX = @MaNSX";
            SqlParameter parameter = new SqlParameter("@MaNSX", maNSX);

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query, parameter);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new NhaSanXuatDTO
                {
                    MaNSX = (int)row["MaNSX"],
                    TenNSX = row["TenNSX"].ToString()
                };
            }

            return null;
        }


    }
}
