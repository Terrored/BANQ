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


    }
}
