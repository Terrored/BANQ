using BusinessLogic.DTOs;
using System.Collections.Generic;

namespace WebLibrary.Models
{
    public class BankAccountViewModel
    {
        public IEnumerable<BankAccountTypeDto> BankAccountTypes { get; set; }
        public BankAccountDto BankAccountDto { get; set; }
    }
}