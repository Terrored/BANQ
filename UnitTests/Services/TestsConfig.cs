using AutoMapper;
using BusinessLogic.DTOs;
using DataAccess.Identity;
using NUnit.Framework;

namespace UnitTests.Services
{
    [SetUpFixture]
    public class TestsConfig
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ApplicationIdentityUser, UserDto>();
                cfg.CreateMap<MoneyTransfer, MoneyTransferDto>();
                cfg.CreateMap<BankAccount, BankAccountDto>().
                    ForMember(c => c.BankAccountType, d => d.MapFrom(c => c.BankAccountType.Name)).
                    ForMember(c => c.ApplicationUserId, d => d.MapFrom(c => c.ApplicationIdentityUserId));

                cfg.CreateMap<BankAccountDto, BankAccount>();
                cfg.CreateMap<Credit, CreditDto>().
                    ForMember(cdto => cdto.UserId, opt => opt.MapFrom(c => c.BankAccountId)).
                    ForMember(cdto => cdto.InstallmentCount, opt => opt.MapFrom(c => c.TotalInstallments));

                cfg.CreateMap<Loan, LoanDto>().ForMember(cdto => cdto.UserId, opt => opt.MapFrom(c => c.BankAccountId));
                cfg.CreateMap<CreditInstallment, CreditInstallmentDto>();
                cfg.CreateMap<BankAccountType, BankAccountTypeDto>();

            });
        }

        [OneTimeTearDown]
        public void FinalizeTests()
        {
            Mapper.Reset();
        }
    }
}
