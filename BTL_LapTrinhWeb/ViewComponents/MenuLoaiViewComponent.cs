using BTL_LapTrinhWeb.Data;
using BTL_LapTrinhWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BTL_LapTrinhWeb.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private readonly Hshop2023Context db;

        public MenuLoaiViewComponent(Hshop2023Context context)=> db = context;
        public IViewComponentResult Invoke()
        {
            var data = db.Loais.Select(lo => new MenuLoaiVM { 
                MaLoai= lo.MaLoai , TenLoai = lo.TenLoai , SoLuong = lo.HangHoas.Count()
            }).OrderBy(x=> x.TenLoai);
            return View(data);
        }
    }
}
