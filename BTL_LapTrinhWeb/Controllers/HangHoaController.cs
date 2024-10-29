using BTL_LapTrinhWeb.Data;
using BTL_LapTrinhWeb.Helpers;
using BTL_LapTrinhWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing.Printing;
using X.PagedList;
using X.PagedList.Extensions;

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
<<<<<<< Updated upstream
=======
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
>>>>>>> Stashed changes

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

        [HttpPost]
        public IActionResult PostComment(int MaHh, string BinhLuan, int Rating)
        {
            string maKh = User.FindFirst(MySetting.CLAIM_CUSTOMERID)?.Value;

            // Kiểm tra nếu maKh không null
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


            // Sau khi lưu, quay lại trang chi tiết sản phẩm
            return RedirectToAction("Detail", new { id = MaHh });
        }
    }
}
