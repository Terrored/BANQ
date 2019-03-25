using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Identity;
using Model.Models.Enums;
using Model.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    public class LoanService : ILoanService
    {
        private readonly IEntityRepository<Loan> _loanRepository;
        private readonly IEntityRepository<LoanInstallment> _loanInstallmentRepository;
        private readonly IEntityRepository<BankAccount> _bankAccountRepository;
        private readonly IBankAccountService _bankAccountService;

        public LoanService(IEntityRepository<Loan> loanRepository, IEntityRepository<BankAccount> bankAccountRepository, IEntityRepository<LoanInstallment> loanInstallmentRepository, IBankAccountService bankAccountService)
        {
            _loanRepository = loanRepository;
            _bankAccountRepository = bankAccountRepository;
            _loanInstallmentRepository = loanInstallmentRepository;
            _bankAccountService = bankAccountService;
        }

        public ResultDto TakeLoan(LoanDto loanDto)
        {
            var result = new ResultDto();
            var bankAccount = _bankAccountRepository.GetSingle(loanDto.UserId, u => u.ApplicationIdentityUser, t => t.BankAccountType);
            int activeLoans = _loanRepository.GetAll().Where(l => l.BankAccountId == bankAccount.Id).ToList().Count;

            var isValid = CanTakeLoan(activeLoans, bankAccount.BankAccountType);

            if (isValid)
            {
                var id = CreateLoan(loanDto, bankAccount);
                if (id > 0)
                {
                    _bankAccountService.GiveCash(loanDto.LoanAmount, loanDto.UserId);
                    result.Success = true;
                }


            }
            else
            {
                result.Message = "You cannot take more loans - upgrade your bank account or contact support";
            }


            return result;

        }

        public ResultDto PayInstallment(LoanInstallmentDto installmentDto)
        {
            var result = new ResultDto();
            var loan = _loanRepository.GetSingle(installmentDto.LoanId, t => t.BankAccount);
            if (loan.NextInstallmentDate >= DateTime.Now && !loan.Repayment && loan.InstallmentsLeft > 0)
            {


                var installment = new LoanInstallment()
                {
                    InstallmentAmount = loan.InstallmentAmount,
                    LoanId = loan.Id,
                    PaidOn = DateTime.Now
                };


                var isValid = _bankAccountService.TakeCash(installment.InstallmentAmount, loan.BankAccountId);
                if (isValid)
                {
                    loan.InstallmentsLeft--;
                    loan.LoanAmountLeft = loan.LoanAmountLeft - installment.InstallmentAmount;
                    loan.NextInstallmentDate = DateTime.Now.AddDays(1);
                    if (loan.InstallmentsLeft == 0 && loan.LoanAmountLeft == 0)
                    {
                        loan.Repayment = true;
                        loan.RepaymentDate = DateTime.Now;
                    }
                    _loanRepository.Update(loan);
                    _loanInstallmentRepository.Create(installment);

                    result.Success = true;

                }
                else
                {
                    result.Message = "There was a problem with paying installment";

                }

            }
            else
            {
                result.Message = "You cannot pay the installment for this loan. Check date of your next installment or contact support.";
            }

            return result;


        }

        private int CreateLoan(LoanDto loanDto, BankAccount bankAccount)
        {
            var loan = new Loan()
            {
                BankAccount = bankAccount,
                BankAccountId = bankAccount.Id,
                DateTaken = DateTime.Now,
                InstallmentAmount = loanDto.InstallmentAmount,
                InstallmentsLeft = loanDto.TotalInstallments,
                LoanAmount = loanDto.LoanAmount,
                LoanAmountLeft = loanDto.LoanAmount,
                NextInstallmentDate = DateTime.Now.AddDays(1),
                PercentageRate = 10,
                Repayment = false,
                TotalInstallments = loanDto.TotalInstallments,
                Installments = new List<LoanInstallment>()

            };
            // var loan = Mapper.Map<LoanDto, Loan>(loanDto);

            return _loanRepository.CreateAndReturnId(loan);
        }

        private bool CanTakeLoan(int numberOfActiveLoans, BankAccountType bankAccountType)
        {
            if (bankAccountType.Name ==
                Enum.GetName(typeof(BankAccountTypeEnum), BankAccountTypeEnum.Corporate))
            {
                return true;
            }
            else if (numberOfActiveLoans <= 2 && bankAccountType.Name ==
                     Enum.GetName(typeof(BankAccountTypeEnum), BankAccountTypeEnum.Regular))
            {
                return true;
            }
            else if (numberOfActiveLoans <= 0 && bankAccountType.Name ==
                     Enum.GetName(typeof(BankAccountTypeEnum), BankAccountTypeEnum.Student))
            {
                return true;
            }

            return false;
        }
    }
}
