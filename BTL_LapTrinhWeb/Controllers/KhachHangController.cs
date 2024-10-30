using AutoMapper;
using BTL_LapTrinhWeb.Data;
using BTL_LapTrinhWeb.Helpers;
using BTL_LapTrinhWeb.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace BTL_LapTrinhWeb.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly Hshop2023Context db;
        private readonly IMapper _mapper;

        public KhachHangController(Hshop2023Context context, IMapper mapper)
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
                    khachHang.Hinh = "avatar.jpg";
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
            if (ModelState.IsValid)
            {
                var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == model.UserName);
                if (khachHang == null)
                {
                    ModelState.AddModelError("Loi", "Khong ton tai ten dang nhap nay");
                }
                else
                {
                    if (!khachHang.HieuLuc)
                    {
                        ModelState.AddModelError("Loi", "Tai khoan da bi khoa, vui long lien he admin");
                    }
                    else
                    {
                        if (khachHang.MatKhau != model.Password.ToMd5Hash(khachHang.RandomKey))
                        {
                            ModelState.AddModelError("Loi", "Thong tin dang nhap khong dung");
                        }
                        else
                        {
                            // Xác định vai trò người dùng
                            var role = khachHang.VaiTro == 0 ? "Customer" : "Admin";

                            // Tạo claims
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email, khachHang.Email),
                                new Claim(ClaimTypes.Name, khachHang.HoTen),
                                new Claim(MySetting.CLAIM_CUSTOMERID, khachHang.MaKh),
                                new Claim(ClaimTypes.Role, role)
                            };
                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPrincipal);

                            if (role == "Customer")
                            {
                                if (Url.IsLocalUrl(ReturnURL))
                                {
                                    return Redirect(ReturnURL);
                                }
                                return Redirect("/");
                            }
                            else
                            {
                                return RedirectToAction("Index", "HomeAdmin", new { area = "admin" });
                            }
                        }
                    }
                }
            }
            return View();
        }
        #endregion

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == model.Username);

                if (khachHang == null)
                {
                    //TempData["Message"] = $"Không tồn tại người dùng {model.Username}";
                    ModelState.AddModelError("Username", "Tên đăng nhập không tồn tại.");
                    return View(model);
                }

                if (khachHang.Email != model.Email)
                {
                    ModelState.AddModelError("Email", "Email không khớp với tên đăng nhập này.");
                    return View(model);
                }

				khachHang.RandomKey = MyUtil.GenerateRamdomKey();
				khachHang.MatKhau = model.NewPassword.ToMd5Hash(khachHang.RandomKey);
                db.SaveChanges();
                
                return RedirectToAction("DangNhap", "KhachHang");
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
