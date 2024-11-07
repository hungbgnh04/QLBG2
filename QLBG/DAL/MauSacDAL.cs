using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QLBG.DTO;

namespace QLBG.DAL
{
    public class MauSacDAL
    {
        public List<MauSacDTO> GetAllMauSac()
        {
            List<MauSacDTO> mauSacList = new List<MauSacDTO>();
            string query = "SELECT MaMau, TenMau FROM MauSac";

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                MauSacDTO mauSac = new MauSacDTO
                {
                    MaMau = (int)row["MaMau"],
                    TenMau = row["TenMau"].ToString()
                };
                mauSacList.Add(mauSac);
            }

            return mauSacList;
        }

        public MauSacDTO SelectById(int maMau)
        {
            string query = "SELECT MaMau, TenMau FROM MauSac WHERE MaMau = @MaMau";
            SqlParameter parameter = new SqlParameter("@MaMau", maMau);

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query, parameter);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new MauSacDTO
                {
                    MaMau = (int)row["MaMau"],
                    TenMau = row["TenMau"].ToString()
                };
            }

            return null;
        }

        public bool InsertMauSac(MauSacDTO mauSac)
        {
            string query = "INSERT INTO MauSac (TenMau) VALUES (@TenMau)";
            SqlParameter[] parameters = {
                new SqlParameter("@TenMau", mauSac.TenMau)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateMauSac(MauSacDTO mauSac)
        {
            string query = "UPDATE MauSac SET TenMau = @TenMau WHERE MaMau = @MaMau";
            SqlParameter[] parameters = {
                new SqlParameter("@TenMau", mauSac.TenMau),
                new SqlParameter("@MaMau", mauSac.MaMau)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteMauSac(int maMau)
        {
            string query = "DELETE FROM MauSac WHERE MaMau = @MaMau";
            SqlParameter[] parameters = {
                new SqlParameter("@MaMau", maMau)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }
    }
}
