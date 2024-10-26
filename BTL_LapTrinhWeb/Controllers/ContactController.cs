using BTL_LapTrinhWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace BTL_LapTrinhWeb.Controllers
{
    public class ContactController : Controller
    {
        private readonly EmailService _emailService;

        public ContactController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactVM model)
        {
            if (ModelState.IsValid)
            {
                // Gửi email
                string subject = "Thư liên hệ từ khách hàng " + model.Name;
                string message = $"Nội dung: {model.Message}";

                await _emailService.SendEmailAsync(model.Email, subject, message);

                // Thông báo thành công
                ViewBag.Message = "Gửi tin nhắn thành công!";
                return View();
            }

            return View(model);
        }
    }

}
