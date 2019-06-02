using BusinessLogic.DTOs;
using BusinessLogic.Services;
using DataAccess.Identity;
using Model.RepositoryInterfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Services
{
    [TestFixture]
    public class BankAccountTypeServiceTests
    {
        private Mock<IEntityRepository<BankAccountType>> _bankAccountTypeRepository;

        [SetUp]
        public void SetupTest()
        {
            _bankAccountTypeRepository = new Mock<IEntityRepository<BankAccountType>>();
        }

        [TearDown]
        public void FinalizeTest()
        {
            _bankAccountTypeRepository = null;
        }

        [Test]
        public void CanReturnBankAccountTypes()
        {
            //arrange
            var mockedAccountTypes = new BankAccountType[]
            {
                new BankAccountType() {Id = 1, Name = "ABCD"},
                new BankAccountType() {Id = 2, Name = "test"}
            };

            _bankAccountTypeRepository.Setup(t => t.GetAll()).Returns(mockedAccountTypes.AsQueryable());
            var service = new BankAccountTypeService(_bankAccountTypeRepository.Object);

            //act
            var result = service.GetBankAccountTypes();
            var types = result.ToList();

            //assert
            Assert.IsInstanceOf<IEnumerable<BankAccountTypeDto>>(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(1, types[0].Id);
            Assert.AreEqual(2, types[1].Id);
            Assert.AreEqual("ABCD", types[0].Name);
            Assert.AreEqual("test", types[1].Name);
        }
    }
}
