using BusinessLogic.DTOs;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface ILoanService
    {
        ResultDto TakeLoan(LoanDto loanDto);
        ResultDto PayInstallment(LoanInstallmentDto installmentDto);
        List<LoanDto> GetLoans(int userId);

    }
}
