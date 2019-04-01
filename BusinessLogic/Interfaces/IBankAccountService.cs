using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface IBankAccountService
    {
        void CreateBankAccount(BankAccountDto bankAccountDto);
        bool UserAlreadyHasAccount(int userId);
        bool TakeCash(decimal amount, int userId);
        bool GiveCash(decimal amount, int userId);
        bool HasUnconfirmedCredit(int userId);

        BankAccountDto GetBankAccountDetails(int userId);
    }
}
