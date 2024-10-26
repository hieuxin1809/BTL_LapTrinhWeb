using System.ComponentModel.DataAnnotations;

namespace BTL_LapTrinhWeb.ViewModels
{
    public class ContactVM
    {
        [Required(ErrorMessage ="Vui lòng nhập tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Vui lòng nhập đúng định dạng Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung tin nhắn")]
        public string Message { get; set; }
    }
}
