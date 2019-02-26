using DataAccess.Identity;
using System;

namespace BusinessLogic.DTOs
{
    public class BankAccountDto
    {
        public int Id { get; set; }
        public decimal Cash { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ApplicationUserId { get; set; }
        public string Message { get; set; }
        public int BankAccountTypeId { get; set; }

        public static BankAccountDto ToDto(BankAccount bankAccount, string message = null)
        {
            return new BankAccountDto()
            {
                ApplicationUserId = bankAccount.ApplicationIdentityUserId,
                Cash = bankAccount.Cash,
                CreatedOn = bankAccount.CreatedOn,
                Id = bankAccount.Id,
                Message = message
            };
        }
    }
}
