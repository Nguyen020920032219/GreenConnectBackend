using GreenConnectPlatform.Bussiness.Models.ScrapPosts;
using GreenConnectPlatform.Data.Entities;
using Profile = AutoMapper.Profile;

namespace GreenConnectPlatform.Bussiness.Mappers;

public class BaseMappingProfile : Profile
{
    public BaseMappingProfile()
    {
        CreateMap<ScrapPost, ScrapPostModel>();
        CreateMap<ScrapPostRequest, ScrapPost>();
    }
}