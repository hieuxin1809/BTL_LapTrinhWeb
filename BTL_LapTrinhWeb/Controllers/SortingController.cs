using BTL_LapTrinhWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BTL_LapTrinhWeb.Controllers
{
    public class SortingController : Controller
    {
        private readonly Hshop2023Context db;
        public SortingController(Hshop2023Context _db)
        {
            db = _db;
        }
        public IActionResult Index(string sortOrder)
        {
            var products = db.HangHoas.AsQueryable();
            switch (sortOrder)
            {
                case "price_asc": // Giá từ thấp đến cao
                    products = products.OrderBy(p => p.DonGia);
                    break;
                case "price_desc": // Giá từ cao đến thấp
                    products = products.OrderByDescending(p => p.DonGia);
                    break;
                default:
                    // Nếu không có sắp xếp, giữ nguyên thứ tự
                    break;
            }
            var productList = products.ToList(); // Chuyển đổi về danh sách

            return View(productList);
        }
    }
}
