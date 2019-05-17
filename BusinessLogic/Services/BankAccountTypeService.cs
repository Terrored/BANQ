using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Identity;
using Model.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Services
{
    public class BankAccountTypeService : IBankAccountTypeService
    {
        private readonly IEntityRepository<BankAccountType> _bankAccountTypeRepository;

        public BankAccountTypeService(IEntityRepository<BankAccountType> bankAccountTypeRepository)
        {
            _bankAccountTypeRepository = bankAccountTypeRepository;
        }

        public IEnumerable<BankAccountTypeDto> GetBankAccountTypes()
        {
            var types = _bankAccountTypeRepository.GetAll().AsEnumerable();
            foreach (var type in types)
            {
                yield return Mapper.Map<BankAccountTypeDto>(type);
            }
        }
    }
}
