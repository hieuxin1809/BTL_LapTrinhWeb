using AutoMapper;
using BTL_LapTrinhWeb.Data;
using BTL_LapTrinhWeb.ViewModels;

namespace BTL_LapTrinhWeb.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterVM, KhachHang>();
                //.ForMember(kh => kh.HoTen, option => option.MapFrom(RegisterVM => RegisterVM.HoTen))
                //.ReverseMap();
        }
    }
}
