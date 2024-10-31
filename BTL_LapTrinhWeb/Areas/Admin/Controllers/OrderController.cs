using BTL_LapTrinhWeb.Data;
using BTL_LapTrinhWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace BTL_LapTrinhWeb.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/order")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly Hshop2023Context db;
        public OrderController(Hshop2023Context _db)
        {
            db = _db;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("danhmucdonhang")]
        public IActionResult DanhMucDonHang(int page = 1, int pageSize = 12)
        {
            // Tính tổng số đơn hàng trước khi phân trang
            int totalItems = db.HoaDons.Count();

            // Lấy danh sách đơn hàng và sắp xếp theo thứ tự giảm dần
            var lst = db.HoaDons.OrderByDescending(h => h.MaHd)
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();

            // Tính tổng số trang dựa trên tổng số đơn hàng
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Truyền thông tin phân trang cho View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            return View(lst);
        }
        [Route("themdonhang")]
        [HttpGet]
        public IActionResult ThemDonHang()
        {
            ViewBag.MaKh = new SelectList(db.KhachHangs.ToList(), "MaKh", "HoTen");
            ViewBag.MaNv = new SelectList(db.NhanViens.ToList(), "MaNv", "HoTen");
            ViewBag.MaTrangThai = new SelectList(db.TrangThais.ToList(), "MaTrangThai", "TenTrangThai");
            return View();
        }
        [Route("ChiTietDonHang")]
        [HttpGet]
        public IActionResult ChiTietDonHang(int Mahd)
        {
            var hoaDon = db.HoaDons.Find(Mahd);

            var data = new HoaDonAnnotation
            {
                HoTen = hoaDon.HoTen,
                NgayCan = hoaDon.NgayCan,
                NgayDat = hoaDon.NgayDat,
                NgayGiao = hoaDon.NgayGiao,
                DiaChi = hoaDon.DiaChi,
                CachVanChuyen = hoaDon.CachVanChuyen,
                CachThanhToan = hoaDon.CachThanhToan,
                PhiVanChuyen = hoaDon.PhiVanChuyen,
            };

            return View(data);
        }

    }
}
