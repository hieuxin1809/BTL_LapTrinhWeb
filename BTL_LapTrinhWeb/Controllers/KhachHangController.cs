using AutoMapper;
using BTL_LapTrinhWeb.Data;
using BTL_LapTrinhWeb.Helpers;
using BTL_LapTrinhWeb.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Security.Claims;

namespace BTL_LapTrinhWeb.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly Hshop2023Context db;
        private readonly IMapper _mapper;

        public KhachHangController(Hshop2023Context context,IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        #region Register
        [HttpGet]
		public IActionResult DangKy()
		{
			return View();
		}

		[HttpPost]
		public IActionResult DangKy(RegisterVM model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var khachHang = _mapper.Map<KhachHang>(model);
					khachHang.RandomKey = MyUtil.GenerateRamdomKey();
					khachHang.MatKhau = model.MatKhau.ToMd5Hash(khachHang.RandomKey);
					khachHang.HieuLuc = true;//sẽ xử lý khi dùng Mail để active
					khachHang.VaiTro = 0;


					db.Add(khachHang);
					db.SaveChanges();
					return RedirectToAction("Index", "HangHoa");
				}
				catch (Exception ex)
				{
					var mess = $"{ex.Message} shh";
				}
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
            }
            return View();
		}
		#endregion

		#region Loggin in
		[HttpGet]
        public IActionResult DangNhap(string? ReturnURL)
        {
            ViewBag.ReturnURL = ReturnURL;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginVM model, string? ReturnURL)
        {
            ViewBag.ReturnURL = ReturnURL;
            if(ModelState.IsValid)
            {
                var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == model.UserName);
                if(khachHang == null)
                {
                    ModelState.AddModelError("Loi", "Khong ton tai ten dang nhap nay");
                }
                else
                {
                    if(!khachHang.HieuLuc)
                    {
                        ModelState.AddModelError("Loi", "Tai khoan da bi khoa, vui long lien he admin");
                    }
                    else
                    {
                        if(khachHang.MatKhau != model.Password.ToMd5Hash(khachHang.RandomKey))
                        {
                            ModelState.AddModelError("Loi", "Thong tin dang nhap khong dung");
                        }
                        else
                        {
                            //ghi nhan
                            var claims = new List<Claim> {
                                new Claim(ClaimTypes.Email, khachHang.Email),
                                new Claim(ClaimTypes.Name, khachHang.HoTen),
                                new Claim(MySetting.CLAIM_CUSTOMERID, khachHang.MaKh),

                                ///claim động
                                new Claim(ClaimTypes.Role, "Customer"),
                            }; 
                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPrincipal);

                            if(Url.IsLocalUrl(ReturnURL))
                            {
                                return Redirect(ReturnURL);
                            } else
                            {
                                return Redirect("/");
                            }
                        }
                    }
                }
            }
            return View();
        }
        #endregion

        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Profile(ProfileVM model, IFormFile Hinh)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var khachHang = _mapper.Map<KhachHang>(model);
                    if (Hinh != null)
                    {
                        khachHang.Hinh = MyUtil.UploadHinh(Hinh, "KhachHang");
                    }

                    db.Add(khachHang);
                    db.SaveChanges();
                    return RedirectToAction("Profile", "KhachHang");
                }
                catch (Exception ex) 
                {
                    var message = $"Lỗi: {ex.Message}";
                }
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
