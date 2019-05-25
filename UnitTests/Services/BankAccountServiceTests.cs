using BusinessLogic.DTOs;
using BusinessLogic.Services;
using DataAccess.Identity;
using Model.RepositoryInterfaces;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Services
{
    [TestFixture]
    public class BankAccountServiceTests
    {
        private Mock<IEntityRepository<BankAccount>> _bankAccountRepository;
        private Mock<IEntityRepository<Credit>> _creditRepository;
        private BankAccountService InstantiateService()
        {
            return new BankAccountService(_bankAccountRepository.Object, _creditRepository.Object);
        }

        [SetUp]
        public void SetupTest()
        {
            _bankAccountRepository = new Mock<IEntityRepository<BankAccount>>();
            _creditRepository = new Mock<IEntityRepository<Credit>>();
        }

        [TearDown]
        public void FinalizeTest()
        {
            _bankAccountRepository = null;
            _creditRepository = null;
        }


        [Test]
        public void CanCheckWhetherUserHasAccount()
        {
            //arrange
            int userIdAccount = 1, userIdNoAccount = 2;
            var mockedBankAccounts = new BankAccount[]
            {
                new BankAccount() { ApplicationIdentityUserId = userIdAccount }
            };

            _bankAccountRepository.Setup(b => b.GetAll()).Returns(mockedBankAccounts.AsQueryable());

            var service = InstantiateService();

            //act
            var resultAccount = service.UserAlreadyHasAccount(userIdAccount);
            var resultNoAccount = service.UserAlreadyHasAccount(userIdNoAccount);

            //assert
            Assert.IsTrue(resultAccount);
            Assert.IsFalse(resultNoAccount);
        }

        [Test]
        public void CanCreateBankAccout()
        {
            //arrange
            var bankAccountDto = new BankAccountDto();
            var service = InstantiateService();

            //act
            service.CreateBankAccount(bankAccountDto);

            //assert
            _bankAccountRepository.Verify(g => g.Create(It.IsAny<BankAccount>()), Times.Once());

        }

        [Test]
        public void CanGetBankAccountDetails()
        {
            //arrange
            int userIdAccount = 1, userIdNoAccount = 2;

            var mockedBankAccounts = new BankAccount[]
            {
                new BankAccount() {ApplicationIdentityUserId = userIdAccount}
            };

            _bankAccountRepository.Setup(b => b.GetAll(a => a.BankAccountType)).Returns(mockedBankAccounts.AsQueryable());
            var service = InstantiateService();

            //act
            var resultAccount = service.GetBankAccountDetails(userIdAccount);
            var resultNoAccount = service.GetBankAccountDetails(userIdNoAccount);

            //assert
            Assert.IsInstanceOf<BankAccountDto>(resultAccount);
            Assert.AreEqual(userIdAccount, resultAccount.ApplicationUserId);
            Assert.IsNull(resultNoAccount);
        }

        [Test]
        public void CanTakeCash()
        {
            //arrange
            decimal amountToTake = 20m; int userId = 1;
            var mockedBankAccount = new BankAccount()
            {
                ApplicationIdentityUserId = userId,
                Cash = 100m
            };
            _bankAccountRepository.Setup(b => b.GetSingle(It.Is<int>(a => a.Equals(userId)))).Returns(mockedBankAccount);
            _bankAccountRepository.Setup(b => b.Update(It.Is<BankAccount>(a => a.Equals(mockedBankAccount))));

            var service = InstantiateService();

            //act
            var result = service.TakeCash(amountToTake, userId);

            //assert
            Assert.IsTrue(result);
            _bankAccountRepository.Verify(b => b.Update(It.Is<BankAccount>(a => a.Equals(mockedBankAccount))), Times.Once());
            Assert.AreEqual(80m, mockedBankAccount.Cash);
        }

        [Test]
        public void CannotTakeCashWithoutEnoughFunds()
        {
            //arrange
            decimal amountToTake = 20m; int userId = 1;
            var mockedBankAccount = new BankAccount()
            {
                ApplicationIdentityUserId = userId,
                Cash = 10m
            };
            _bankAccountRepository.Setup(b => b.GetSingle(It.Is<int>(a => a.Equals(userId)))).Returns(mockedBankAccount);

            var service = InstantiateService();

            //act
            var result = service.TakeCash(amountToTake, userId);

            //assert
            Assert.IsFalse(result);
            _bankAccountRepository.Verify(b => b.Update(It.Is<BankAccount>(a => a.Equals(mockedBankAccount))), Times.Never());
            Assert.AreEqual(10m, mockedBankAccount.Cash);
        }

        [Test]
        public void CannotTakeNegativeCash()
        {
            //arrange
            decimal amountToTake = -20m; int userId = 1;
            var mockedBankAccount = new BankAccount()
            {
                ApplicationIdentityUserId = userId,
                Cash = 10m
            };
            _bankAccountRepository.Setup(b => b.GetSingle(It.Is<int>(a => a.Equals(userId)))).Returns(mockedBankAccount);

            var service = InstantiateService();

            //act
            var result = service.TakeCash(amountToTake, userId);

            //assert
            Assert.IsFalse(result);
            _bankAccountRepository.Verify(b => b.Update(It.Is<BankAccount>(a => a.Equals(mockedBankAccount))), Times.Never());
            Assert.AreEqual(10m, mockedBankAccount.Cash);
        }

        [Test]
        public void CannotTakeCashFromNotExistingAccount()
        {
            //arrange
            decimal amountToTake = 20m; int askingUserId = 2;

            var service = InstantiateService();

            //act
            var result = service.TakeCash(amountToTake, askingUserId);

            //assert
            Assert.IsFalse(result);
            _bankAccountRepository.Verify(b => b.Update(It.IsAny<BankAccount>()), Times.Never());
        }

        [Test]
        public void CanGiveCash()
        {
            //arrange
            decimal cashToGive = 20m; int userId = 1;
            var mockedBankAccount = new BankAccount()
            {
                ApplicationIdentityUserId = userId,
                Cash = 100m
            };
            _bankAccountRepository.Setup(b => b.GetSingle(It.Is<int>(x => x.Equals(userId)))).Returns(mockedBankAccount);
            var service = InstantiateService();

            //act
            var result = service.GiveCash(cashToGive, userId);

            //assert
            Assert.IsTrue(result);
            _bankAccountRepository.Verify(a => a.Update(It.Is<BankAccount>(x => x.Equals(mockedBankAccount))), Times.Once());
            Assert.AreEqual(120m, mockedBankAccount.Cash);
        }

        [Test]
        public void CannotGiveNegativeCash()
        {
            //arrange
            decimal cashToGive = -20m; int userId = 1;
            var mockedBankAccount = new BankAccount()
            {
                ApplicationIdentityUserId = userId,
                Cash = 100m
            };
            _bankAccountRepository.Setup(b => b.GetSingle(It.Is<int>(x => x.Equals(userId)))).Returns(mockedBankAccount);
            var service = InstantiateService();

            //act
            var result = service.GiveCash(cashToGive, userId);

            //assert
            Assert.IsFalse(result);
            _bankAccountRepository.Verify(b => b.Update(It.IsAny<BankAccount>()), Times.Never());
            Assert.AreEqual(100m, mockedBankAccount.Cash);
        }

        [Test]
        public void CannotGiveCashToNotExistingAccount()
        {
            //arrange
            decimal cashToGive = 20m; int userId = 1;
            var service = InstantiateService();

            //act
            var result = service.GiveCash(cashToGive, userId);

            //assert
            Assert.IsFalse(result);
            _bankAccountRepository.Verify(b => b.Update(It.IsAny<BankAccount>()), Times.Never());
        }

        [Test]
        public void CanCheckForUnconfirmedCredit()
        {
            //arrange
            int userIdConfirmed = 1, userIdUnconfirmed = 2;
            var mockedCredits = new Credit[]
            {
                new Credit()
                {
                    BankAccountId = 1,
                    Confirmed = true
                },
                new Credit()
                {
                    BankAccountId = 2,
                    Confirmed = false
                }
            };
            _creditRepository.Setup(c => c.GetAll()).Returns(mockedCredits.AsQueryable());
            var service = InstantiateService();

            //act
            var resultConfirmed = service.HasUnconfirmedCredit(userIdConfirmed);
            var resultUnconfirmed = service.HasUnconfirmedCredit(userIdUnconfirmed);

            //assert
            Assert.IsTrue(resultUnconfirmed);
            Assert.IsFalse(resultConfirmed);
        }
    }
}
