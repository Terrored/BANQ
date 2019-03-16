using AutoMapper;
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

        public ResultDto Transfer(MoneyTransferDto moneyTransferDto)
        {
            ResultDto resultDto = new ResultDto() { Success = false };

            if (moneyTransferDto.From.Id != moneyTransferDto.To.Id)
            {
                var successTransferFrom = _bankAccountService.TakeCash(moneyTransferDto.CashAmount, moneyTransferDto.From.Id);
                if (successTransferFrom)
                {
                    var successTransferTo = _bankAccountService.GiveCash(moneyTransferDto.CashAmount, moneyTransferDto.To.Id);

                    if (successTransferTo)
                    {
                        var moneyTransfer = new MoneyTransfer()
                        {
                            CashAmount = moneyTransferDto.CashAmount,
                            CreatedOn = DateTime.Now,
                            FromId = moneyTransferDto.From.Id,
                            ToId = moneyTransferDto.To.Id,
                            Name = moneyTransferDto.Name
                        };

                        _moneyTransferRepository.Create(moneyTransfer);

                        resultDto.Success = true;
                        resultDto.Message = "Tranfer has been successful";

                    }
                    else
                    {
                        resultDto.Message = "Error occurs when trying to transfer money from customer. Make sure you've provided valid account number";
                    }
                }
                else
                {
                    resultDto.Message = "Error occurs when trying to transfer money from customer. Make sure you have enough money.";
                }

            }
            else
            {
                resultDto.Message = "You cannot transfer money to yourself!";
            }
            return resultDto;
        }

        public List<MoneyTransferDto> GetLastSentFiveTransfers(int userId)
        {
            var transfers = _moneyTransferRepository.GetAll(t => t.From, t => t.To).
                Where(t => t.ToId == userId || t.FromId == userId).
                OrderByDescending(t => t.CreatedOn).
                Take(5).
                ToList();

            return Mapper.Map<List<MoneyTransferDto>>(transfers);
        }


        public MoneyTransfersMetadataDto GetMetadata(int userId)
        {
            var userTransfers = _moneyTransferRepository.GetAll().Where(t => t.FromId == userId || t.ToId == userId).AsEnumerable();

            var transfersSent = userTransfers.Where(t => t.FromId == userId);
            var transfersReceived = userTransfers.Where(t => t.ToId == userId);

            var metadata = new MoneyTransfersMetadataDto()
            {
                TransfersSent = transfersSent.Count(),
                TransfersReceived = transfersReceived.Count(),
                TotalMoneySent = transfersSent.Sum(t => t.CashAmount),
                TotalMoneyReceived = transfersReceived.Sum(t => t.CashAmount)
            };
            return metadata;
        }
        public List<MoneyTransferDto> GetAllTransfers(int userId)
        {
            var transfers = _moneyTransferRepository.GetAll(t => t.From, t => t.To)
                .Where(t => t.ToId == userId || t.FromId == userId)
                .OrderByDescending(t => t.CreatedOn)
                .ToList();

            return Mapper.Map<List<MoneyTransferDto>>(transfers);

        }
    }
}
