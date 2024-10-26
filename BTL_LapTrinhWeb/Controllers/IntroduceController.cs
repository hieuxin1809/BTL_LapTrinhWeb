using Microsoft.AspNetCore.Mvc;

namespace BTL_LapTrinhWeb.Controllers
{
    public class IntroduceController : Controller
    {
        public IActionResult Introduce()
        {
            return View();
        }
    }
}
