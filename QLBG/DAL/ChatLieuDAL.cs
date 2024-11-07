using QLBG.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace QLBG.DAL
{
    public class ChatLieuDAL
    {
        public List<ChatLieuDTO> GetAllChatLieu()
        {
            List<ChatLieuDTO> chatLieuList = new List<ChatLieuDTO>();
            string query = "SELECT MaChatLieu, TenChatLieu FROM ChatLieu";

            DataTable dataTable = DatabaseManager.Instance.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                ChatLieuDTO chatLieu = new ChatLieuDTO
                {
                    MaChatLieu = (int)row["MaChatLieu"],
                    TenChatLieu = row["TenChatLieu"].ToString()
                };
                chatLieuList.Add(chatLieu);
            }

            return chatLieuList;
        }

        public bool UpdateChatLieu(ChatLieuDTO chatLieu)
        {
            string query = "UPDATE ChatLieu SET TenChatLieu = @TenChatLieu WHERE MaChatLieu = @MaChatLieu";
            SqlParameter[] parameters = {
                new SqlParameter("@TenChatLieu", chatLieu.TenChatLieu),
                new SqlParameter("@MaChatLieu", chatLieu.MaChatLieu)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteChatLieu(int maChatLieu)
        {
            string query = "DELETE FROM ChatLieu WHERE MaChatLieu = @MaChatLieu";
            SqlParameter[] parameters = {
                new SqlParameter("@MaChatLieu", maChatLieu)
            };
            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool InsertChatLieu(ChatLieuDTO newChatLieu)
        {
            string query = "INSERT INTO ChatLieu (TenChatLieu) VALUES (@TenChatLieu);";
            SqlParameter[] parameters = {
                new SqlParameter("@TenChatLieu", newChatLieu.TenChatLieu)
            };

            return DatabaseManager.Instance.ExecuteNonQuery(query, parameters) > 0;
        }

    }
}
