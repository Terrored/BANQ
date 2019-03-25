using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface ILoanService
    {
        ResultDto TakeLoan(LoanDto loanDto);
        ResultDto PayInstallment(LoanInstallmentDto installmentDto);
    }
}
