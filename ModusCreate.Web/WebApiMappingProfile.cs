using AutoMapper;
using ModusCreate.Core.Models;
using ModusCreate.Web.Models;

namespace ModusCreate.Web
{
    public class WebApiMappingProfile : Profile
    {
        public WebApiMappingProfile()
        {
            CreateMap<SignupModel, User>()
                .ForMember(x => x.UserName, c => c.MapFrom(x => x.Email));

        }
    }
}
