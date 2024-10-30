using System.ComponentModel.DataAnnotations;

namespace BTL_LapTrinhWeb.ViewModels
{
	public class ForgotPasswordVM
	{
		[Display(Name = "Tên người dùng")]
		[Required(ErrorMessage = "Vui lòng nhập tên người dùng.")]
		public string Username { get; set; }

		[Display(Name = "Email")]
		[Required(ErrorMessage = "Vui lòng nhập email.")]
		[DataType(DataType.EmailAddress)]
		[EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
		public string Email { get; set; }

		[Display(Name = "Mật khẩu mới")]
		[Required(ErrorMessage = "Vui lòng nhập mật khẩu mới.")]
		[StringLength(100, ErrorMessage = "Mật khẩu mới phải có ít nhất {2} ký tự.", MinimumLength = 6)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
			ErrorMessage = "Mật khẩu phải chứa ít nhất 6 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }
	}
}
