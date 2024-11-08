using System.Text.RegularExpressions;

namespace QLBG.Helpers
{
    public static class Validate
    {
        // Phương thức kiểm tra số điện thoại hợp lệ
        public static bool IsPhoneNumberValid(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            string pattern = @"^0\d{9}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        // Phương thức kiểm tra mật khẩu hợp lệ
        public static bool IsPasswordValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            return password.Length >= 6 && !password.Contains(" ");
        }

        // Phương thức kiểm tra chuỗi có phải là số thực hợp lệ không
        public static bool IsRealNumber(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return double.TryParse(input, out _);
        }

        /// <summary>
        /// Kiểm tra xem email có hợp lệ không.
        /// Email hợp lệ phải có định dạng như "example@example.com".
        /// </summary>
        public static bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
