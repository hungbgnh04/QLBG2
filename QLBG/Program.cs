using System;
using System.Windows.Forms;
using QLBG.Helpers; // Import để sử dụng Session
using QLBG.Views;   // Import để sử dụng frmLayout và frmLogin
using QLBG.Views.Access; // Import để sử dụng LoginForm

namespace QLBG
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Kiểm tra Authentication Token
            if (Session.LoadAuthToken() && Session.IsSessionValid())
            {
                // Mở form chính nếu có token hợp lệ
                Application.Run(new frmLayout());
            }
            else
            {
                // Mở form đăng nhập nếu không có token hoặc phiên đã hết hạn
                Application.Run(new LoginForm());
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
