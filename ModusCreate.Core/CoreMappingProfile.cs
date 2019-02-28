using AutoMapper;
using ModusCreate.Core.DAL.Domain;
using ModusCreate.Core.Models;

namespace ModusCreate.Core
{
    public class CoreMappingProfile : Profile
    {
        public CoreMappingProfile()
        {
            CreateMap<UserEntity, User>();
            CreateMap<FeedEntity, Feed>();
            CreateMap<NewsEntity, News>();
            CreateMap<FeedCategoryEntity, FeedCategory>();
        }
    }
}
