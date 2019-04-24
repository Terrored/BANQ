using System;
using System.Linq;
using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Identity;
using Model.RepositoryInterfaces;

namespace BusinessLogic.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IEntityRepository<BankAccount> _bankAccountRepository;
        private readonly IEntityRepository<Credit> _creditRepository;
        private readonly IApplicationUserManager _applicationUserManager;

        public BankAccountService(IEntityRepository<BankAccount> bankAccountRepository, IEntityRepository<Credit> creditRepository, IApplicationUserManager applicationUserManager)
        {
            _bankAccountRepository = bankAccountRepository;
            _creditRepository = creditRepository;
            _applicationUserManager = applicationUserManager;
        }

        public bool UserAlreadyHasAccount(int userId)
        {
            var bankAccount = _bankAccountRepository.GetAll().FirstOrDefault(ba => ba.ApplicationIdentityUserId == userId);
            return bankAccount != null ? true : false;
        }

        public void CreateBankAccount(BankAccountDto bankAccountDto)
        {
            var bankAccount = new BankAccount()
            {
                ApplicationIdentityUserId = bankAccountDto.ApplicationUserId,
                Cash = 100,
                CreatedOn = DateTime.Now,
                Id = bankAccountDto.ApplicationUserId,
                BankAccountTypeId = bankAccountDto.BankAccountTypeId
            };

            _bankAccountRepository.Create(bankAccount);
        }

        public BankAccountDto GetBankAccountDetails(int userId)
        {
            var bankAccount = _bankAccountRepository.GetAll(b => b.BankAccountType).FirstOrDefault(ba => ba.ApplicationIdentityUserId == userId);

            if (bankAccount == null)
            {
                return null;
            }
            else
            {
                return Mapper.Map<BankAccount, BankAccountDto>(bankAccount);
            }
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

        public bool HasUnconfirmedCredit(int userId)
        {
            var bankAccountId = userId;
            var credit = _creditRepository.GetAll().SingleOrDefault(c => c.BankAccountId == bankAccountId && c.Confirmed == false);

            if (credit == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
