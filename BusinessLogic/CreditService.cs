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

        public ResultDto CreateCredit(CreditDto creditDto)
        {
            var bankAccount = _bankAccountRepository.GetSingle(creditDto.UserId, ba => ba.ApplicationIdentityUser, ba => ba.BankAccountType);

            if (bankAccount == null)
                return new ResultDto() { Success = false, Message = "The referenced bank account does not exist" };

            if (bankAccount.CreditId != 0)
                return new ResultDto() { Success = false, Message = "You already have obtained a credit and it's not fully paid." };

            if (ValidateCreditPeriod(creditDto) == false)
                return new ResultDto() { Success = false, Message = "Provided credit period is invalid." };

            if (ValidateCreditAmount(creditDto, bankAccount.BankAccountType.Name) == false)
                return new ResultDto() { Success = false, Message = "Provided credit amount is not suitable for your account type" };

            decimal percentageRate = GetPercentageRate(creditDto, bankAccount.BankAccountType.Name);
            decimal installmentAmount = GetInstallmentAmount(creditDto);

            var credit = new Credit()
            {
                BankAccount = bankAccount,
                BankAccountId = bankAccount.Id,
                CreditAmount = creditDto.CreditAmount,
                PercentageRate = percentageRate,
                DateTaken = DateTime.Now,
                Confirmed = false,
                ConfirmationDate = null,
                InstallmentAmount = installmentAmount,
                InstallmentsAlreadyPaid = 0,
                PaidInFull = false,
                TotalInstallments = creditDto.InstallmentCount,
                NextInstallmentDate = null,
            };

            _creditRepository.Create(credit);

            return new ResultDto() { Success = true, Message = "You have submitted a credit. Please wait for confirmation from our staff." };
        }

        //temporary
        public decimal GetInstallmentAmount(CreditDto creditDto)
        {
            return 10m;
        }

        public void ConfirmCredit(int userId)
        {
            var credit = _creditRepository.GetAll().SingleOrDefault(c => c.BankAccountId == userId);
            credit.Confirmed = true;
            credit.ConfirmationDate = DateTime.Now;
            _creditRepository.Update(credit);
        }

        public bool ValidateCreditAmount(CreditDto creditDto, string bankAccountType)
        {
            if (bankAccountType == "Regular")
            {
                if (creditDto.CreditAmount >= 2000m && creditDto.CreditAmount <= 50000m)
                    return true;
                else
                    return false;
            }
            else if (bankAccountType == "Corporate")
            {
                if (creditDto.CreditAmount >= 10000m && creditDto.CreditAmount <= 100000m)
                    return true;
                else
                    return false;
            }
            else if (bankAccountType == "Savings")
            {
                return false;
            }
            else if (bankAccountType == "Student")
            {
                return false;
            }
            else
            {
                //uncommon situation
                return false;
            }
        }

        //temporary
        public decimal GetPercentageRate(CreditDto creditDto, string bankAccountType)
        {
            return 5m;
        }

        public bool ValidateCreditPeriod(CreditDto creditDto)
        {
            if (creditDto.InstallmentCount < 12 || creditDto.InstallmentCount > 120)
                return false;
            else
                return true;
        }
    }
}
