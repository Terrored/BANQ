using BusinessLogic.DTOs;

namespace BusinessLogic
{
    public interface IBankAccountService
    {
        BankAccountDto CreateBankAccount(int userId);
    }
}
