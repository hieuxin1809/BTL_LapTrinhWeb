using BTL_LapTrinhWeb.Data;
using BTL_LapTrinhWeb.Helpers;
using BTL_LapTrinhWeb.ViewComponents;
using BTL_LapTrinhWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BTL_LapTrinhWeb.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly Hshop2023Context db;
        public CartViewComponent(Hshop2023Context context)
        {
            db = context;
        }
        public IViewComponentResult Invoke()
        {
            var cart =  HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();
            return View("CartPanel",
                new CartModel
                {
                    quantity = cart.Count(),
                    Total = cart.Sum(p => p.ThanhTien)
                }
                );
        }

    }
}
