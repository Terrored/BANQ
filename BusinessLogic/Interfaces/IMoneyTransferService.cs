using BusinessLogic.DTOs;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface IMoneyTransferService
    {
        MoneyTransferDto Transfer(decimal amount, int fromId, int toId);
        List<MoneyTransferDto> GetLastSentFiveTransfers(int userId);
    }
}
