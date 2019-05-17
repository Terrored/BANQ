using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Identity;
using Model.RepositoryInterfaces;
using System;
using System.Linq;

namespace BusinessLogic.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IEntityRepository<BankAccount> _bankAccountRepository;
        private readonly IEntityRepository<Credit> _creditRepository;

        public BankAccountService(IEntityRepository<BankAccount> bankAccountRepository, IEntityRepository<Credit> creditRepository)
        {
            _bankAccountRepository = bankAccountRepository;
            _creditRepository = creditRepository;
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
            if (bankAccount != null && amount < bankAccount.Cash && amount >= 0)
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
            if (bankAccount != null && amount >= 0)
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

            return credit == null ? false : true;
        }
    }
}
