using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BTL_LapTrinhWeb.Areas.Admin.ViewModels
{
    public class RevenueVM
    {
        public int TodaySales { get; set; }
        public int TotalSale { get; set; }
        public double TodayRevenue { get; set; }
        public double TotalRevenue { get; set; }
        public List<double> MonthlyRevenue { get; set; }
        public List<int> MonthlySales { get; set; }

        public List<RecentSales> RecentSales { get; set; } = new List<RecentSales>();
    }
    public class RecentSales
    {
        [Display(Name = "Ngày Đặt")]
        public DateTime NgayDat { get; set; }

        [Display(Name = "Tên khách hàng")]
        public string TenKH { get; set; }

        [Display(Name = "Mã hóa đơn")]
        public int MaHD { get; set; }

        [Display(Name = "Tổng tiền")]
        public string TongTienHD { get; set; }

        [Display(Name = "Trạng thái")]
        public string TrangThai {  get; set; }
    }
}
