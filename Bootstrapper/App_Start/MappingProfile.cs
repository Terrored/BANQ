using AutoMapper;
using DataAccess.DTOs;
using DataAccess.Identity;

namespace Bootstrapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationIdentityUser, UserDto>();
        }
    }
}
