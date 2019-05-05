using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Identity;
using Model.Models.Enums;
using Model.RepositoryInterfaces;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Services
{

    [TestFixture]
    public class LoanServiceTests
    {
        private Mock<IEntityRepository<BankAccount>> bankAccountRepositoryMock;
        private Mock<IEntityRepository<Loan>> loanRepositoryMock;
        private Mock<IEntityRepository<LoanInstallment>> loanInstallmentRepositoryMock;
        private Mock<IBankAccountService> bankAccountServiceMock;

        [SetUp]
        public void SetUp()
        {
            bankAccountRepositoryMock = new Mock<IEntityRepository<BankAccount>>();
            loanRepositoryMock = new Mock<IEntityRepository<Loan>>();
            loanInstallmentRepositoryMock = new Mock<IEntityRepository<LoanInstallment>>();
            bankAccountServiceMock = new Mock<IBankAccountService>();
        }


        [Test]
        public void Can_Student_Take_Loan_When_Has_Loan_Already()
        {
            //Arrange

            LoanDto loanDto = new LoanDto { LoanAmount = 600, UserId = 1 };

            bankAccountRepositoryMock.Setup(x => x.GetSingle(It.Is<int>(y => y.Equals(loanDto.UserId)), u => u.ApplicationIdentityUser, t => t.BankAccountType)).Returns(new BankAccount() { Id = 4, BankAccountType = new BankAccountType { Name = BankAccountTypeEnum.Student.ToString("G") } });
            loanRepositoryMock.Setup(x => x.GetAll()).Returns(new Loan[] { new Loan { BankAccountId = 4 } }.AsQueryable);
            //Act
            var obj = new LoanService(loanRepositoryMock.Object, bankAccountRepositoryMock.Object, loanInstallmentRepositoryMock.Object, bankAccountServiceMock.Object);
            var result = obj.TakeLoan(loanDto);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("You cannot take more loans - upgrade your bank account or contact support", result.Message);

        }
        [Test]
        public void Can_Corporate_Take_Loan_When_Has_Loan_Already()
        {
            //Arrange

            LoanDto loanDto = new LoanDto { LoanAmount = 600, UserId = 1 };

            bankAccountRepositoryMock.Setup(x => x.GetSingle(It.Is<int>(y => y.Equals(loanDto.UserId)), u => u.ApplicationIdentityUser, t => t.BankAccountType)).Returns(new BankAccount() { Id = 4, BankAccountType = new BankAccountType { Name = BankAccountTypeEnum.Corporate.ToString("G") } });
            loanRepositoryMock.Setup(x => x.GetAll()).Returns(new Loan[] { new Loan { BankAccountId = 4 }, new Loan { BankAccountId = 4 } }.AsQueryable);
            loanRepositoryMock.Setup(x => x.CreateAndReturnId(It.IsAny<Loan>())).Returns(222);
            bankAccountServiceMock.Setup(x => x.GiveCash(It.Is<decimal>(c => c.Equals(loanDto.LoanAmount)), It.Is<int>(y => y.Equals(loanDto.UserId)))).Returns(true);
            //Act
            var obj = new LoanService(loanRepositoryMock.Object, bankAccountRepositoryMock.Object, loanInstallmentRepositoryMock.Object, bankAccountServiceMock.Object);
            var result = obj.TakeLoan(loanDto);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success);

        }

        [Test]
        public void Cannot_Transfer_Cash_When_Taking_Loan()
        {
            //Arrange
            LoanDto loanDto = new LoanDto { LoanAmount = 600, UserId = 1 };
            bankAccountRepositoryMock.Setup(x => x.GetSingle(It.Is<int>(y => y.Equals(loanDto.UserId)), u => u.ApplicationIdentityUser, t => t.BankAccountType)).Returns(new BankAccount() { Id = 4, BankAccountType = new BankAccountType { Name = BankAccountTypeEnum.Corporate.ToString("G") } });
            loanRepositoryMock.Setup(x => x.GetAll()).Returns(new Loan[] { new Loan { BankAccountId = 4 }, new Loan { BankAccountId = 4 } }.AsQueryable);
            loanRepositoryMock.Setup(x => x.CreateAndReturnId(It.IsAny<Loan>())).Returns(222);
            bankAccountServiceMock.Setup(x => x.GiveCash(It.Is<decimal>(c => c.Equals(loanDto.LoanAmount)), It.Is<int>(y => y.Equals(loanDto.UserId)))).Returns(false);
            //Act
            var obj = new LoanService(loanRepositoryMock.Object, bankAccountRepositoryMock.Object, loanInstallmentRepositoryMock.Object, bankAccountServiceMock.Object);
            var result = obj.TakeLoan(loanDto);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("There was a problem with money transfer. Contact support", result.Message);
        }

        [Test]
        public void Cannot_Take_Loan_With_Wrong_LoanAmount([Values(100, 100000)] decimal loanAmount)
        {
            //Arrange
            LoanDto loanDto = new LoanDto { LoanAmount = loanAmount, UserId = 1 };
            //Act
            var obj = new LoanService(loanRepositoryMock.Object, bankAccountRepositoryMock.Object, loanInstallmentRepositoryMock.Object, bankAccountServiceMock.Object);
            var result = obj.TakeLoan(loanDto);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("Invalid loan amount! MIN is 500 PLN and MAX is 10000 PLN", result.Message);

        }
    }
}
