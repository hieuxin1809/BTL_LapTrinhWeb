using BTL_LapTrinhWeb.Areas.Admin.ViewModels;
using BTL_LapTrinhWeb.Data;
using BTL_LapTrinhWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BTL_LapTrinhWeb.Areas.Admin.Controllers
{
	[Area("admin")]
	[Route("admin")]
	[Route("admin/homeadmin")]
	[Authorize(Roles = "Admin")]
	public class HomeAdminController : Controller
	{
		Hshop2023Context db = new Hshop2023Context();

		// Tiêm DbContext thông qua constructor
		public HomeAdminController(Hshop2023Context _db)
		{
			db = _db;
		}
		[Route("")]
		[Route("index")]
		public IActionResult Index()
		{
			var monthlySales = new List<int>();
			var monthlyRevenue = new List<double>();

			var currDay = DateTime.Now.Day;
			var currMonth = DateTime.Now.Month;
			var currYear = DateTime.Now.Year;

			for (int i = 1; i <= 12; i++)
			{
				var sales = db.ChiTietHds.Join(db.HoaDons, cthd => cthd.MaHd, hd => hd.MaHd, (cthd, hd) => new { cthd, hd })
					.Where(x => x.hd.NgayDat.Month == i && x.hd.NgayDat.Year == currYear)
					.Select(x => x.cthd.MaHd)
					.Distinct()
					.Count();
				var revenue = db.ChiTietHds.Join(db.HoaDons, cthd => cthd.MaHd, hd => hd.MaHd, (cthd, hd) => new { cthd, hd })
					.Where(x => x.hd.NgayDat.Month == i && x.hd.NgayDat.Year == currYear)
					.Sum(x => x.cthd.SoLuong * x.cthd.DonGia);

				monthlySales.Add(sales);
				monthlyRevenue.Add(revenue);
			}

			var todaySale = db.ChiTietHds.Join(db.HoaDons, cthd => cthd.MaHd, hd => hd.MaHd, (cthd, hd) => new { cthd, hd })
					.Where(x => x.hd.NgayDat.Day == currDay && x.hd.NgayDat.Month == currMonth && x.hd.NgayDat.Year == currYear)
					.Select(x => x.cthd.MaHd)
					.Distinct()
					.Count();

			var todayRevenue = db.ChiTietHds.Join(db.HoaDons, cthd => cthd.MaHd, hd => hd.MaHd, (cthd, hd) => new { cthd, hd })
					.Where(x => x.hd.NgayDat.Day == currDay && x.hd.NgayDat.Month == currMonth && x.hd.NgayDat.Year == currYear)
					.Sum(x => x.cthd.SoLuong * x.cthd.DonGia);

			var recentSales = db.HoaDons.Join(db.ChiTietHds,hd => hd.MaHd,cthd => cthd.MaHd,(hd, cthd) => new { hd, cthd })
					.Join(db.TrangThais,combined => combined.hd.MaTrangThai,trangThai => trangThai.MaTrangThai,
					(combined, trangThai) => new {
						combined.hd.MaHd,
						combined.hd.NgayDat,
						combined.hd.HoTen,
						combined.cthd.SoLuong,
						combined.cthd.DonGia,
						combined.cthd.GiamGia,
					TrangThai = trangThai.TenTrangThai
					})
					.GroupBy(x => new { x.MaHd, x.NgayDat, x.HoTen, x.TrangThai })
					.Select(g => new RecentSales
					{
						NgayDat = g.Key.NgayDat,
						MaHD = g.Key.MaHd,
						TenKH = g.Key.HoTen,
						TrangThai = g.Key.TrangThai,
						TongTienHD = g.Sum(x => x.SoLuong * x.DonGia * (x.GiamGia==0?1:x.GiamGia)).ToString("C0")
					})
					.OrderByDescending(x => x.NgayDat)
					.Take(10)
					.ToList();

			var revenueVM = new RevenueVM
			{
				TodaySales = todaySale,
				TotalSale = monthlySales[currMonth - 1],
				TodayRevenue = todayRevenue,
				TotalRevenue = monthlyRevenue[currMonth - 1],
				MonthlyRevenue = monthlyRevenue,//new List<double> { 500.75, 600.50, 550.30, 700.45, 670.80, 750.25, 800.65, 850.90, 900.10, 950.25, 990.40, 1200.60 },
				MonthlySales = monthlySales, // new List<int> { 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100 }
				RecentSales = recentSales
			};

            return View(revenueVM);
		}
	}
}
