﻿using System.ComponentModel.DataAnnotations;

namespace BTL_LapTrinhWeb.ViewModels
{
	public class LoginVM
	{
		[Display(Name = "Tên đăng nhập")]
		[Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
		[MaxLength(20, ErrorMessage = "Tối đa 20 ký tự.")]
		public string UserName { get; set; }

		[Display(Name = "Mật khẩu")]
		[Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
