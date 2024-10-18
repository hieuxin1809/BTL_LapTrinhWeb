namespace BTL_LapTrinhWeb.ViewModels
{
    public class CartItem
    {
        public int maHH {  get; set; }
        public string Hinh { get; set; }
        public string TenHH { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien => DonGia * SoLuong;

    }
}
