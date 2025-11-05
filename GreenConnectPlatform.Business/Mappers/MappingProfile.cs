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
        CreateMap<ScrapPostCreateModel, ScrapPost>();
        CreateMap<ScrapPost, ScrapPostOverralModel>();
        CreateMap<ScrapPostDetailCreateModel, ScrapPostDetail>();
        CreateMap<ScrapPostDetail, ScrapPostDetailModel>();
        CreateMap<ScrapPostUpdateModel, ScrapPost>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<ScrapPostDetailUpdateModel, ScrapPostDetail>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}