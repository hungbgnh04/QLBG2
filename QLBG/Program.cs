using System;
using System.Windows.Forms;
using QLBG.Helpers;
using QLBG.Views; 
using QLBG.Views.Access; 

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

            if (Session.LoadAuthToken() && Session.IsSessionValid())
            {
                Application.Run(new frmLayout());
            }
            else
            {
                Application.Run(new LoginForm());
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
