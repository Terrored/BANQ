using System;

namespace BusinessLogic.DTOs
{
    public class LoanDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
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
