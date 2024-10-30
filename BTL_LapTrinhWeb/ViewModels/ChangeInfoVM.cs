using System.ComponentModel.DataAnnotations;

namespace BTL_LapTrinhWeb.ViewModels
{
	public class ChangeInfoVM
	{
		[Display(Name = "Họ tên")]
		[Required(ErrorMessage = "Vui lòng nhập họ tên.")]
		[MaxLength(50, ErrorMessage = "Tối đa 50 ký tự.")]
		public string HoTen { get; set; }

		public bool GioiTinh { get; set; } = true;

		[Display(Name = "Ngày sinh")]
		[DataType(DataType.Date)]
		public DateTime? NgaySinh { get; set; }

		[MaxLength(60, ErrorMessage = "Tối đa 60 ký tự.")]
		public string? DiaChi { get; set; }

		[MaxLength(24, ErrorMessage = "Tối đa 24 ký tự.")]
		[RegularExpression(@"^(0[3|5|7|8|9][0-9]{8})$", ErrorMessage = "Số điện thoại không đúng định dạng.")]
		public string? DienThoai { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập địa chỉ email.")]
		[EmailAddress(ErrorMessage = "Chưa đúng định dạng email.")]
		public string Email { get; set; }

		[DataType(DataType.Upload)]
		public string? Hinh { get; set; }
	}
}
