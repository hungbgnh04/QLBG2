using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBG.DTO
{
    public class HoaDonNhapDTO
    {
        public int SoHDN { get; set; }                
        public DateTime NgayNhap { get; set; }      
        public decimal TongTien { get; set; }       
        public string TenNhanVien { get; set; }      
        public string TenNhaCungCap { get; set; }     

        public List<ChiTietHoaDonNhapDTO> ChiTietHoaDonNhap { get; set; } 

        public HoaDonNhapDTO()
        {
            ChiTietHoaDonNhap = new List<ChiTietHoaDonNhapDTO>();
        }
    }

    public class ChiTietHoaDonNhapDTO
    {
        public int MaHang { get; set; }              
        public string TenHangHoa { get; set; }        
        public int SoLuong { get; set; }            
        public decimal DonGia { get; set; }          
        public decimal GiamGia { get; set; }         
        public decimal ThanhTien { get; set; }       
    }
}
