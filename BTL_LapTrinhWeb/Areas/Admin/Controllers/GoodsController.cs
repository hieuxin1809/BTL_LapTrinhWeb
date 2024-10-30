using BTL_LapTrinhWeb.Data;
using BTL_LapTrinhWeb.Helpers;
using BTL_LapTrinhWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BTL_LapTrinhWeb.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    [Authorize(Roles = "Admin")]
    public class GoodsController : Controller
    {
        Hshop2023Context db = new Hshop2023Context();

        // Tiêm DbContext thông qua constructor
        public GoodsController(Hshop2023Context _db)
        {
            db = _db;
        }
        #region Danh mục sản phẩm

        [Route("danhmucsanpham")]
        public IActionResult DanhMucSanPham(int page = 1, int pageSize = 12)
        {
            // Tính tổng số sản phẩm trước khi phân trang
            int totalItems = db.HangHoas.Count();

            // Lấy danh sách sản phẩm và sắp xếp theo thứ tự giảm dần
            var lst = db.HangHoas.OrderByDescending(h => h.MaHh) // hoặc bất kỳ thuộc tính nào bạn muốn
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();

            // Tính tổng số trang dựa trên tổng số sản phẩm
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Truyền thông tin phân trang cho View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            return View(lst);
        }
        #endregion

        #region Them san pham moi
        [Route("ThemSanPhamMoi")]
        [HttpGet]
        public IActionResult ThemSanPhamMoi()
        {
            ViewBag.MaLoai = new SelectList(db.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.MaNcc = new SelectList(db.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");
            return View();
        }
        [Route("ThemSanPhamMoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemSanPhamMoi(HangHoaAnnotation hangHoaVM, IFormFile? Hinh)
        {
            TempData["Message"] = "";
            if (ModelState.IsValid)
            {
                // Chuyển đổi HangHoaVM sang HangHoa
                var hangHoa = new HangHoa
                {
                    TenHh = hangHoaVM.TenHh,
                    TenAlias = hangHoaVM.TenAlias,
                    MaLoai = (int)hangHoaVM.MaLoai,
                    MoTaDonVi = hangHoaVM.MoTaDonVi,
                    DonGia = hangHoaVM.DonGia,
                    NgaySx = (DateTime)hangHoaVM.NgaySx,
                    GiamGia = (double)hangHoaVM.GiamGia,
                    SoLanXem = (int)hangHoaVM.SoLanXem,
                    MoTa = hangHoaVM.MoTa,
                    MaNcc = hangHoaVM.MaNcc
                };
                if (Hinh != null)
                {
                    hangHoa.Hinh = MyUtil.UploadHinh(Hinh, "KhachHang");
                }

                db.HangHoas.Add(hangHoa);
                db.SaveChanges();
                TempData["Message"] = "Đã thêm thành công hàng hóa";
                return RedirectToAction("DanhMucSanPham");
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

            // Nếu không hợp lệ, truyền lại danh sách loại và nhà cung cấp
            ViewBag.MaLoai = new SelectList(db.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.MaNcc = new SelectList(db.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");
            return View(hangHoaVM); // Trả về model đã nhập
        }
        #endregion

        #region SUa sam pham
        [Route("SuaSanPham")]
        [HttpGet]
        public IActionResult SuaSanPham(int Mahh)
        {
            ViewBag.MaLoai = new SelectList(db.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.MaNcc = new SelectList(db.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");

            var sanPham = db.HangHoas.Find(Mahh);


            var hangHoaVM = new HangHoaAnnotation
            {
                TenHh = sanPham.TenHh,
                TenAlias = sanPham.TenAlias,
                MaLoai = sanPham.MaLoai,
                MoTaDonVi = sanPham.MoTaDonVi,
                DonGia = sanPham.DonGia,
                Hinh = sanPham.Hinh,
                NgaySx = sanPham.NgaySx,
                GiamGia = sanPham.GiamGia,
                SoLanXem = sanPham.SoLanXem,
                MoTa = sanPham.MoTa,
                MaNcc = sanPham.MaNcc
            };

            return View(hangHoaVM); // Trả về HangHoaAnnotation
        }

        [Route("SuaSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaSanPham(HangHoaAnnotation hangHoaVM, IFormFile? Hinh)
        {
            TempData["Message"] = "";
            if (ModelState.IsValid)
            {
                // Tìm sản phẩm theo MaHh
                var hangHoa = db.HangHoas.Find(hangHoaVM.MaHh);
                if (hangHoa == null)
                {
                    return NotFound(); // Trả về lỗi 404 nếu không tìm thấy sản phẩm
                }

                // Cập nhật các thuộc tính
                hangHoa.TenHh = hangHoaVM.TenHh;
                hangHoa.TenAlias = hangHoaVM.TenAlias;
                hangHoa.MaLoai = (int)hangHoaVM.MaLoai;
                hangHoa.MoTaDonVi = hangHoaVM.MoTaDonVi;
                hangHoa.DonGia = hangHoaVM.DonGia;
                if (Hinh != null)
                {
                    hangHoa.Hinh = MyUtil.UploadHinh(Hinh, "KhachHang");
                } else
                {
                    hangHoa.Hinh = hangHoaVM.Hinh;
                }
                hangHoa.NgaySx = (DateTime)hangHoaVM.NgaySx;
                hangHoa.GiamGia = (double)hangHoaVM.GiamGia;
                hangHoa.SoLanXem = (int)hangHoaVM.SoLanXem;
                hangHoa.MoTa = hangHoaVM.MoTa;
                hangHoa.MaNcc = hangHoaVM.MaNcc;

                // Cập nhật cơ sở dữ liệu
                db.Update(hangHoa);
                db.SaveChanges();
                TempData["Message"] = "Đã sửa thành công đơn hàng";
                return RedirectToAction("DanhMucSanPham", "Goods");
            }

            // Nếu không hợp lệ, giữ lại dữ liệu trong model và trả về view
            ViewBag.MaLoai = new SelectList(db.Loais.ToList(), "MaLoai", "TenLoai", hangHoaVM.MaLoai);
            ViewBag.MaNcc = new SelectList(db.NhaCungCaps.ToList(), "MaNcc", "TenCongTy", hangHoaVM.MaNcc);
            return View(hangHoaVM); // Trả về lại view với dữ liệu không hợp lệ
        }
        #endregion Sua san pham

        #region Xoa san pham
        [Route("XoaSanPham")]
        [HttpGet]
        public IActionResult XoaSanPham(int Mahh)
        {
            // Khởi tạo thông báo
            TempData["Message"] = "";

            // Kiểm tra xem sản phẩm có liên quan đến hóa đơn nào không
            var chiTietSanPham = db.VChiTietHoaDons.Where(x => x.MaHh == Mahh).ToList();
            if (chiTietSanPham.Count() > 0)
            {
                TempData["Message"] = "Không thể xóa sản phẩm này vì nó đã được đặt hàng.";
                return RedirectToAction("DanhMucSanPham", "Goods");
            }

            // Tìm sản phẩm theo mã
            var sanPham = db.HangHoas.Find(Mahh);
            if (sanPham != null)
            {
                db.HangHoas.Remove(sanPham); // Xóa sản phẩm
                db.SaveChanges(); // Lưu thay đổi
                TempData["Message"] = "Sản phẩm đã được xóa thành công.";
            }
            else
            {
                TempData["Message"] = "Không tìm thấy sản phẩm.";
            }

            return RedirectToAction("DanhMucSanPham", "Goods");
        }
        #endregion

        [Route("ChiTietSP")]
        [HttpGet]
        public IActionResult ChiTietSP(int Mahh)
        {
            var hangHoa = db.HangHoas.Find(Mahh);

            var data = new HangHoaAnnotation
            {
                MaHh = hangHoa.MaHh,
                TenHh = hangHoa.TenHh,
                TenAlias = hangHoa.TenAlias,
                MoTaDonVi = hangHoa.MoTaDonVi,
                MoTa = hangHoa.MoTa,
                DonGia = hangHoa.DonGia,
                Hinh = hangHoa.Hinh,
                NgaySx = hangHoa.NgaySx,
                GiamGia = hangHoa.GiamGia,
                SoLanXem = hangHoa.SoLanXem
            };
            return View(data);
        }
    }
}
