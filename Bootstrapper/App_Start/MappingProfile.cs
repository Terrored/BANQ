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
            CreateMap<BankAccount, BankAccountDto>().
                ForMember(c => c.BankAccountType, d => d.MapFrom(c => c.BankAccountType.Name)).
                ForMember(c => c.ApplicationUserId, d => d.MapFrom(c => c.ApplicationIdentityUserId));

            CreateMap<BankAccountDto, BankAccount>();
            CreateMap<Credit, CreditDto>().
                ForMember(cdto => cdto.UserId, opt => opt.MapFrom(c => c.BankAccountId)).
                ForMember(cdto => cdto.InstallmentCount, opt => opt.MapFrom(c => c.TotalInstallments));

            CreateMap<Loan, LoanDto>().ForMember(cdto => cdto.UserId, opt => opt.MapFrom(c => c.BankAccountId));
            CreateMap<CreditInstallment, CreditInstallmentDto>();
            CreateMap<BankAccountType, BankAccountTypeDto>();
        }

        public static void Initialize()
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new MappingProfile()));
        }
    }
}
