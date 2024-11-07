using System;
using System.Text.RegularExpressions;

namespace QLBG.Helpers
{
    public static class Validate
    {
        /// <summary>
        /// Kiểm tra xem số điện thoại có hợp lệ không.
        /// Số điện thoại hợp lệ phải có đúng 10 chữ số và bắt đầu bằng số 0.
        /// </summary>
        public static bool IsPhoneNumberValid(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            // Kiểm tra số điện thoại có 10 chữ số và bắt đầu bằng số 0
            string pattern = @"^0\d{9}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        /// <summary>
        /// Kiểm tra xem mật khẩu có hợp lệ không.
        /// Mật khẩu hợp lệ phải có ít nhất 6 ký tự và không chứa khoảng trắng.
        /// </summary>
        public static bool IsPasswordValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Kiểm tra mật khẩu có ít nhất 6 ký tự và không có khoảng trắng
            return password.Length >= 6 && !password.Contains(" ");
        }

        /// <summary>
        /// Kiểm tra xem chuỗi có phải là số thực hợp lệ không.
        /// </summary>
        public static bool IsRealNumber(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            // Kiểm tra xem chuỗi có phải là số thực không (dấu phẩy hoặc dấu chấm thập phân)
            return double.TryParse(input, out _);
        }
    }
}
