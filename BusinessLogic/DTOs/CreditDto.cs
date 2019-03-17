namespace BusinessLogic.DTOs
{
    public class CreditDto
    {
        public decimal CreditAmount { get; set; }
        public decimal PercentageRate { get; set; }
        public int InstallmentCount { get; set; }
        public bool Confirmed { get; set; }
        public int UserId { get; set; }
    }
}
