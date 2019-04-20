using BusinessLogic.DTOs;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface ICreditService
    {
        ResultDto CreateCredit(CreditDto creditDto);
        ResultDto GetCalculatedInstallment(CreditDto creditDto);
        ResultDto GetCalculatedPercentageRate(CreditDto creditDto);
        void ConfirmCredit(int userId);
        CreditDto GetCurrentCreditInfo(int userId);

        IEnumerable<CreditInstallmentDto> GetInstallmentsForCredit(int userId, int creditId);
        ResultDto PayInstallment(int userId, CreditInstallmentDto installmentDto);
        ResultDto IsFullyPaid(int creditId, int userId);
        InstallmentPenaltyDto GetInstallmentWithPenalty(CreditInstallmentDto creditInstallmentDto, int userId);
        IEnumerable<CreditDto> GetCredits(int userId);
    }
}
