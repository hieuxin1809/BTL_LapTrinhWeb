using BTL_LapTrinhWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BTL_LapTrinhWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("/404")]
        public IActionResult PageNotFound()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //public IActionResult AccessDenied()
        //{
        //    TempData["ErrorMessage"] = "Bạn không có quyền truy cập vào trang này.";
        //    return Redirect(Request.Headers["Referer"].ToString()); // Trả về trang trước đó
        //}

    }
}
