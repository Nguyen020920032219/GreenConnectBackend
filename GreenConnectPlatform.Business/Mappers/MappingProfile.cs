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
        CreateMap<ScrapPostRequest, ScrapPost>();
        CreateMap<ScrapPost, ScrapPostOverral>();
        CreateMap<ScrapPostDetailRequest, ScrapPostDetail>();
        CreateMap<ScrapPostDetail, ScrapPostDetailModel>();
    }
}