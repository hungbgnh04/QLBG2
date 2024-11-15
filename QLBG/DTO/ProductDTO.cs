using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBG.DTO
{
    public class ProductDTO
    {
        public int MaHang { get; set; }
        public string TenHangHoa { get; set; }
        public int MaLoai { get; set; }
        public string TenLoai { get; set; }
        public int MaKichThuoc { get; set; }
        public string TenKichThuoc { get; set; }
        public int MaHinhDang { get; set; }
        public string TenHinhDang { get; set; }
        public int MaChatLieu { get; set; }
        public string TenChatLieu { get; set; }
        public int MaNuocSX { get; set; }
        public string TenNuocSX { get; set; } 
        public int MaDacDiem { get; set; }
        public string TenDacDiem { get; set; }
        public int MaMau { get; set; }
        public string TenMau { get; set; }
        public int MaCongDung { get; set; }
        public string TenCongDung { get; set; }
        public int MaNSX { get; set; }
        public string TenNSX { get; set; } 
        public int SoLuong { get; set; }
        public decimal DonGiaNhap { get; set; }
        public decimal DonGiaBan { get; set; }
        public int ThoiGianBaoHanh { get; set; }
        public string Anh { get; set; }
        public string GhiChu { get; set; }
    }
}
