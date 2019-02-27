namespace WebLibrary.Models.ViewModels
{
    public class MoneyTransferViewModel
    {
        public decimal CashAmount { get; set; }
        public int? FromId { get; set; }
        public int ToId { get; set; }
    }
}