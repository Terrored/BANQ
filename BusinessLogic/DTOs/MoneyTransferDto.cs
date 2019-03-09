using System;

namespace BusinessLogic.DTOs
{
    public class MoneyTransferDto
    {
        public UserDto From { get; set; }
        public UserDto To { get; set; }
        public decimal CashAmount { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
