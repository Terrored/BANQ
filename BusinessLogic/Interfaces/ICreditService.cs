using BusinessLogic.DTOs;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface ICreditService
    {
        ResultDto CreateCredit(CreditDto creditDto);
        ResultDto<decimal> GetCalculatedInstallment(CreditDto creditDto);
        ResultDto<decimal> GetCalculatedPercentageRate(CreditDto creditDto);
        void ConfirmCredit(int userId);
        ResultDto<CreditDto> GetCreditInfo(int userId, int creditId);
        ResultDto<CreditDto> HasActiveCredit(int userId);
        IEnumerable<CreditInstallmentDto> GetInstallmentsForCredit(int userId, int creditId);
        ResultDto PayInstallment(int userId, CreditInstallmentDto installmentDto);
        ResultDto IsFullyPaid(int creditId, int userId);
        ResultDto<InstallmentPenaltyDto> GetInstallmentWithPenalty(CreditInstallmentDto creditInstallmentDto, int userId);
        IEnumerable<CreditDto> GetCredits(int userId);
    }
}
