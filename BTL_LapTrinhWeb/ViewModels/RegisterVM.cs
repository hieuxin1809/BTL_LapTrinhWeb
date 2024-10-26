using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace BTL_LapTrinhWeb.ViewModels
{
    public class RegisterVM
    {
        //[Key]
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage ="*")]
        [MaxLength(20, ErrorMessage = "Tối đa 20 kí tự")]
        public string MaKh { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [EmailAddress(ErrorMessage ="Chưa đúng định dạng email")]
        public string Email { get; set; }

        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "*")]
        [MaxLength(50)]
        public string HoTen { get; set; }
    }
}
