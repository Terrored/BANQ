using System;

namespace Model.Models
{
    public class BaseCredit : BaseEntity
    {
        public decimal CreditAmount { get; set; }
        public decimal PercentageRate { get; set; }
        public int TotalInstallments { get; set; }
        public int InstallmentsAlreadyPaid { get; set; }
        public decimal InstallmentAmount { get; set; }
        public DateTime DateTaken { get; set; }
        public DateTime? NextInstallmentDate { get; set; }
        public bool PaidInFull { get; set; }
    }
}
