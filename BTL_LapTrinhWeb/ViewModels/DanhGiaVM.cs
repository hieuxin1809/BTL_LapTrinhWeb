namespace BTL_LapTrinhWeb.ViewModels
{
    public class DanhGiaVM
    {
        public int MaHh { get; set; }

        public string MaKh { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string BinhLuan { get; set; } = null!;

        public int? Rating { get; set; }

        public DateTime? Date { get; set; }
    }
}
