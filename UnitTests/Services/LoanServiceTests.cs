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
    public static class MockExtensions
    {
        public static void SetupIQueryable<TRepository, TEntity>(this Mock<TRepository> mock, IQueryable<TEntity> queryable)
            where TRepository : class, IQueryable<TEntity>
        {
            mock.Setup(r => r.GetEnumerator()).Returns(queryable.GetEnumerator());
            mock.Setup(r => r.Provider).Returns(queryable.Provider);
            mock.Setup(r => r.ElementType).Returns(queryable.ElementType);
            mock.Setup(r => r.Expression).Returns(queryable.Expression);
        }
    }

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
        public void Can_Student_Take_Loan()
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

        }
        [Test]
        public void Can_Corporate_Take_Loan()
        {
            //Arrange

            LoanDto loanDto = new LoanDto { LoanAmount = 600, UserId = 1 };
            bankAccountRepositoryMock.Setup(x => x.GetSingle(It.Is<int>(y => y.Equals(loanDto.UserId)), u => u.ApplicationIdentityUser, t => t.BankAccountType)).Returns(new BankAccount() { Id = 4, BankAccountType = new BankAccountType { Name = BankAccountTypeEnum.Corporate.ToString("G") } });
            loanRepositoryMock.Setup(x => x.GetAll()).Returns(new Loan[] { new Loan { BankAccountId = 4 }, new Loan { BankAccountId = 4 } }.AsQueryable);
            loanRepositoryMock.Setup(x => x.CreateAndReturnId(It.IsAny<Loan>())).Returns(222);

            //Act
            var obj = new LoanService(loanRepositoryMock.Object, bankAccountRepositoryMock.Object, loanInstallmentRepositoryMock.Object, bankAccountServiceMock.Object);
            var result = obj.TakeLoan(loanDto);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Success);

        }
    }
}
