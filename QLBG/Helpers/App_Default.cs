using System;
using System.IO;

namespace QLBG.Helpers
{
    public static class App_Default
    {
        // Thông tin email cấu hình
        public static string SenderEmail { get; } = "realer190904@gmail.com";
        public static string SenderPassword { get; } = "ijfaldgglybtrkgh";
        public static string SmtpHost { get; } = "smtp.gmail.com";
        public static int SmtpPort { get; } = 587;

        // ConnectionString động đến tệp QLBG.mdf trong thư mục App_Data
        public static string ConnectionString
        {
            get
            {
                // Lấy đường dẫn đến thư mục gốc của dự án và xác định đường dẫn đến tệp cơ sở dữ liệu
                string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent?.Parent?.FullName;
                if (projectDirectory == null)
                    throw new InvalidOperationException("Could not locate the project directory.");

                string dbFilePath = Path.Combine(projectDirectory, "App_Data", "QLBG.mdf");
                if (!File.Exists(dbFilePath))
                    throw new FileNotFoundException("Database file not found.", dbFilePath);

                return $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={dbFilePath};Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
            }
        }

        // Kiểm tra cấu hình email
        public static bool ValidateEmailConfig()
        {
            return !string.IsNullOrEmpty(SenderEmail) &&
                   !string.IsNullOrEmpty(SenderPassword) &&
                   !string.IsNullOrEmpty(SmtpHost) &&
                   SmtpPort > 0;
        }
    }
}
