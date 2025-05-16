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

            CreateMap<MstType, MstTypeCreateModel>().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<MstTypeCreateModel, MstType>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

            CreateMap<MstType, MstTypeUpdateModel>().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<MstTypeUpdateModel, MstType>();
        }
    }
}
