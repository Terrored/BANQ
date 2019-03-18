using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Identity;
using Model.Models.Enums;
using Model.RepositoryInterfaces;
using System;
using System.Linq;

namespace BusinessLogic
{
    public class LoanService : ILoanService
    {
        private readonly IEntityRepository<Loan> _loanRepository;
        private readonly IEntityRepository<LoanInstallment> _loanInstallmentRepository;
        private readonly IEntityRepository<BankAccount> _bankAccountRepository;

        public LoanService(IEntityRepository<Loan> loanRepository, IEntityRepository<BankAccount> bankAccountRepository, IEntityRepository<LoanInstallment> loanInstallmentRepository)
        {
            _loanRepository = loanRepository;
            _bankAccountRepository = bankAccountRepository;
            _loanInstallmentRepository = loanInstallmentRepository;
        }

        public void TakeLoan(LoanDto loanDto)
        {

            var bankAccount = _bankAccountRepository.GetSingle(loanDto.UserId, u => u.ApplicationIdentityUser, t => t.BankAccountType);
            int activeLoans = _loanRepository.GetAll().Where(l => l.BankAccountId == bankAccount.Id).ToList().Count;

            var isValid = CanTakeLoan(activeLoans, bankAccount);

            if (isValid)
            {
                CreateLoan(loanDto);
            }





        }

        private void CreateLoan(LoanDto loanDto)
        {
            var loan = new Loan()
            {
                //set
            };

            _loanRepository.CreateAndReturnId(loan);
        }

        private bool CanTakeLoan(int numberOfActiveLoans, BankAccount bankAccount)
        {
            if (bankAccount.BankAccountType.Name ==
                Enum.GetName(typeof(BankAccountTypeEnum), BankAccountTypeEnum.Corporate))
            {
                return true;
            }
            else if (numberOfActiveLoans <= 2 && bankAccount.BankAccountType.Name ==
                     Enum.GetName(typeof(BankAccountTypeEnum), BankAccountTypeEnum.Regular))
            {
                return true;
            }
            else if (numberOfActiveLoans <= 0 && bankAccount.BankAccountType.Name ==
                     Enum.GetName(typeof(BankAccountTypeEnum), BankAccountTypeEnum.Student))
            {
                return true;
            }

            return false;
        }
    }
}
