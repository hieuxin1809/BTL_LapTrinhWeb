﻿namespace BTL_LapTrinhWeb.ViewModels
{
    public class HangHoaVM
    {
        public int Mahh { get; set; }
        public string Tenhh { get; set; }
        public string Hinh { get; set; }
        public double DonGia { get; set; }
        public string MoTaNgan { get; set; }
        public string TenLoai { get; set; }
    }
    public class ChiTietHangHoaVM
    {
        public int Mahh { get; set; }
        public string Tenhh { get; set; }
        public string Hinh { get; set; }
        public double DonGia { get; set; }
        public string MoTaNgan { get; set; }
        public string TenLoai { get; set; }
        public string ChiTiet { get; set; }
        public int DiemDanhGia { get; set; }
        public int SoLuongTon { get; set; }
    }

}
