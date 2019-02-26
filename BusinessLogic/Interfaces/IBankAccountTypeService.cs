using BusinessLogic.DTOs;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface IBankAccountTypeService
    {
        IEnumerable<BankAccountTypeDto> GetBankAccountTypes();
    }
}
