using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface ICreditService
    {
        void CreateCredit(CreditDto creditDto);
        void ConfirmCredit(int userId);
    }
}
