using AutoMapper;
using BTL_LapTrinhWeb.Data;
using BTL_LapTrinhWeb.Helpers;
using BTL_LapTrinhWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;

namespace BTL_LapTrinhWeb.Controllers
{
    public class ProfileController : Controller
    {
        private readonly Hshop2023Context db;
        private readonly IMapper _mapper;
		private string? userName => User.FindFirst(MySetting.CLAIM_CUSTOMERID)?.Value;

		public ProfileController (Hshop2023Context context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        #region Hiển thị thông tin
        [Authorize]
        public IActionResult Profile()
        {
            var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == userName);
            if (khachHang == null)
            {
                TempData["Message"] = "khong tim thay ";
                return RedirectToAction("PageNotFound", "Home");
            }
            var result = new ProfileVM
            {
                HoTen = khachHang.HoTen,
                GioiTinh = khachHang.GioiTinh,
                NgaySinh = khachHang.NgaySinh,
                DiaChi = khachHang.DiaChi,
                DienThoai = khachHang.DienThoai,
                Email = khachHang.Email,
                Hinh = khachHang.Hinh,
            };
            return View(result);
        }
        #endregion

        #region Đổi mật khẩu
            [Authorize]
            [HttpGet]
            public IActionResult ChangePassword()
            {
                return View();
            }
            [HttpPost]
            public IActionResult ChangePassword(ChangePasswordVM model)
            {
			    if (ModelState.IsValid)
			    {
                var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == userName);

                if (khachHang == null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập không tồn tại.");
                    return View(model);
                }

                if (khachHang.MatKhau != model.OldPassword.ToMd5Hash(khachHang.RandomKey))
                {
                    ModelState.AddModelError("OldPassword", "Mật khẩu cũ không đúng.");
                    return View(model);
                }

                if (model.NewPassword != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Mật khẩu mới và mật khẩu nhập lại không khớp.");
                    return View(model);
                }

                khachHang.RandomKey = MyUtil.GenerateRamdomKey();
                khachHang.MatKhau = model.NewPassword.ToMd5Hash(khachHang.RandomKey);
                db.SaveChanges();

				TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";

				return RedirectToAction("ChangePassword");
			}

			    return View(model);
		    }
        #endregion

        #region Danh sách dơn hàng
        [Authorize]
        public IActionResult ListOrder()
        {
            var donHang = db.HoaDons.Include(p => p.MaTrangThaiNavigation).Where(dh => dh.MaKh == userName).ToList();
            if (donHang == null)
            {
                TempData["Message"] = "khong tim thay ";
                return RedirectToAction("PageNotFound", "Home");
            }
            var result = donHang.Select(dh => new HoaDonVM
            {
                MaHd = dh.MaHd,
                NgayDat = dh.NgayDat,
                DiaChi = dh.DiaChi,
                CachThanhToan = dh.CachThanhToan,
                CachVanChuyen = dh.CachVanChuyen,
                GhiChu = dh.GhiChu,
                TenTrangThai = dh.MaTrangThaiNavigation.TenTrangThai,
            }).ToList();
            return View(result);
        }
        #endregion

        #region Chỉnh sửa thông tin
        [Authorize]
        [HttpGet]
        public IActionResult ChangeInfo()
        {
            var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == userName);

            if(khachHang == null)
            {
                TempData["Message"] = "khong tim thay ";
                return RedirectToAction("PageNotFound", "Home");
            }

            var data = new ChangeInfoVM
            {
                HoTen = khachHang.HoTen,
                GioiTinh = khachHang.GioiTinh,
                NgaySinh = khachHang.NgaySinh,
                DiaChi = khachHang.DiaChi,
                DienThoai = khachHang.DienThoai,
                Email = khachHang.Email,
                Hinh = string.IsNullOrEmpty(khachHang.Hinh) ? "avatar.jpg" : khachHang.Hinh,
            };

            return View(data);
        }
        [HttpPost]
        public IActionResult ChangeInfo(ChangeInfoVM model, IFormFile? Hinh)
        {
            if (ModelState.IsValid)
            {
                var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == userName);

                if (khachHang == null)
                {
                    TempData["Message"] = "khong tim thay ";
                    return RedirectToAction("PageNotFound", "Home");
                }

                khachHang.HoTen = model.HoTen;
                khachHang.GioiTinh = model.GioiTinh;
                khachHang.NgaySinh = model.NgaySinh.Value;
                khachHang.DiaChi = model.DiaChi;
                khachHang.DienThoai = model.DienThoai;
                khachHang.Email = model.Email;

                if (Hinh != null)
                {
                    khachHang.Hinh = MyUtil.UploadHinh(Hinh, "KhachHang");
                }

                db.SaveChanges();
                return RedirectToAction("Profile");
            } else
            {
				var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
				ViewData["ErrorMessage"] = errors;
			}
            return View(model);
        }
        #endregion
    }
}
