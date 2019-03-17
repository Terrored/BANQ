using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Identity;
using Model.RepositoryInterfaces;
using System;
using System.Linq;

namespace BusinessLogic
{
    public class CreditService : ICreditService
    {
        private readonly IEntityRepository<BankAccount> _bankAccountRepository;
        private readonly IEntityRepository<Credit> _creditRepository;
        private readonly IApplicationUserManager _applicationUserManager;

        public CreditService(IEntityRepository<BankAccount> bankAccountRepository, IEntityRepository<Credit> creditRepository, IApplicationUserManager applicationUserManager)
        {
            _bankAccountRepository = bankAccountRepository;
            _creditRepository = creditRepository;
            _applicationUserManager = applicationUserManager;
        }

        public void CreateCredit(CreditDto creditDto)
        {
            var bankAccount = _bankAccountRepository.GetSingle(creditDto.UserId, ba => ba.ApplicationIdentityUser);
            var credit = new Credit()
            {
                BankAccount = bankAccount,
                BankAccountId = bankAccount.Id,
                DateTaken = DateTime.Now,
                ConfirmationDate = null,
                Confirmed = creditDto.Confirmed,
                CreditAmount = creditDto.CreditAmount,
                PercentageRate = creditDto.PercentageRate,
                TotalInstallments = creditDto.InstallmentCount
            };

            _creditRepository.Create(credit);
        }

        public void ConfirmCredit(int userId)
        {
            var credit = _creditRepository.GetAll().SingleOrDefault(c => c.BankAccountId == userId);
            credit.Confirmed = true;
            credit.ConfirmationDate = DateTime.Now;
            _creditRepository.Update(credit);
        }
    }
}
