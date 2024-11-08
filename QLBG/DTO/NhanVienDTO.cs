using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBG.DTO
{
    internal class NhanVienDTO
    {
        public int MaNV { get; set; }
        public string TenNV { get; set; }
        public bool GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string DienThoai { get; set; }
        public string Password { get; set; }
        public bool QuyenAdmin { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string Anh { get; set; }
        public int MaCV { get; set; } // Mã công việc
        public string TenCongViec { get; set; }
    }
}
