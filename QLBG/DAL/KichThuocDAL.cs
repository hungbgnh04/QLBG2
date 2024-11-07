using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QLBG.DTO;

namespace QLBG.DAL
{
    public class KichThuocDAL
    {
        public List<KichThuocDTO> GetAllKichThuoc()
        {
            List<KichThuocDTO> kichThuocList = new List<KichThuocDTO>();
            string query = "SELECT MaKichThuoc, TenKichThuoc FROM KichThuoc";

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                KichThuocDTO kichThuoc = new KichThuocDTO
                {
                    MaKichThuoc = (int)row["MaKichThuoc"],
                    TenKichThuoc = row["TenKichThuoc"].ToString()
                };
                kichThuocList.Add(kichThuoc);
            }

            return kichThuocList;
        }

        public bool InsertKichThuoc(KichThuocDTO kichThuoc)
        {
            string query = "INSERT INTO KichThuoc (TenKichThuoc) VALUES (@TenKichThuoc)";
            SqlParameter[] parameters = {
                new SqlParameter("@TenKichThuoc", kichThuoc.TenKichThuoc)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateKichThuoc(KichThuocDTO kichThuoc)
        {
            string query = "UPDATE KichThuoc SET TenKichThuoc = @TenKichThuoc WHERE MaKichThuoc = @MaKichThuoc";
            SqlParameter[] parameters = {
                new SqlParameter("@TenKichThuoc", kichThuoc.TenKichThuoc),
                new SqlParameter("@MaKichThuoc", kichThuoc.MaKichThuoc)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteKichThuoc(int maKichThuoc)
        {
            string query = "DELETE FROM KichThuoc WHERE MaKichThuoc = @MaKichThuoc";
            SqlParameter[] parameters = {
                new SqlParameter("@MaKichThuoc", maKichThuoc)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }
    }
}
