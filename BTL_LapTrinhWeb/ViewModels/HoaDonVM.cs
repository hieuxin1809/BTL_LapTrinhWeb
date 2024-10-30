using BTL_LapTrinhWeb.Data;

namespace BTL_LapTrinhWeb.ViewModels
{
    public class HoaDonVM
    {
        public int MaHd { get; set; }
        public DateTime NgayDat { get; set; }
        public string DiaChi { get; set; } = null!;
        public string CachThanhToan { get; set; } = null!;
        public string CachVanChuyen { get; set; } = null!;
        public string? GhiChu { get; set; }
        public string TenTrangThai { get; set; } = null!;
    }
}
