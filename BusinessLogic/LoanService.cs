using BusinessLogic.Interfaces;
using DataAccess.Identity;
using Model.RepositoryInterfaces;
using System;

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

        public void TakeLoan(int userId)
        {
            var bankAccount = _bankAccountRepository.GetSingle(userId, u => u.ApplicationIdentityUser);
            var loan = new Loan
            {
                BankAccount = bankAccount,
                BankAccountId = bankAccount.Id,
                DateTaken = DateTime.Now,
                TotalInstallments = 12,
                InstallmentsLeft = 12,
                LoanAmount = 2000M,
                PercentageRate = 5M,

            };

            var loanId = _loanRepository.CreateAndReturnId(loan);

            var installment = new LoanInstallment()
            {
                LoanId = loanId,
                InstallmentAmount = 10M,
                PaidOn = DateTime.Now
            };

            _loanInstallmentRepository.Create(installment);



        }
    }
}
