using BTL_LapTrinhWeb.Data;
using BTL_LapTrinhWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BTL_LapTrinhWeb.ViewComponents
{
    public class BestSalerViewComponent : ViewComponent
    {
        private readonly Hshop2023Context db;
        public BestSalerViewComponent(Hshop2023Context context) => db = context;
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await db.HangHoas.Select(lo => new BestSalerVM
            {
                Mahh = lo.MaHh,
                Tenhh = lo.TenHh,
                TenLoai = lo.MaLoaiNavigation.TenLoai,
                Hinh = lo.Hinh,
                DonGia = lo.DonGia,
                MoTaNgan = lo.MoTaDonVi,
                SoLanXem = lo.SoLanXem
            }).OrderByDescending(x => x.SoLanXem).Take(10).ToListAsync();
            return View(data);
        }
    }
}