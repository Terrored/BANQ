using System;

namespace Model.Models
{
    public class BaseBankAccount : BaseEntity
    {
        public decimal Cash { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
