using AutoMapper;
using Finance.Repository.Abstraction.Entities;
using Finance.Shared.Models.MstType;

namespace Finance.Api.Mapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<MstType, MstTypeResponseModel>().ReverseMap();

            CreateMap<MstType, MstTypeCreateModel>();
            CreateMap<MstTypeCreateModel, MstType>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Enable, opt => opt.MapFrom(src => true));

            CreateMap<MstType, MstTypeUpdateModel>();
            CreateMap<MstTypeUpdateModel, MstType>()
            .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<MstType, MstTypeResponseModel>().ReverseMap();
        }
    }
}
