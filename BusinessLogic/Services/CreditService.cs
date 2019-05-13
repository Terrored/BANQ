using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Identity;
using Model.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Services
{
    public class CreditService : ICreditService
    {
        private readonly IEntityRepository<BankAccount> _bankAccountRepository;
        private readonly IEntityRepository<Credit> _creditRepository;
        private readonly IBankAccountService _bankAccountService;
        private readonly IEntityRepository<CreditInstallment> _creditInstallmentsRepository;

        public CreditService(IEntityRepository<BankAccount> bankAccountRepository,
            IEntityRepository<Credit> creditRepository,
            IBankAccountService bankAccountService,
            IEntityRepository<CreditInstallment> creditInstallmentsRepository)
        {
            _bankAccountRepository = bankAccountRepository;
            _creditRepository = creditRepository;
            _bankAccountService = bankAccountService;
            _creditInstallmentsRepository = creditInstallmentsRepository;
        }

        public ResultDto CreateCredit(CreditDto creditDto)
        {
            var bankAccount = _bankAccountRepository.GetSingle(creditDto.UserId, ba => ba.BankAccountType, ba => ba.Credits);

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

        public ResultDto<decimal> GetCalculatedInstallment(CreditDto creditDto)
        {
            if (creditDto.CreditAmount <= 0)
            {
                return new ResultDto<decimal>() { Success = false, Message = "The entered amount has to be greater than 0" };
            }

            if (creditDto.CreditAmount >= 10000000m)
            {
                return new ResultDto<decimal>() { Success = false, Message = "The entered amount has to be less than 10 million" };
            }

            if (creditDto.PercentageRate <= 0 || creditDto.PercentageRate > 100)
            {
                return new ResultDto<decimal>() { Success = false, Message = "Percentage rate has to be between 0 and 100%" };
            }

            if (creditDto.InstallmentCount <= 0 || creditDto.InstallmentCount > 120 || creditDto.InstallmentCount % 12 != 0)
            {
                return new ResultDto<decimal>() { Success = false, Message = "We do not support credits for such periods" };
            }

            decimal installment = GetInstallmentAmount(creditDto);
            return new ResultDto<decimal>() { Success = true, Message = "We have successfully calculated installment", Data = installment };
        }

        public ResultDto<decimal> GetCalculatedPercentageRate(CreditDto creditDto)
        {
            if (creditDto.CreditAmount <= 0 || creditDto.CreditAmount > 10000000m)
            {
                return new ResultDto<decimal>() { Success = false, Message = "Unsupported credit amount" };
            }

            var bankAccount = _bankAccountRepository.GetSingle(creditDto.UserId, ba => ba.BankAccountType);
            var bankAccountType = bankAccount.BankAccountType.Name;

            var rate = GetPercentageRate(creditDto, bankAccountType);
            return new ResultDto<decimal>() { Success = true, Message = "We have successfully calculated percentage rate", Data = rate };
        }

        public void ConfirmCredit(int userId)
        {
            var credit = _creditRepository.GetAll().SingleOrDefault(c => c.BankAccountId == userId && c.Confirmed == false && c.PaidInFull == false);

            if (credit == null)
                return;

            credit.Confirmed = true;
            credit.ConfirmationDate = DateTime.Now;
            credit.NextInstallmentDate = (DateTime.Now + TimeSpan.FromDays(1)).Date;

            var firstInstallment = new CreditInstallment()
            {
                CreditId = credit.Id,
                InstallmentAmount = credit.InstallmentAmount,
                PaymentDeadline = credit.NextInstallmentDate.Value
            };

            _creditInstallmentsRepository.Create(firstInstallment);
            _creditRepository.Update(credit);

            _bankAccountService.GiveCash(credit.CreditAmount, userId);
        }

        public ResultDto<CreditDto> GetCreditInfo(int userId, int creditId)
        {
            var credit = _creditRepository.GetSingle(creditId);

            if (credit == null || credit.BankAccountId != userId)
                return new ResultDto<CreditDto>() { Success = false, Message = "The credit does not exist or you don't have permission to access it" };
            else
                return new ResultDto<CreditDto>() { Success = true, Data = Mapper.Map<CreditDto>(credit) };
        }

        public ResultDto<IEnumerable<CreditInstallmentDto>> GetInstallmentsForCredit(int userId, int creditId)
        {
            var credit = _creditRepository.GetSingle(creditId, c => c.Installments);

            if (credit == null || credit.BankAccountId != userId)
                return new ResultDto<IEnumerable<CreditInstallmentDto>> { Success = false, Message = "The credit does not exist or you don't have permission to access it" };

            return new ResultDto<IEnumerable<CreditInstallmentDto>>
            {
                Data = Mapper.Map<IEnumerable<CreditInstallmentDto>>(credit.Installments.OrderByDescending(ci => ci.PaymentDeadline)),
                Message = "Installments fetched successfully",
                Success = true
            };
        }

        public ResultDto PayInstallment(int userId, CreditInstallmentDto installmentDto)
        {
            var credit = _creditRepository.GetSingle(installmentDto.CreditId, c => c.Installments);
            if (credit == null || credit.BankAccountId != userId)
                return new ResultDto() { Success = false, Message = "The credit does not exist or you don't have access to it" };

            var installment = credit.Installments.SingleOrDefault(ci => ci.Id == installmentDto.Id);
            if (installment == null)
                return new ResultDto() { Success = false, Message = "Cannot find the installment" };

            var penaltyPercentage = CalculatePenalty(installment.PaymentDeadline);
            installment.InstallmentAmount += installment.InstallmentAmount * (penaltyPercentage / 100m);

            var result = _bankAccountService.TakeCash(installment.InstallmentAmount, userId);

            if (!result)
                return new ResultDto() { Success = false, Message = "Cannot finish transaction" };

            credit.InstallmentsAlreadyPaid++;

            installment.PaidOn = DateTime.Now;
            _creditInstallmentsRepository.Update(installment);

            if (credit.InstallmentsAlreadyPaid == credit.TotalInstallments)
                credit.PaidInFull = true;
            else
            {
                credit.NextInstallmentDate += TimeSpan.FromDays(1);
                var nextInstallment = new CreditInstallment()
                {
                    CreditId = credit.Id,
                    InstallmentAmount = credit.InstallmentAmount,
                    PaymentDeadline = credit.NextInstallmentDate.Value
                };
                _creditInstallmentsRepository.Create(nextInstallment);
            }

            _creditRepository.Update(credit);
            return new ResultDto() { Success = true, Message = "You have successfully paid an installment" };
        }

        public ResultDto IsFullyPaid(int creditId, int userId)
        {
            var credit = _creditRepository.GetSingle(creditId);

            if (credit == null || credit.BankAccountId != userId)
                return new ResultDto() { Success = false };

            if (credit.PaidInFull)
                return new ResultDto() { Success = true, Message = "Congratulations, you have successfully repaid whole credit. It will be now moved to credit history. You can still access it there." };
            else
                return new ResultDto() { Success = false };
        }

        public ResultDto<InstallmentPenaltyDto> GetInstallmentWithPenalty(CreditInstallmentDto creditInstallmentDto, int userId)
        {
            var credit = _creditRepository.GetSingle(creditInstallmentDto.CreditId, c => c.Installments);

            if (credit == null || credit.BankAccountId != userId)
                return new ResultDto<InstallmentPenaltyDto>() { Success = false, Message = "The credit does not exist or you don't have permission to access it" };

            var installment = credit.Installments.SingleOrDefault(i => i.Id == creditInstallmentDto.Id);

            if (installment == null)
                return new ResultDto<InstallmentPenaltyDto>() { Success = false, Message = "Specified installment does not exist" };

            var penalty = CalculatePenalty(installment.PaymentDeadline);
            var installmentPenalty = new InstallmentPenaltyDto()
            {
                Amount = installment.InstallmentAmount + (installment.InstallmentAmount * penalty / 100m),
                PenaltyPercentage = penalty
            };
            return new ResultDto<InstallmentPenaltyDto>() { Success = true, Message = "Successfully calculated penalty", Data = installmentPenalty };
        }

        public ResultDto<IEnumerable<CreditDto>> GetCredits(int userId)
        {
            var credits = _creditRepository.GetAll().Where(c => c.BankAccountId == userId).OrderByDescending(c => c.DateTaken);
            return new ResultDto<IEnumerable<CreditDto>> { Data = Mapper.Map<IEnumerable<CreditDto>>(credits) };
        }

        public ResultDto<CreditDto> HasActiveCredit(int userId)
        {
            var credit = _creditRepository.GetAll().SingleOrDefault(c => c.BankAccountId == userId && c.PaidInFull == false);

            if (credit == null)
                return new ResultDto<CreditDto>() { Success = false, Message = "Couldn't find the requested credit" };
            else
                return new ResultDto<CreditDto>() { Success = true, Data = Mapper.Map<CreditDto>(credit) };
        }

        #region Private helpers

        private decimal GetInstallmentAmount(CreditDto creditDto)
        {
            var q = ((creditDto.PercentageRate / 100) / 12) + 1;
            var helper = Math.Pow((double)q, creditDto.InstallmentCount);

            var installment = creditDto.CreditAmount * (decimal)helper * (q - 1) / ((decimal)helper - 1);
            return installment;
        }

        private bool ValidateCreditAmount(CreditDto creditDto, string bankAccountType)
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

        private decimal GetPercentageRate(CreditDto creditDto, string bankAccountType)
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

        private bool ValidateCreditPeriod(CreditDto creditDto)
        {
            if (creditDto.InstallmentCount < 12 || creditDto.InstallmentCount > 120 || creditDto.InstallmentCount % 12 != 0)
                return false;
            else
                return true;
        }

        private decimal CalculatePenalty(DateTime deadline)
        {
            var comparison = DateTimeOffset.Compare(DateTime.Now, deadline + TimeSpan.FromDays(1));
            if (comparison <= 0)
                return 0;
            var timeInterval = (DateTime.Now - deadline).Days;
            return timeInterval * 5;
        }

        #endregion
    }
}
