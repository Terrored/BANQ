namespace BusinessLogic.DTOs
{
    public class CreditDto
    {
        public int Id { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal PercentageRate { get; set; }
        public int InstallmentCount { get; set; }
        public int InstallmentsAlreadyPaid { get; set; }
        public bool Confirmed { get; set; }
        public bool PaidInFull { get; set; }
        public int UserId { get; set; }
    }
}
