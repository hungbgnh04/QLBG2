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
        public string TenLoai { get; set; }             // Tên loại
        public int MaKichThuoc { get; set; }
        public string TenKichThuoc { get; set; }         // Tên kích thước
        public int MaHinhDang { get; set; }
        public string TenHinhDang { get; set; }          // Tên hình dạng
        public int MaChatLieu { get; set; }
        public string TenChatLieu { get; set; }          // Tên chất liệu
        public int MaNuocSX { get; set; }
        public string TenNuocSX { get; set; }            // Tên nước sản xuất
        public int MaDacDiem { get; set; }
        public string TenDacDiem { get; set; }           // Tên đặc điểm
        public int MaMau { get; set; }
        public string TenMau { get; set; }               // Tên màu
        public int MaCongDung { get; set; }
        public string TenCongDung { get; set; }          // Tên công dụng
        public int MaNSX { get; set; }
        public string TenNSX { get; set; }               // Tên nhà sản xuất
        public int SoLuong { get; set; }
        public decimal DonGiaNhap { get; set; }
        public decimal DonGiaBan { get; set; }
        public int ThoiGianBaoHanh { get; set; }
        public string Anh { get; set; }
        public string GhiChu { get; set; }
    }
}
