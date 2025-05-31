using AutoMapper;
using Finance.Repository.Abstraction.Entities;
using Finance.Shared.Models.MstApp;
using Finance.Shared.Models.MstType;
using Finance.Shared.Models.MstUser;

namespace Finance.Api.Mapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            #region MstType
            CreateMap<MstType, MstTypeResponseModel>().ReverseMap();

            CreateMap<MstType, MstTypeCreateModel>();
            CreateMap<MstTypeCreateModel, MstType>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Enable, opt => opt.MapFrom(src => true));

            CreateMap<MstType, MstTypeUpdateModel>();
            CreateMap<MstTypeUpdateModel, MstType>()
            .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => DateTime.Now));
            #endregion

            #region MstApp
            CreateMap<MstApp, MstAppResponseModel>().ReverseMap();
            CreateMap<MstApp, MstAppCreateModel>();
            CreateMap<MstAppCreateModel, MstApp>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Enable, opt => opt.MapFrom(src => true));

            CreateMap<MstApp, MstAppUpdateModel>();
            CreateMap<MstAppUpdateModel, MstApp>()
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => DateTime.Now));
            #endregion

            #region MstUSer
            CreateMap<MstUser, MstUserResponseModel>().ReverseMap();
            CreateMap<MstUserCreateModel, MstUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Enable, opt => opt.MapFrom(src => true));

            CreateMap<MstUserUpdateInforModel, MstUser>()
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => DateTime.Now));
            #endregion
        }
    }
}
