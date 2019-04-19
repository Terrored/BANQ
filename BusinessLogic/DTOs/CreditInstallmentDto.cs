using System;

namespace BusinessLogic.DTOs
{
    public class CreditInstallmentDto
    {
        public int Id { get; set; }
        public int CreditId { get; set; }
        public decimal InstallmentAmount { get; set; }
        public DateTime? PaidOn { get; set; }
        public DateTime PaymentDeadline { get; set; }
    }
}
