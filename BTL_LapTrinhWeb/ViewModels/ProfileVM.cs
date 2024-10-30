using System.ComponentModel.DataAnnotations;

namespace BTL_LapTrinhWeb.ViewModels
{
    public class ProfileVM
    {
        [Key]
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Vui lòng nhập tên người dùng")]
        [MaxLength(20, ErrorMessage = "Tối đa 20 kí tự")]
        public string MaKh { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string HoTen { get; set; }

        public bool GioiTinh { get; set; } = true;

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [MaxLength(60, ErrorMessage = "Tối đa 60 kí tự")]
        public string? DiaChi { get; set; }

        [MaxLength(24, ErrorMessage = "Tối đa 24 kí tự")]
        //[RegularExpression("0[9875]/d{8}", ErrorMessage ="Chứ đúng định dạng di động Việt Nam")]
        public string? DienThoai { get; set; }

        [EmailAddress(ErrorMessage = "Chưa đúng định dạng email")]
        public string Email { get; set; }

        [DataType(DataType.Upload)]
        public string? Hinh { get; set; }
    }
}
