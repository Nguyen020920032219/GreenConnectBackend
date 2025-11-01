using GreenConnectPlatform.Bussiness.Models.ScrapPosts;
using GreenConnectPlatform.Bussiness.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Data.Entities;
using Profile = AutoMapper.Profile;

namespace GreenConnectPlatform.Bussiness.Mappers;

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