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
        private readonly IBankAccountService _bankAccountService;

        public CreditService(IEntityRepository<BankAccount> bankAccountRepository, IEntityRepository<Credit> creditRepository, IApplicationUserManager applicationUserManager, IBankAccountService bankAccountService)
        {
            _bankAccountRepository = bankAccountRepository;
            _creditRepository = creditRepository;
            _applicationUserManager = applicationUserManager;
            _bankAccountService = bankAccountService;
        }

        public ResultDto CreateCredit(CreditDto creditDto)
        {
            var bankAccount = _bankAccountRepository.GetSingle(creditDto.UserId, ba => ba.ApplicationIdentityUser, ba => ba.BankAccountType, ba => ba.Credits);

            if (bankAccount == null)
                return new ResultDto() { Success = false, Message = "The referenced bank account does not exist" };

            if (bankAccount.Credits.Any(c => c.PaidInFull == false))
                return new ResultDto() { Success = false, Message = "You already have obtained a credit and it's not fully paid." };

            if (ValidateCreditPeriod(creditDto) == false)
                return new ResultDto() { Success = false, Message = "Provided credit period is invalid." };

            if (ValidateCreditAmount(creditDto, bankAccount.BankAccountType.Name) == false)
                return new ResultDto() { Success = false, Message = "Provided credit amount is not suitable for your account type" };

            decimal percentageRate = GetPercentageRate(creditDto, bankAccount.BankAccountType.Name);
            creditDto.PercentageRate = percentageRate;

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

            return new ResultDto()
            {
                Success = true,
                Message = "You have submitted a credit request. " +
                    "Since we are taking care about your safety, the request will be analyzed by our staff. " +
                    "You will be contacted within 24 hours in order to confirm the credit. "
            };
        }

        public decimal GetInstallmentAmount(CreditDto creditDto)
        {
            var q = ((creditDto.PercentageRate / 100) / 12) + 1;
            var helper = Math.Pow((double)q, creditDto.InstallmentCount);

            var installment = creditDto.CreditAmount * (decimal)helper * (q - 1) / ((decimal)helper - 1);
            return installment;
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

        public decimal GetPercentageRate(CreditDto creditDto, string bankAccountType)
        {
            if (bankAccountType == "Regular")
            {
                if (creditDto.CreditAmount < 30000m)
                    return 12m;
                else
                    return 10m;
            }
            else if (bankAccountType == "Corporate")
            {
                if (creditDto.CreditAmount < 50000m)
                    return 9.5m;
                else
                    return 7m;
            }
            else
                return 0m;
        }

        public bool ValidateCreditPeriod(CreditDto creditDto)
        {
            if (creditDto.InstallmentCount < 12 || creditDto.InstallmentCount > 120)
                return false;
            else
                return true;
        }

        public void ConfirmCredit(int userId)
        {
            var credit = _creditRepository.GetAll().SingleOrDefault(c => c.BankAccountId == userId && c.Confirmed == false && c.PaidInFull == false);
            credit.Confirmed = true;
            credit.ConfirmationDate = DateTime.Now;
            _creditRepository.Update(credit);

            _bankAccountService.GiveCash(credit.CreditAmount, userId);
        }

        public ResultDto GetCalculatedInstallment(CreditDto creditDto)
        {
            if (creditDto.CreditAmount <= 0)
                return new ResultDto() { Success = false, Message = "The entered amount has to be greater than 0" };
            if (creditDto.CreditAmount >= 10000000m)
                return new ResultDto { Success = false, Message = "The entered amount has to be less than 10 million" };
            if (creditDto.PercentageRate <= 0 || creditDto.PercentageRate > 100)
                return new ResultDto() { Success = false, Message = "Percentage rate has to be between 0 and 100%" };
            if (creditDto.InstallmentCount <= 0 || creditDto.InstallmentCount > 120 || creditDto.InstallmentCount % 12 != 0)
                return new ResultDto() { Success = false, Message = "We do not support credits for such periods" };

            decimal installment = GetInstallmentAmount(creditDto);
            return new ResultDto() { Success = true, Message = installment.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) };
        }
    }
}
