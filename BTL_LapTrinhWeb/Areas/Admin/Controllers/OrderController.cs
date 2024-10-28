using BTL_LapTrinhWeb.Data;
using BTL_LapTrinhWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace BTL_LapTrinhWeb.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/order")]
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
        [Route("themdonhang")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemDonHang(HoaDonAnnotation hoaDonVM)
        {
            TempData["Message"] = "";

            // Kiểm tra xem model có hợp lệ hay không
            if (ModelState.IsValid)
            {
                // Chuyển dữ liệu từ ViewModel sang Model nếu cần
                var hoaDon = new HoaDon
                {
                    MaKh = hoaDonVM.MaKh,
                    HoTen = hoaDonVM.HoTen,
                    DiaChi = hoaDonVM.DiaChi,
                    CachThanhToan = hoaDonVM.CachThanhToan,
                    CachVanChuyen = hoaDonVM.CachVanChuyen,
                    PhiVanChuyen = hoaDonVM.PhiVanChuyen,
                    NgayDat = hoaDonVM.NgayDat,
                    NgayCan = hoaDonVM.NgayCan,
                    NgayGiao = hoaDonVM.NgayGiao,
                    GhiChu = hoaDonVM.GhiChu,
                    MaNv = hoaDonVM.MaNv,
                    MaTrangThai = hoaDonVM.MaTrangThai
                };
                // Thêm vào cơ sở dữ liệu
                db.HoaDons.Add(hoaDon);
                db.SaveChanges();

                TempData["Message"] = "Đã thêm thành công đơn hàng";
                return RedirectToAction("DanhMucDonHang");
            }
            else
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine("Error: " + error.ErrorMessage);
                    }
                }
            }
            ViewBag.MaKh = new SelectList(db.KhachHangs.ToList(), "MaKh", "HoTen");
            ViewBag.MaNv = new SelectList(db.NhanViens.ToList(), "MaNv", "HoTen");
            ViewBag.MaTrangThai = new SelectList(db.TrangThais.ToList(), "MaTrangThai", "TenTrangThai");
            // Nếu model không hợp lệ, trả về trang view với dữ liệu đã nhập
            return View(hoaDonVM);
        }

        [Route("SuaDonHang")]
        [HttpGet]
        public IActionResult SuaDonHang(int Mahd)
        {
            ViewBag.MaKh = new SelectList(db.KhachHangs.ToList(), "MaKh", "HoTen");
            ViewBag.MaNv = new SelectList(db.NhanViens.ToList(), "MaNv", "HoTen");
            ViewBag.MaTrangThai = new SelectList(db.TrangThais.ToList(), "MaTrangThai", "TenTrangThai");
            var hoadon = db.HoaDons.Find(Mahd);
            var hoaDonVM = new HoaDonAnnotation
            {
                MaKh = hoadon.MaKh,
                HoTen = hoadon.HoTen,
                DiaChi = hoadon.DiaChi,
                CachThanhToan = hoadon.CachThanhToan,
                CachVanChuyen = hoadon.CachVanChuyen,
                PhiVanChuyen = hoadon.PhiVanChuyen,
                NgayDat = hoadon.NgayDat,
                NgayCan = hoadon.NgayCan,
                NgayGiao = hoadon.NgayGiao,
                GhiChu = hoadon.GhiChu,
                MaNv = hoadon.MaNv,
                MaTrangThai = hoadon.MaTrangThai
            };
            return View(hoaDonVM);
        }
        [Route("SuaDonHang")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaDonHang(HoaDonAnnotation hoaDonVM)
        {
            TempData["Message"] = "";
            if (ModelState.IsValid)
            {
                var hoaDon = db.HoaDons.Find(hoaDonVM.MaHd);
                hoaDon.MaKh = hoaDonVM.MaKh;
                hoaDon.HoTen = hoaDonVM.HoTen;
                hoaDon.DiaChi = hoaDonVM.DiaChi;
                hoaDon.CachThanhToan = hoaDonVM.CachThanhToan;
                hoaDon.CachVanChuyen = hoaDonVM.CachVanChuyen;
                hoaDon.PhiVanChuyen = hoaDonVM.PhiVanChuyen;
                hoaDon.NgayDat = hoaDonVM.NgayDat;
                hoaDon.NgayCan = hoaDonVM.NgayCan;
                hoaDon.NgayGiao = hoaDonVM.NgayGiao;
                hoaDon.GhiChu = hoaDonVM.GhiChu;
                hoaDon.MaNv = hoaDonVM.MaNv;
                hoaDon.MaTrangThai = hoaDonVM.MaTrangThai;
                db.Update(hoaDon);
                db.SaveChanges();
                TempData["Message"] = "Đã sửa thành công đơn hàng";
                return RedirectToAction("DanhMucDonHang", "Order");
            }
            else
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine("Error: " + error.ErrorMessage);
                    }
                }
            }
            ViewBag.MaKh = new SelectList(db.KhachHangs.ToList(), "MaKh", "HoTen");
            ViewBag.MaNv = new SelectList(db.NhanViens.ToList(), "MaNv", "HoTen");
            ViewBag.MaTrangThai = new SelectList(db.TrangThais.ToList(), "MaTrangThai", "TenTrangThai");
            // Nếu model không hợp lệ, trả về trang view với dữ liệu đã nhập
            return View(hoaDonVM);

        }
        [Route("ChiTietDonHang")]
        [HttpGet]
        public IActionResult ChiTietDonHang(int Mahd)
        {
            var hoaDon = db.HoaDons.Find(Mahd);
            return View(hoaDon);
        }
        [Route("XoaDonHang")]
        [HttpGet]
        public IActionResult XoaDonHang(int Mahd)
        {
            // Khởi tạo thông báo
            TempData["Message"] = "";
            // Tìm đơn hàng theo mã
            var hoaDon = db.HoaDons.Find(Mahd);
            if (hoaDon != null)
            {
                db.HoaDons.Remove(hoaDon); // Xóa đơn hàng
                db.SaveChanges(); // Lưu thay đổi
                TempData["Message"] = "Sản phẩm đã được xóa thành công.";
            }
            else
            {
                TempData["Message"] = "Không tìm thấy đơn hàng.";
            }

            return RedirectToAction("DanhMucDonHang", "Order");
        }

    }
}
