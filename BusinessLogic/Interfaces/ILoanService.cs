using System.Collections.Generic;
using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface ILoanService
    {
        ResultDto TakeLoan(LoanDto loanDto);
        ResultDto PayInstallment(LoanInstallmentDto installmentDto);
        List<LoanDto> GetLoans(int userId);
    }
}
