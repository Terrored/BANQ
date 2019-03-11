namespace BusinessLogic.DTOs
{
    public class MoneyTransfersMetadataDto
    {
        public int TransfersSent { get; set; }
        public int TransfersReceived { get; set; }
        public decimal TotalMoneySent { get; set; }
        public decimal TotalMoneyReceived { get; set; }
    }
}
