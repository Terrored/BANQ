using System;

namespace Model.Models
{
    public class BaseLoanInstallment : BaseEntity
    {
        public decimal InstallmentAmount { get; set; }
        public DateTime PaidOn { get; set; }
    }
}
