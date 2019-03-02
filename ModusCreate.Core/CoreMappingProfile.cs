using AutoMapper;
using ModusCreate.Core.DAL.Domain;
using ModusCreate.Core.Models;

namespace ModusCreate.Core
{
    public class CoreMappingProfile : Profile
    {
        public CoreMappingProfile()
        {
            CreateMap<User, UserEntity>()
                .ForMember(x => x.UserName, c => c.MapFrom(x => x.Email));

            CreateMap<UserEntity, User>();
            CreateMap<FeedEntity, Feed>()
                .ForMember(x => x.IsSubscribed, c => c.MapFrom(x => true));

            CreateMap<NewsEntity, News>()
                .ForMember(x => x.FeedName, c => c.MapFrom(x => x.Feed.Name));

            CreateMap<FeedCategoryEntity, FeedCategory>();
        }
    }
}
