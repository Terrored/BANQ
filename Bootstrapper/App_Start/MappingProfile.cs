using AutoMapper;
using BusinessLogic.DTOs;
using DataAccess.Identity;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Bootstrapper.MappingProfile), "Initialize")]

namespace Bootstrapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationIdentityUser, UserDto>();
            CreateMap<MoneyTransfer, MoneyTransferDto>();
        }

        public static void Initialize()
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new MappingProfile()));
        }
    }
}
