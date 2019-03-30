using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface ICreditService
    {
        bool ValidateCreditPeriod(CreditDto creditDto);
        bool ValidateCreditAmount(CreditDto creditDto, string bankAccountType);
        decimal GetInstallmentAmount(CreditDto creditDto);
        decimal GetPercentageRate(CreditDto creditDto, string bankAccountType);
        ResultDto CreateCredit(CreditDto creditDto);
        void ConfirmCredit(int userId);
    }
}
