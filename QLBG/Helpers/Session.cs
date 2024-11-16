using System;
using Microsoft.Win32;

namespace QLBG.Helpers
{
    public static class Session
    {
        public static int MaNV { get; set; }
        public static bool QuyenAdmin { get; set; }
        public static DateTime LastLoginTime { get; set; }
        public static string AuthToken { get; private set; }

        public static void SetAuthentication(int maNV, bool quyenAdmin)
        {
            MaNV = maNV;
            QuyenAdmin = quyenAdmin;
            AuthToken = Guid.NewGuid().ToString();
            LastLoginTime = DateTime.Now;

            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\QLBGApp");
            key.SetValue("AuthToken", AuthToken);
            key.SetValue("LastLoginTime", LastLoginTime.ToString());
            key.SetValue("MaNV", MaNV);
            key.SetValue("QuyenAdmin", QuyenAdmin);
            key.Close();
        }

        public static bool IsSessionValid()
        {
            TimeSpan sessionDuration = DateTime.Now - LastLoginTime;
            return sessionDuration.TotalMinutes < 30;
        }

        public static void ClearAuthentication()
        {
            MaNV = 0;
            QuyenAdmin = false;
            AuthToken = null;

            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\QLBGApp");
            key.DeleteValue("AuthToken", false);
            key.DeleteValue("LastLoginTime", false);
            key.DeleteValue("MaNV", false);
            key.DeleteValue("QuyenAdmin", false);
            key.Close();
        }

        public static bool LoadAuthToken()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\QLBGApp");
            if (key != null)
            {
                AuthToken = key.GetValue("AuthToken") as string;

                string lastLoginTimeStr = key.GetValue("LastLoginTime") as string;
                DateTime lastLoginTime;
                if (DateTime.TryParse(lastLoginTimeStr, out lastLoginTime))
                {
                    LastLoginTime = lastLoginTime;
                }

                MaNV = Convert.ToInt32(key.GetValue("MaNV", 0));
                QuyenAdmin = Convert.ToBoolean(key.GetValue("QuyenAdmin", false));
                key.Close();

                return true;
            }
            return false;
        }
    }
}
