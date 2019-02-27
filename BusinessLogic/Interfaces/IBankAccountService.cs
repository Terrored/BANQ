using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface IBankAccountService
    {
        void CreateBankAccount(BankAccountDto bankAccountDto);
        bool UserAlreadyHasAccount(int userId);
        bool TakeCash(decimal amount, int userId);
        bool GiveCash(decimal amount, int userId);

        //consider changing in the future
        BankAccountDto GetBankAccountDetails(int userId);
    }
}
