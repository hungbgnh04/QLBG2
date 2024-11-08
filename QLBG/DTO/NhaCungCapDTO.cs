using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBG.DTO
{
    internal class NhaCungCapDTO
    {
        public int MaNCC { get; set; }         // Mã nhà cung cấp
        public string TenNCC { get; set; }     // Tên nhà cung cấp
        public string DiaChi { get; set; }     // Địa chỉ nhà cung cấp
        public string DienThoai { get; set; }
    }
}
