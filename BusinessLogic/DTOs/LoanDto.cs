using System;

namespace BusinessLogic.DTOs
{
    public class LoanDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal LoanAmountLeft { get; set; }
        public decimal InstallmentAmount { get; set; }
        public decimal InstallmentsLeft { get; set; }
        public decimal PercentageRate { get; set; }
        public DateTime? NextInstallmentDate { get; set; }
        public int TotalInstallments { get; set; }

    }
}
