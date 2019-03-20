using System;

namespace BusinessLogic.DTOs
{
    public class LoanInstallmentDto
    {
        public int Id { get; set; }
        public int LoanId { get; set; }
        public decimal InstallmentAmount { get; set; }
        public DateTime PaidOn { get; set; }
    }
}
