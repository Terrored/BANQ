using System;

namespace Model.Models
{
    public class BaseMoneyTransfer : BaseEntity
    {
        public DateTime DateTime { get; set; }
        public decimal CashAmount { get; set; }
    }
}
