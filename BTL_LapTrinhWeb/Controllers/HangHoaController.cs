using BTL_LapTrinhWeb.Data;
using BTL_LapTrinhWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_LapTrinhWeb.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly Hshop2023Context db;
        public HangHoaController(Hshop2023Context context)
        {
            db = context;
        }
        //public IActionResult Index(int? loai)
        //{
        //    var hangHoas = db.HangHoas.AsQueryable();
        //    if (loai.HasValue)
        //    {
        //        hangHoas = hangHoas.Where(p=>p.MaLoai == loai.Value);
        //    }
        //    var result = hangHoas.Select(p => new HangHoaVM
        //    {
        //        Mahh = p.MaHh,
        //        Tenhh = p.TenHh,
        //        DonGia = p.DonGia ?? 0,
        //        Hinh = p.Hinh ?? "",
        //        MoTaNgan = p.MoTaDonVi ?? "",
        //        TenLoai = p.MaLoaiNavigation.TenLoai
        //    }); 
        //    ViewBag.Loai = loai;
        //    return View(result);
        //}
        public IActionResult Index(int? loai, int page = 1, int pageSize = 9)
        {
            var hangHoas = db.HangHoas.AsQueryable();

            // Lọc theo loại nếu có
            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);
            }

            // Tổng số mục
            int totalItems = hangHoas.Count();

            // Lấy danh sách sản phẩm sau khi phân trang
            var result = hangHoas
                .Select(p => new HangHoaVM
                {
                    Mahh = p.MaHh,
                    Tenhh = p.TenHh,
                    DonGia = p.DonGia ?? 0,
                    Hinh = p.Hinh ?? "",
                    MoTaNgan = p.MoTaDonVi ?? "",
                    TenLoai = p.MaLoaiNavigation.TenLoai
                })
                .Skip((page - 1) * pageSize) // Bỏ qua các mục của trang trước
                .Take(pageSize)              // Lấy số mục của trang hiện tại
                .ToList();

            // Tổng số trang
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // ViewBag để truyền thông tin phân trang sang view
            ViewBag.Loai = loai;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View(result);
        }

        public IActionResult Search(string? query)
        {
            var hangHoas = db.HangHoas.AsQueryable();
            if (query != null)
            {
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
            }
            var result = hangHoas.Select(p => new HangHoaVM
            {
                Mahh = p.MaHh,
                Tenhh = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });
            return View(result);
        }
        public IActionResult Detail(int id)
        {
            var data = db.HangHoas.Include(p => p.MaLoaiNavigation).SingleOrDefault(p => p.MaHh == id);
            if (data == null)
            {
                TempData["Message"] = "khong tim thay ";
                return RedirectToAction("/404");
            }
            var result = new ChiTietHangHoaVM
            {
                Mahh = data.MaHh,
                Tenhh = data.TenHh,
                DonGia = data.DonGia ?? 0,
                ChiTiet = data.MoTa ?? string.Empty,
                Hinh = data.Hinh ?? string.Empty,
                MoTaNgan = data.MoTaDonVi ?? string.Empty,
                TenLoai = data.MaLoaiNavigation.TenLoai,
                SoLuongTon = 10,
                DiemDanhGia = 5
            };
            return View(result);
        }
    }
}
