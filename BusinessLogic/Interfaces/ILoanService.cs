﻿using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface ILoanService
    {
        void TakeLoan(LoanDto loanDto);
        ResultDto PayInstallment(LoanInstallmentDto installmentDto);
    }
}
