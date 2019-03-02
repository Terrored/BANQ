using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Identity;
using Model.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    public class MoneyTransferService : IMoneyTransferService
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
            MoneyTransferDto dto = new MoneyTransferDto();
            if (fromId != toId)
            {
                var successTransferFrom = _bankAccountService.TakeCash(amount, fromId);
                if (successTransferFrom)
                {

                    var successTransferTo = _bankAccountService.GiveCash(amount, toId);

                    if (successTransferTo)
                    {
                        var moneyTransfer = new MoneyTransfer()
                        {
                            CashAmount = amount,
                            CreatedOn = DateTime.Now,
                            FromId = fromId,
                            ToId = toId,
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
                else
                {
                    dto.Message = "Error occurs when trying to transfer money from customer";
                }

            }
            else
            {
                dto.Message = "You cannot transfer money to yourself!";
            }


            return dto;

        }

        public List<MoneyTransferDto> GetLastSentFiveTransfers(int userId)
        {
            var transfers = _moneyTransferRepository.GetAll(t => t.From, t => t.To).OrderByDescending(t => t.CreatedOn).Take(5).ToList();

            List<MoneyTransferDto> dtos = new List<MoneyTransferDto>();

            foreach (var moneyTransfer in transfers)
            {
                dtos.Add(MoneyTransferDto.ToDto(moneyTransfer));
            }

            return dtos;
        }

    }
}
