using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBG.DTO
{
    internal class HoaDonBanDTO
    {
        public int SoHDB { get; set; }               
        public DateTime NgayBan { get; set; }        
        public int MaNV { get; set; }                
        public string TenNhanVien { get; set; }       
        public int MaKhach { get; set; }            
        public string TenKhachHang { get; set; }      
        public string DiaChiKhachHang { get; set; }   
        public string SDTKhachHang { get; set; }      
        public decimal TongTien { get; set; }
    }
}
