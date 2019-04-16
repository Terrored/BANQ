using System;

namespace Model.Models
{
    public class BaseCreditInstallment : BaseEntity
    {
        public decimal InstallmentAmount { get; set; }
        public DateTime PaymentDeadline { get; set; }
        public DateTime? PaidOn { get; set; }
    }
}
