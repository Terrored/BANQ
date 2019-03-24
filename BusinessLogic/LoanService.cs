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

        public void TakeLoan(LoanDto loanDto)
        {

            var bankAccount = _bankAccountRepository.GetSingle(loanDto.UserId, u => u.ApplicationIdentityUser, t => t.BankAccountType);
            int activeLoans = _loanRepository.GetAll().Where(l => l.BankAccountId == bankAccount.Id).ToList().Count;

            var isValid = CanTakeLoan(activeLoans, bankAccount.BankAccountType);

            if (isValid)
            {
                CreateLoan(loanDto, bankAccount);
            }

            _bankAccountService.GiveCash(loanDto.LoanAmount, loanDto.UserId);



        }

        public void PayInstallment(LoanInstallmentDto installmentDto)
        {
            //1.sprawdzenie czy można zapłacić ratę dla tego kredytu
            //2. Zabranie pieniędzy
            //3. Stworzenie rekordu rata
            //3.5 Sprawdzenie czy już pożyczka jest spłacona.
            //4. Aktualizacja pożyczki
            //TODO:Nie ma w modelu wysokości raty



            var loan = _loanRepository.GetSingle(installmentDto.LoanId, t => t.BankAccount);
            if (loan.NextInstallmentDate >= DateTime.Now && !loan.Repayment && loan.InstallmentsLeft > 0)
            {

                //Stworz rekord z kwotą i datą
                var installment = new LoanInstallment()
                {
                    InstallmentAmount = loan.InstallmentAmount,
                    LoanId = loan.Id,
                    PaidOn = DateTime.Now
                };

                //InstallmentLeft zmniejsz o 1
                //odejmij kwotę od TODO:LoanAmountLeft 
                //TODO: Adding one day to next installment date
                _bankAccountService.TakeCash(installment.InstallmentAmount, loan.BankAccountId);
                loan.InstallmentsLeft--;
                loan.LoanAmountLeft = loan.LoanAmountLeft - installment.InstallmentAmount;
                _loanRepository.Update(loan);
                _loanInstallmentRepository.Create(installment);




            }


        }

        private void CreateLoan(LoanDto loanDto, BankAccount bankAccount)
        {
            var loan = new Loan()
            {
                BankAccount = bankAccount,
                BankAccountId = bankAccount.Id,
                DateTaken = DateTime.Now,
                InstallmentAmount = 10M,
                InstallmentsLeft = loanDto.TotalInstallments,
                LoanAmount = loanDto.LoanAmount,
                NextInstallmentDate = loanDto.NextInstallmentDate,
                PercentageRate = loanDto.PercentageRate,
                Repayment = loanDto.Repayment,
                TotalInstallments = loanDto.TotalInstallments,
                Installments = new List<LoanInstallment>()

            };
            // var loan = Mapper.Map<LoanDto, Loan>(loanDto);

            _loanRepository.CreateAndReturnId(loan);
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
