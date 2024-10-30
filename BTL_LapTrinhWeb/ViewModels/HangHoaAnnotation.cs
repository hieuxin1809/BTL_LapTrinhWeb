using System.ComponentModel.DataAnnotations;

namespace BTL_LapTrinhWeb.ViewModels
{
    public class HangHoaAnnotation
    {
        [Display(Name = "Mã hàng hóa")]
        public int MaHh { get; set; }

        [Required(ErrorMessage = "Tên hàng hóa là bắt buộc.")]
        [Display(Name = "Tên hàng hóa")]
        public string TenHh { get; set; } = null!;

        [StringLength(100, ErrorMessage = "Tên alias không được dài quá 100 ký tự.")]
        [Display(Name = "Tên bí danh")]
        public string? TenAlias { get; set; }

        [Required(ErrorMessage = "Mã loại là bắt buộc.")]
        [Display(Name = "Mã loại")]
        public int? MaLoai { get; set; }

        [Required(ErrorMessage = "Mô tả đơn vị là bắt buộc.")]
        [Display(Name = "Mô tả đơn vị")]
        public string MoTaDonVi { get; set; } = null!;

        [Required(ErrorMessage = "Đơn giá là bắt buộc.")]
        [Range(0, 100000, ErrorMessage = "Đơn giá phải nằm trong khoảng từ 0 đến 100000.")]
        [Display(Name = "Đơn giá")]
        public double? DonGia { get; set; }

        [Display(Name = "Hình")]
        [DataType(DataType.Upload)]
        public string? Hinh { get; set; }

        [Range(typeof(DateTime), "1/1/1963", "10/21/2024")]
        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Ngày sản xuất")]
        public DateTime? NgaySx { get; set; }

        [Required(ErrorMessage = "Giảm Giá là bắt buộc.")]
        [Display(Name = "Giảm giá")]
        [Range(0, 100, ErrorMessage = "Giảm giá phải nằm trong khoảng từ 0 đến 100.")]
        public double? GiamGia { get; set; }
        [Required(ErrorMessage = "Số Lần Xem là bắt buộc.")]
        [Display(Name = "Số lần xem")]
        public int? SoLanXem { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được dài quá 500 ký tự.")]
        [Display(Name = "Mô tả sản phẩm")]
        public string? MoTa { get; set; }

        [Required(ErrorMessage = "Mã nhà cung cấp là bắt buộc.")]
        [Display(Name = "Mã nhà cung cấp")]
        public string MaNcc { get; set; } = null!;
    }
}
