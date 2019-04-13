using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface ICreditService
    {
        ResultDto CreateCredit(CreditDto creditDto);
        ResultDto GetCalculatedInstallment(CreditDto creditDto);
        ResultDto GetCalculatedPercentageRate(CreditDto creditDto);
        void ConfirmCredit(int userId);
    }
}
