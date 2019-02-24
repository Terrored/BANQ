using System;

namespace Model.Models
{
    public class BaseMoneyTransfer : BaseEntity
    {
        public DateTime CreatedOn { get; set; }
        public decimal CashAmount { get; set; }
    }
}
