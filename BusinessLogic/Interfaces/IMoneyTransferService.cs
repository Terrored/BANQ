using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface IMoneyTransferService
    {
        MoneyTransferDto Transfer(decimal amount, int fromId, int toId);
    }
}
