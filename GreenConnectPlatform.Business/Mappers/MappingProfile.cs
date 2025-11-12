using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Data.Entities;
using Profile = AutoMapper.Profile;

namespace GreenConnectPlatform.Business.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ScrapPost, ScrapPostModel>();
        CreateMap<ScrapPostCreateModel, ScrapPost>().ForMember(dest => dest.Location, opt => opt.Ignore());
        ;
        CreateMap<ScrapPost, ScrapPostOverralModel>();
        CreateMap<ScrapPostDetailCreateModel, ScrapPostDetail>();
        CreateMap<ScrapPostDetail, ScrapPostDetailModel>();
        CreateMap<ScrapPostUpdateModel, ScrapPost>()
            .ForMember(dest => dest.Location, opt => opt.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<ScrapPostDetailUpdateModel, ScrapPostDetail>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}