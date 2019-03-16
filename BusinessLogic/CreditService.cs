using BusinessLogic.Interfaces;
using DataAccess.Identity;
using Model.RepositoryInterfaces;
using System;

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

        public void CreateCredit(int userId)
        {
            var bankAccount = _bankAccountRepository.GetSingle(userId, ba => ba.ApplicationIdentityUser);
            var credit = new Credit()
            {
                BankAccount = bankAccount,
                BankAccountId = bankAccount.Id,
                DateTaken = DateTime.Now
            };

            _creditRepository.Create(credit);
        }
    }
}
