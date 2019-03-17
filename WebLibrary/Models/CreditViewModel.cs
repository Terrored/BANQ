using System.ComponentModel.DataAnnotations;

namespace WebLibrary.Models
{
    public class CreditViewModel
    {
        [Required]
        public decimal CreditAmount { get; set; }

        [Required]
        public decimal PercentageRate { get; set; }

        [Required]
        public int InstallmentCount { get; set; }
    }
}