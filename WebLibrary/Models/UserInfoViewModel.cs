using BusinessLogic.DTOs;

namespace WebLibrary.Models
{
    public class UserInfoViewModel
    {
        public BankAccountDto BankAccount { get; set; }
        public string UserName { get; set; }
    }
}