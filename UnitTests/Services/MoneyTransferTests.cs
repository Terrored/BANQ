using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Identity;
using Model.RepositoryInterfaces;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;

namespace UnitTests.Services
{
    [TestFixture]
    public class MoneyTransferTests
    {
        private Mock<IBankAccountService> _bankAccountServiceMock;
        private Mock<IEntityRepository<MoneyTransfer>> _moneyTransferRepositoryMock;

        public IMoneyTransferService InstantiateService()
        {
            return new MoneyTransferService(_moneyTransferRepositoryMock.Object, _bankAccountServiceMock.Object);
        }

        [SetUp]
        public void Setup()
        {
            _bankAccountServiceMock = new Mock<IBankAccountService>();
            _moneyTransferRepositoryMock = new Mock<IEntityRepository<MoneyTransfer>>();
        }

        [TearDown]
        public void Teardown()
        {
            _bankAccountServiceMock = null;
            _moneyTransferRepositoryMock = null;
        }

        [Test]
        public void Do_Not_Make_Transfer_From_User_To_Himself()
        {
            //Arrange
            var toUser = new UserDto { Id = 1 };
            var fromUser = new UserDto { Id = 1 };
            var moneyTransferDto = new MoneyTransferDto
            { CashAmount = 100M, CreatedOn = DateTime.Now, From = fromUser, Id = 1, Name = "Test", To = toUser };
            var service = InstantiateService();
            //Act
            var result = service.Transfer(moneyTransferDto);
            //Assert
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("You cannot transfer money to yourself!", result.Message);
        }

        [Test]
        public void Do_Not_Make_Transfer_When_User_Does_Not_Have_Enough_Money()
        {
            //Arrange
            var toUser = new UserDto { Id = 1 };
            var fromUser = new UserDto { Id = 2 };
            var moneyTransferDto = new MoneyTransferDto
            { CashAmount = 100M, CreatedOn = DateTime.Now, From = fromUser, Id = 1, Name = "Test", To = toUser };
            _bankAccountServiceMock.Setup(s => s.TakeCash(It.Is<decimal>(m => m.Equals(moneyTransferDto.CashAmount)),
                It.Is<int>(i => i.Equals(moneyTransferDto.From.Id)))).Returns(false);
            var service = InstantiateService();
            //Act
            var result = service.Transfer(moneyTransferDto);
            //Assert
            _bankAccountServiceMock.VerifyAll();
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(
                "Error occurs when trying to transfer money from customer. Make sure you have enough money.",
                result.Message);
        }

        [Test]
        public void Do_Not_Make_Transfer_When_User_Gives_Wrong_Account_Number()
        {
            //Arrange
            var toUser = new UserDto { Id = 1 };
            var fromUser = new UserDto { Id = 2 };
            var moneyTransferDto = new MoneyTransferDto
            { CashAmount = 100M, CreatedOn = DateTime.Now, From = fromUser, Id = 1, Name = "Test", To = toUser };
            _bankAccountServiceMock.Setup(s => s.TakeCash(It.Is<decimal>(m => m.Equals(moneyTransferDto.CashAmount)),
                It.Is<int>(i => i.Equals(moneyTransferDto.From.Id)))).Returns(true);
            _bankAccountServiceMock.Setup(s => s.GiveCash(It.Is<decimal>(m => m.Equals(moneyTransferDto.CashAmount)),
                It.Is<int>(i => i.Equals(moneyTransferDto.To.Id)))).Returns(false);
            var service = InstantiateService();
            //Act
            var result = service.Transfer(moneyTransferDto);
            //Assert
            _bankAccountServiceMock.VerifyAll();
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(
                "Error occurs when trying to transfer money from customer. Make sure you've provided valid account number",
                result.Message);
        }

        [Test]
        public void Create_Successful_Transfer()
        {
            //Arrange
            var toUser = new UserDto { Id = 1 };
            var fromUser = new UserDto { Id = 2 };
            var moneyTransferDto = new MoneyTransferDto
            { CashAmount = 100M, CreatedOn = DateTime.Now, From = fromUser, Id = 1, Name = "Test", To = toUser };
            _bankAccountServiceMock.Setup(s => s.TakeCash(It.Is<decimal>(m => m.Equals(moneyTransferDto.CashAmount)),
                It.Is<int>(i => i.Equals(moneyTransferDto.From.Id)))).Returns(true);
            _bankAccountServiceMock.Setup(s => s.GiveCash(It.Is<decimal>(m => m.Equals(moneyTransferDto.CashAmount)),
                It.Is<int>(i => i.Equals(moneyTransferDto.To.Id)))).Returns(true);
            var moneyTransfer = new MoneyTransfer();
            _moneyTransferRepositoryMock.Setup(s => s.Create(It.Is<MoneyTransfer>(m =>
                m.ToId == toUser.Id && m.CashAmount == moneyTransferDto.CashAmount && m.FromId == fromUser.Id)));
            var service = InstantiateService();
            //Act
            var result = service.Transfer(moneyTransferDto);
            //Assert
            _bankAccountServiceMock.VerifyAll();
            _moneyTransferRepositoryMock.VerifyAll();
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Transfer has been successful", result.Message);
        }

    }
}