using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Identity;
using Model.RepositoryInterfaces;
using System;

namespace BusinessLogic
{
    public class MoneyTransferService
    {
        private readonly IApplicationUserManager _applicationUserManager;
        private readonly IEntityRepository<MoneyTransfer> _moneyTransferRepository;
        private readonly IEntityRepository<BankAccount> _bankAccountTransferRepository;
        private readonly IBankAccountService _bankAccountService;

        public MoneyTransferService(IEntityRepository<MoneyTransfer> moneyTransferRepository, IApplicationUserManager applicationUserManager, IEntityRepository<BankAccount> bankAccountTransferRepository, IBankAccountService bankAccountService)
        {
            _applicationUserManager = applicationUserManager;
            _moneyTransferRepository = moneyTransferRepository;
            _bankAccountTransferRepository = bankAccountTransferRepository;
            _bankAccountService = bankAccountService;
        }

        public MoneyTransferDto Transfer(decimal amount, int fromId, int toId)
        {
            var successTransferFrom = _bankAccountService.TakeCash(amount, fromId);
            MoneyTransferDto dto = new MoneyTransferDto();
            if (successTransferFrom)
            {
                dto.Message = "Error occurs when trying to transfer money from customer";
                var successTransferTo = _bankAccountService.GiveCash(amount, toId);

                if (successTransferTo)
                {
                    var moneyTransfer = new MoneyTransfer()
                    {
                        CashAmount = amount,
                        CreatedOn = DateTime.Now,
                        From = fromId,
                        To = toId,
                    };

                    _moneyTransferRepository.Create(moneyTransfer);
                    dto = MoneyTransferDto.ToDto(moneyTransfer);
                    dto.Message = "Transfer has been successful";

                }
                else
                {
                    dto.Message = "Error occurs when trying to transfer money to customer";
                }
            }

            return dto;

        }

    }
}
