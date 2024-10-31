using System.ComponentModel.DataAnnotations;

namespace BTL_LapTrinhWeb.ViewModels
{
    public class HoaDonAnnotation
    {       
        public int MaHd { get; set; }

        [Required(ErrorMessage = "Mã khách hàng là bắt buộc")]
        [Display(Name = "Mã khách hàng")]
        public string? MaKh { get; set; } = null!;

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
        [Display(Name = "Họ tên khách hàng")]
        public string? HoTen { get; set; } = null!;

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string? DiaChi { get; set; } = null!;

        [Required(ErrorMessage = "Cách thanh toán là bắt buộc")]
        [Display(Name = "Phương thức thanh toán")]
        public string? CachThanhToan { get; set; } = null!;

        [Required(ErrorMessage = "Cách vận chuyển là bắt buộc")]
        [Display(Name = "Phương thức vận chuyển")]
        public string? CachVanChuyen { get; set; } = null!;
        [Required(ErrorMessage = "Phí Vận Chuyển  là bắt buộc")]

        [Range(0, double.MaxValue, ErrorMessage = "Phí vận chuyển phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Phí vận chuyển")]
        public double PhiVanChuyen { get; set; }
        [Required(ErrorMessage = "Mã Trạng Thái là bắt buộc")]

        [Display(Name = "Mã Trạng Thái")]
        public int MaTrangThai { get; set; }
        [Required(ErrorMessage = "Mã Nhân Viên là bắt buộc")]

        [Display(Name = "Mã nhân viên phụ trách")]
        public string? MaNv { get; set; }
        [Required(ErrorMessage = "Ghi Chú là bắt buộc")]

        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự")]
        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        [Required(ErrorMessage = "Ngày đặt là bắt buộc")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày đặt hàng")]
        public DateTime NgayDat { get; set; }
        [Required(ErrorMessage = "Ngày cần là bắt buộc")]

        [DataType(DataType.Date)]
        [Display(Name = "Ngày cần giao")]
        public DateTime? NgayCan { get; set; }
        [Required(ErrorMessage = "Ngày Giao là bắt buộc")]

        [DataType(DataType.Date)]
        [Display(Name = "Ngày giao hàng")]
        public DateTime? NgayGiao { get; set; }        
    }
}
