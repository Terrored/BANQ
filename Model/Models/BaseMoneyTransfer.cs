using System;

namespace Model.Models
{
    public class BaseMoneyTransfer : BaseEntity
    {
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal CashAmount { get; set; }
    }
}
