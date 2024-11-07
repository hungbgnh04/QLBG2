using QLBG.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace QLBG.DAL
{
    public class DacDiemDAL
    {
        public List<DacDiemDTO> GetAllDacDiem()
        {
            List<DacDiemDTO> dacDiemList = new List<DacDiemDTO>();
            string query = "SELECT MaDacDiem, TenDacDiem FROM DacDiem";

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                DacDiemDTO dacDiem = new DacDiemDTO
                {
                    MaDacDiem = (int)row["MaDacDiem"],
                    TenDacDiem = row["TenDacDiem"].ToString()
                };
                dacDiemList.Add(dacDiem);
            }

            return dacDiemList;
        }

        public bool UpdateDacDiem(DacDiemDTO dacDiem)
        {
            string query = "UPDATE DacDiem SET TenDacDiem = @TenDacDiem WHERE MaDacDiem = @MaDacDiem";
            SqlParameter[] parameters = {
                new SqlParameter("@TenDacDiem", dacDiem.TenDacDiem),
                new SqlParameter("@MaDacDiem", dacDiem.MaDacDiem)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteDacDiem(int maDacDiem)
        {
            string query = "DELETE FROM DacDiem WHERE MaDacDiem = @MaDacDiem";
            SqlParameter[] parameters = {
                new SqlParameter("@MaDacDiem", maDacDiem)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool InsertDacDiem(DacDiemDTO newDacDiem)
        {
            string query = "INSERT INTO DacDiem (TenDacDiem) VALUES (@TenDacDiem);";
            SqlParameter[] parameters = {
                new SqlParameter("@TenDacDiem", newDacDiem.TenDacDiem)
            };

            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        internal DacDiemDTO GetDacDiemById(int maDacDiem)
        {
            string query = "SELECT MaDacDiem, TenDacDiem FROM DacDiem WHERE MaDacDiem = @MaDacDiem";
            SqlParameter parameter = new SqlParameter("@MaDacDiem", maDacDiem);

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query, parameter);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new DacDiemDTO
                {
                    MaDacDiem = (int)row["MaDacDiem"],
                    TenDacDiem = row["TenDacDiem"].ToString()
                };
            }

            return null;
        }
    }
}
