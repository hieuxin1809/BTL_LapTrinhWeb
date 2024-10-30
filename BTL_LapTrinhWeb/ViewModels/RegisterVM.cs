using System.ComponentModel.DataAnnotations;

namespace BTL_LapTrinhWeb.ViewModels
{
	public class RegisterVM
	{
		[Key]
		[Display(Name = "Tên đăng nhập")]
		[Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
		[MaxLength(20, ErrorMessage = "Tối đa 20 ký tự.")]
		public string MaKh { get; set; }

		[Display(Name = "Mật khẩu")]
		[Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
		[DataType(DataType.Password)]
		[StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
			ErrorMessage = "Mật khẩu phải chứa ít nhất 6 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.")]
		public string MatKhau { get; set; }

		[Display(Name = "Xác nhận mật khẩu")]
		[Required(ErrorMessage = "Vui lòng xác nhận mật khẩu.")]
		[DataType(DataType.Password)]
		[Compare("MatKhau", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
		public string ConfirmPassword { get; set; }

		[Display(Name = "Email")]
		[Required(ErrorMessage = "Vui lòng nhập email.")]
		[EmailAddress(ErrorMessage = "Chưa đúng định dạng email.")]
		public string Email { get; set; }

		[Display(Name = "Họ tên")]
		[Required(ErrorMessage = "Vui lòng nhập họ tên.")]
		[MaxLength(50)]
		public string HoTen { get; set; }
	}
}
