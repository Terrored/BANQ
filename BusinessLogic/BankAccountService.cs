using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Identity;
using Model.RepositoryInterfaces;
using System;
using System.Linq;

namespace BusinessLogic
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IEntityRepository<BankAccount> _bankAccountRepository;
        private readonly IApplicationUserManager _applicationUserManager;

        public BankAccountService(IEntityRepository<BankAccount> bankAccountRepository, IApplicationUserManager applicationUserManager)
        {
            _bankAccountRepository = bankAccountRepository;
            _applicationUserManager = applicationUserManager;
        }

        public BankAccountDto CreateBankAccount(int userId)
        {
            var bankAccount = _bankAccountRepository.GetAll().FirstOrDefault(ba => ba.ApplicationIdentityUserId == userId);
            BankAccountDto dto;
            if (bankAccount == null)
            {
                bankAccount = new BankAccount() { ApplicationIdentityUserId = userId, Cash = 100, CreatedOn = DateTime.Now, Id = userId };
                _bankAccountRepository.Create(bankAccount);
                dto = BankAccountDto.ToDto(bankAccount, "Account created successfully");
            }
            else
            {
                dto = BankAccountDto.ToDto(bankAccount, "Action failed");
            }
            return dto;
        }

        public bool TakeCash(decimal amount, int userId)
        {
            var bankAccount = _bankAccountRepository.GetSingle(userId);
            bool success = false;
            if (bankAccount != null && amount < bankAccount.Cash)
            {
                bankAccount.Cash = bankAccount.Cash - amount;
                _bankAccountRepository.Update(bankAccount);
                success = true;
            }
            return success;
        }

        public bool GiveCash(decimal amount, int userId)
        {
            var bankAccount = _bankAccountRepository.GetSingle(userId);
            bool success = false;
            if (bankAccount != null)
            {
                bankAccount.Cash = bankAccount.Cash + amount;
                _bankAccountRepository.Update(bankAccount);
                success = true;
            }

            return success;
        }


    }
}
