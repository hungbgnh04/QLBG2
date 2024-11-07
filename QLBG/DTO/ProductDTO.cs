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
        public int MaKichThuoc { get; set; }
        public int MaHinhDang { get; set; }
        public int MaChatLieu { get; set; }
        public int MaNuocSX { get; set; }
        public int MaDacDiem { get; set; }
        public int MaMau { get; set; }
        public int MaCongDung { get; set; }
        public int MaNSX { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGiaNhap { get; set; }
        public decimal DonGiaBan { get; set; }
        public int ThoiGianBaoHanh { get; set; }
        public string Anh { get; set; }
        public string GhiChu { get; set; }
    }
}
