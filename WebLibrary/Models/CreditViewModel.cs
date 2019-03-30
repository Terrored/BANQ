using System.ComponentModel.DataAnnotations;

namespace WebLibrary.Models
{
    public class CreditViewModel
    {
        [Required]
        public decimal CreditAmount { get; set; }

        [Required]
        public int CreditPeriodInYears { get; set; }
    }
}