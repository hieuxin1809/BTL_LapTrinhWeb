﻿using BTL_LapTrinhWeb.Data;
using BTL_LapTrinhWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_LapTrinhWeb.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private readonly Hshop2023Context db;

        public MenuLoaiViewComponent(Hshop2023Context context)=> db = context;
        public async Task<IViewComponentResult> InvokeAsync(bool isDefaultView)
        {
            var data = await db.Loais
                .Select(lo => new MenuLoaiVM
                {
                    MaLoai = lo.MaLoai,
                    TenLoai = lo.TenLoai,
                    Hinh = lo.Hinh,
                    SoLuong = lo.HangHoas.Count()
                })
                .OrderBy(x => x.TenLoai)
                .ToListAsync();

            if(isDefaultView)
            {
                return View("Default",data);
            }
            return View("BannerLoai",data);
        }
    }
}