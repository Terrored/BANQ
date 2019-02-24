using DataAccess.Identity;
using System;

namespace BusinessLogic.DTOs
{
    public class MoneyTransferDto
    {
        public int Id { get; set; }
        public decimal CashAmount { get; set; }
        public DateTime CreatedOn { get; set; }

        public ApplicationIdentityUser From { get; set; }
        public ApplicationIdentityUser To { get; set; }

        public string Message { get; set; }


        public static MoneyTransferDto ToDto(MoneyTransfer moneyTransfer)
        {
            return new MoneyTransferDto()
            {
                CashAmount = moneyTransfer.CashAmount,
                CreatedOn = moneyTransfer.CreatedOn,
                From = moneyTransfer.From,
                To = moneyTransfer.To,
                Id = moneyTransfer.Id

            };
        }

    }
}
