using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BTL_LapTrinhWeb.ViewModels
{
	public class ChangePasswordVM
	{
		[Display(Name = "Mật khẩu cũ")]
		[Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
		[DataType(DataType.Password)]
		public string OldPassword { get; set; }

		[Display(Name = "Mật khẩu mới")]
		[Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
		[StringLength(100, ErrorMessage = "Mật khẩu mới phải có ít nhất {2} ký tự.", MinimumLength = 6)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
			ErrorMessage = "Mật khẩu phải chứa ít nhất 6 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Display(Name = "Nhập lại mật khẩu mới")]
		[Required(ErrorMessage = "Vui lòng nhập lại mật khẩu mới")]
		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "Mật khẩu nhập lại không khớp.")]
		public string ConfirmPassword { get; set; }
	}
}
