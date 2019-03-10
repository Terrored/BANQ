using BusinessLogic.DTOs;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface IMoneyTransferService
    {
        ResultDto Transfer(MoneyTransferDto moneyTransferDto);
        List<MoneyTransferDto> GetLastSentFiveTransfers(int userId);
        List<MoneyTransferDto> GetAllTransfers(int userId);
    }
}
