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
        public int BankAccountTypeId { get; set; }
        public string BankAccountType { get; set; }


        public static BankAccountDto ToDto(BankAccount bankAccount)
        {
            return new BankAccountDto()
            {
                ApplicationUserId = bankAccount.ApplicationIdentityUserId,
                Cash = bankAccount.Cash,
                CreatedOn = bankAccount.CreatedOn,
                Id = bankAccount.Id,
                BankAccountType = bankAccount.BankAccountType.Name
            };
        }
    }
}
