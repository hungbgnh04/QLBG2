using System;
using QLBG.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace QLBG.DAL
{
    internal class DatabaseManager
    //
    {
        private static DatabaseManager instance;
        private readonly string connectionString;

        private DatabaseManager()
        {
            connectionString = App_Default.ConnectionString;
        }

        public static DatabaseManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DatabaseManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// Opens the database connection if it's closed.
        /// </summary>
        public void OpenConnection(SqlConnection connection)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        /// <summary>
        /// Closes the database connection if it's open.
        /// </summary>
        public void CloseConnection(SqlConnection connection)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Executes a query and returns the results in a DataTable.
        /// </summary>
        public DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    OpenConnection(connection);

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        try
                        {
                            adapter.Fill(dataTable);
                        }
                        catch (Exception ex)
                        {
                            ShowErrorMessage("Error executing query: " + ex.Message);
                        }
                    }
                }
            }
            return dataTable;
        }

        /// <summary>
        /// Executes a non-query SQL command (INSERT, UPDATE, DELETE).
        /// </summary>
        public int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction()) // Bắt đầu giao dịch
                using (SqlCommand command = new SqlCommand(query, connection, transaction))
                {
                    command.Parameters.AddRange(parameters);

                    try
                    {
                        int result = command.ExecuteNonQuery();
                        transaction.Commit(); // Cam kết giao dịch nếu lệnh thành công
                        Console.WriteLine($"ExecuteNonQuery: {query}, RowsAffected: {result}");
                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // Rollback giao dịch nếu có lỗi
                        ShowErrorMessage("Error executing non-query: " + ex.Message);
                        return -1;
                    }
                }
            }
        }



        /// <summary>
        /// Executes a scalar SQL command and returns the result.
        /// </summary>
        public object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters);
                    OpenConnection(connection);
                    try
                    {
                        return command.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("Error executing scalar: " + ex.Message);
                        return null;
                    }
                }
            }
        }

            /// <summary>
            /// Displays an error message to the user.
            /// </summary>
        internal void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

            /// <summary>
            /// Displays a success message to the user.
            /// </summary>
        internal void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public DataTable ExecuteDataTable(string query, SqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }

    }
}