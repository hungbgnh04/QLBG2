using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBG.DTO
{
    public class HoaDonNhapDTO
    {
        public int SoHDN { get; set; }            // Số hóa đơn nhập
        public DateTime NgayNhap { get; set; }     // Ngày nhập
        public int MaNV { get; set; }              // Mã nhân viên
        public string TenNhanVien { get; set; }    // Tên nhân viên
        public int MaNCC { get; set; }             // Mã nhà cung cấp
        public string TenNhaCungCap { get; set; }  // Tên nhà cung cấp
        public decimal TongTien { get; set; }      // Tổng tiền
    }

}
