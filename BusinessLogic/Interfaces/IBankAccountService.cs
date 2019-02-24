using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface IBankAccountService
    {
        BankAccountDto CreateBankAccount(int userId);
        bool TakeCash(decimal amount, int userId);
        bool GiveCash(decimal amount, int userId);
    }
}
