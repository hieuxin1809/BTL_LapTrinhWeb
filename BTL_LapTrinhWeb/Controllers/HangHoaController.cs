using BTL_LapTrinhWeb.Data;
using BTL_LapTrinhWeb.Helpers;
using BTL_LapTrinhWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing.Printing;
using X.PagedList;
using X.PagedList.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BTL_LapTrinhWeb.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly Hshop2023Context db;
        public HangHoaController(Hshop2023Context context)
        {
            db = context;
        }
        public IActionResult Index(int? loai, string sortOrder, string priceRange, string nameOrder, int page = 1, int pageSize = 9)
        {
            var hangHoas = db.HangHoas.AsQueryable();

            // Apply filters and sorting based on parameters
            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);
            }

            if (!string.IsNullOrEmpty(priceRange))
            {
                switch (priceRange)
                {
                    case "under_50000":
                        hangHoas = hangHoas.Where(p => p.DonGia < 50000);
                        break;
                    case "50000_100000":
                        hangHoas = hangHoas.Where(p => p.DonGia >= 50000 && p.DonGia <= 100000);
                        break;
                    case "100000_500000":
                        hangHoas = hangHoas.Where(p => p.DonGia >= 100000 && p.DonGia <= 500000);
                        break;
                    case "over_500000":
                        hangHoas = hangHoas.Where(p => p.DonGia > 500000);
                        break;
                }
            }

            switch (sortOrder)
            {
                case "price_asc":
                    hangHoas = hangHoas.OrderBy(p => p.DonGia);
                    break;
                case "price_desc":
                    hangHoas = hangHoas.OrderByDescending(p => p.DonGia);
                    break;
            }

            switch (nameOrder)
            {
                case "az":
                    hangHoas = hangHoas.OrderBy(p => p.TenHh);
                    break;
                case "za":
                    hangHoas = hangHoas.OrderByDescending(p => p.TenHh);
                    break;
            }

            int totalItems = hangHoas.Count();
            var result = hangHoas
                .Select(p => new HangHoaVM
                {
                    Mahh = p.MaHh,
                    Tenhh = p.TenHh,
                    DonGia = p.DonGia ?? 0,
                    Hinh = p.Hinh ?? "",
                    MoTaNgan = p.MoTaDonVi ?? "",
                    TenLoai = p.MaLoaiNavigation.TenLoai,
                    Rating = p.Rating ?? 0
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Pass parameters to the view to retain current filters and sorting
            ViewBag.Loai = loai;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.SortOrder = sortOrder;
            ViewBag.PriceRange = priceRange;
            ViewBag.NameOrder = nameOrder;

            return View(result);
        }


        public IActionResult Detail(int id, int pageNumber = 1, int pageSize = 5)
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
                Rating = data.Rating ?? 0,
                MoTaNgan = data.MoTaDonVi ?? string.Empty,
                TenLoai = data.MaLoaiNavigation.TenLoai,
                SoLuongTon = 10
            };

            //Lay ra comments
            var totalItems = db.DanhGia.Where(c => c.MaHh == id).ToList().Count;
            var totalPages = (int)Math.Ceiling((double)(totalItems) / pageSize);
            var comments = db.DanhGia
                .Where(c => c.MaHh == id)
                .OrderByDescending(c => c.Date)
                .ToPagedList(pageNumber, pageSize); 

            ViewData["Comments"] = comments;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            return View(result);
        }

        public IActionResult Search(string? query)
        {
            var hangHoas = db.HangHoas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
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
                TenLoai = p.MaLoaiNavigation.TenLoai,
                Rating = (int)(p.Rating ?? 0)

            }).ToList();

            if (!result.Any())
            {
                TempData["Message"] = "không tìm thấy sản phẩm nào.";
                return RedirectToAction("PageNotFound", "Home");
            }

            return View(result);
        }

        public IActionResult SortProducts(int? loai, string sortOrder, string priceRange, string nameOrder, int page = 1, int pageSize = 9)

        {
            var hangHoas = db.HangHoas.AsQueryable();

            // Filter by category (loai) if specified
            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);
            }

            // Filter by price range
            switch (priceRange)
            {
                case "under_50000":
                    hangHoas = hangHoas.Where(p => p.DonGia < 50000);
                    break;
                case "50000_100000":
                    hangHoas = hangHoas.Where(p => p.DonGia >= 50000 && p.DonGia <= 100000);
                    break;
                case "100000_500000":
                    hangHoas = hangHoas.Where(p => p.DonGia >= 100000 && p.DonGia <= 500000);
                    break;
                case "over_500000":
                    hangHoas = hangHoas.Where(p => p.DonGia > 500000);
                    break;
            }

            // Sort by price
            switch (sortOrder)
            {
                case "price_asc":
                    hangHoas = hangHoas.OrderBy(p => p.DonGia);
                    break;
                case "price_desc":
                    hangHoas = hangHoas.OrderByDescending(p => p.DonGia);
                    break;
            }

            // Sort by name
            switch (nameOrder)
            {
                case "az":
                    hangHoas = hangHoas.OrderBy(p => p.TenHh);
                    break;
                case "za":
                    hangHoas = hangHoas.OrderByDescending(p => p.TenHh);
                    break;
            }

            int totalItems = hangHoas.Count();
            var result = hangHoas
                .Select(p => new HangHoaVM
                {
                    Mahh = p.MaHh,
                    Tenhh = p.TenHh,
                    DonGia = p.DonGia ?? 0,
                    Hinh = p.Hinh ?? "",
                    MoTaNgan = p.MoTaDonVi ?? "",
                    TenLoai = p.MaLoaiNavigation.TenLoai,
                    Rating = p.Rating ?? 0
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            ViewBag.Loai = loai;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SortOrder = sortOrder;

            return PartialView("SortProducts", result);
        }

        [Authorize]
        [HttpPost]
        public IActionResult PostComment(int MaHh, string BinhLuan, int Rating)
        {
            try
            {
                string maKh = User.FindFirst(MySetting.CLAIM_CUSTOMERID)?.Value;

                if (maKh != null)
                {
                    var newComment = new DanhGia
                    {
                        MaKh = maKh,
                        MaHh = MaHh,
                        BinhLuan = BinhLuan,
                        Rating = Rating,
                        Date = DateTime.Now
                    };
                    db.DanhGia.Add(newComment);
                    db.SaveChanges();
                }
                else
                {
                    return Unauthorized(); // Return an error if not logged in
                }

                return RedirectToAction("Detail", new { id = MaHh });
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
