using System;

namespace Model.Models
{
    public class BaseLoan : BaseEntity
    {
        public decimal LoanAmount { get; set; }
        public decimal PercentageRate { get; set; }
        public int TotalInstallments { get; set; }
        public int InstallmentsLeft { get; set; }
        public DateTime DateTaken { get; set; }
        public DateTime? NextInstallmentDate { get; set; }
        public bool Repayment { get; set; }
        public DateTime? RepaymentDate { get; set; }
    }
}
