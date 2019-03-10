using System;

namespace BusinessLogic.DTOs
{
    public class MoneyTransferDto
    {
        public int Id { get; set; }
        public UserDto From { get; set; }
        public UserDto To { get; set; }
        public decimal CashAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Name { get; set; }
    }
}
