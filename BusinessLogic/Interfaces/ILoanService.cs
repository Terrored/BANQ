using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface ILoanService
    {
        void TakeLoan(LoanDto loanDto);
        void PayInstallment(LoanInstallmentDto installmentDto);
    }
}
