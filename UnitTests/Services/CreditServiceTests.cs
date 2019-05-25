using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Identity;
using Model.RepositoryInterfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace UnitTests.Services
{
    [TestFixture]
    public class CreditServiceTests
    {
        private Mock<IEntityRepository<BankAccount>> _bankAccountRepository;
        private Mock<IEntityRepository<Credit>> _creditRepository;
        private Mock<IBankAccountService> _bankAccountService;
        private Mock<IEntityRepository<CreditInstallment>> _creditInstallmentRepository;
        private CreditService InstantiateService()
        {
            return new CreditService(_bankAccountRepository.Object, _creditRepository.Object, _bankAccountService.Object, _creditInstallmentRepository.Object);
        }

        [SetUp]
        public void SetupTest()
        {
            _bankAccountRepository = new Mock<IEntityRepository<BankAccount>>();
            _creditInstallmentRepository = new Mock<IEntityRepository<CreditInstallment>>();
            _bankAccountService = new Mock<IBankAccountService>();
            _creditRepository = new Mock<IEntityRepository<Credit>>();
        }

        [TearDown]
        public void FinalizeTest()
        {
            _bankAccountRepository = null;
            _creditRepository = null;
            _bankAccountService = null;
            _creditInstallmentRepository = null;
        }

        [Test]
        public void CanCreateCredit()
        {
            //arrange
            var creditDto = new CreditDto() { UserId = 1, CreditAmount = 12500m, InstallmentCount = 12 };
            var mockedBankAccout = new BankAccount()
            {
                Credits = new Collection<Credit>(),
                BankAccountType = new BankAccountType() { Name = "Regular" }
            };

            _bankAccountRepository.Setup(ba => ba.GetSingle(It.Is<int>(u => u.Equals(creditDto.UserId)), y => y.BankAccountType, t => t.Credits)).Returns(mockedBankAccout);
            var service = InstantiateService();

            //act
            var result = service.CreateCredit(creditDto);

            //assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("You have submitted a credit request. " +
                    "Since we are taking care about your safety, the request will be analyzed by our staff. " +
                    "You will be contacted within 24 hours in order to confirm the credit. ", result.Message);
            _creditRepository.Verify(c => c.Create(It.IsAny<Credit>()), Times.Once());
        }

        [Test]
        public void CannotCreateCreditWhenHasntRepaidExistingOne()
        {
            //arrange
            var creditDto = new CreditDto() { UserId = 1, CreditAmount = 12500m, InstallmentCount = 12 };
            var mockedBankAccout = new BankAccount()
            {
                Credits = new Collection<Credit>() { new Credit() { PaidInFull = false } },
                BankAccountType = new BankAccountType() { Name = "Regular" }
            };
            _bankAccountRepository.Setup(b => b.GetSingle(It.Is<int>(x => x.Equals(creditDto.UserId)), x => x.BankAccountType, x => x.Credits)).Returns(mockedBankAccout);
            var service = InstantiateService();

            //act
            var result = service.CreateCredit(creditDto);

            //assert
            Assert.AreEqual("You already have obtained a credit and it's not fully paid.", result.Message);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void CannotCreateCreditWithWrongPeriod()
        {
            //arrange
            var creditDto = new CreditDto() { UserId = 1, CreditAmount = 12500m, InstallmentCount = 127 };
            var mockedBankAccout = new BankAccount()
            {
                Credits = new Collection<Credit>(),
                BankAccountType = new BankAccountType() { Name = "Regular" }
            };
            _bankAccountRepository.Setup(b => b.GetSingle(It.Is<int>(x => x.Equals(creditDto.UserId)), x => x.BankAccountType, x => x.Credits)).Returns(mockedBankAccout);
            var service = InstantiateService();

            //act
            var result = service.CreateCredit(creditDto);

            //assert
            Assert.AreEqual("Provided credit period is invalid.", result.Message);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void CannotCreateCreditWithWrongAmount()
        {
            //arrange
            var creditDto = new CreditDto() { UserId = 1, CreditAmount = 60000m, InstallmentCount = 36 };
            var mockedBankAccout = new BankAccount()
            {
                Credits = new Collection<Credit>(),
                BankAccountType = new BankAccountType() { Name = "Regular" }
            };
            _bankAccountRepository.Setup(b => b.GetSingle(It.Is<int>(x => x.Equals(creditDto.UserId)), x => x.BankAccountType, x => x.Credits)).Returns(mockedBankAccout);
            var service = InstantiateService();

            //act
            var result = service.CreateCredit(creditDto);

            //assert
            Assert.AreEqual("Provided credit amount is not suitable for your account type", result.Message);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void CanCalculateInstallment()
        {
            //arrange
            var sampleData = new CreditDto[]
            {
                new CreditDto(){CreditAmount = 100000, PercentageRate = 10, InstallmentCount = 24},
                new CreditDto(){CreditAmount = 20000, PercentageRate = 8.5m, InstallmentCount = 60},
                new CreditDto(){CreditAmount = 5000, PercentageRate = 7.2m, InstallmentCount = 12},
                new CreditDto(){CreditAmount = 1000000, PercentageRate = 12, InstallmentCount = 120},
                new CreditDto(){CreditAmount = 12000, PercentageRate = 50, InstallmentCount = 36}
            };

            var expectedValues = new double[]
            {
                4614.49,
                410.33,
                433.10,
                14347.09,
                649.37
            };

            var service = InstantiateService();
            var results = new List<ResultDto<decimal>>();

            //act
            foreach (var creditDto in sampleData)
                results.Add(service.GetCalculatedInstallment(creditDto));


            //assert
            for (int i = 0; i < expectedValues.Length; i++)
            {
                Assert.IsTrue(results[i].Success);
                Assert.AreEqual(expectedValues[i], (double)results[i].Data, 0.01);
                Assert.AreEqual("We have successfully calculated installment", results[i].Message);
            }
        }

        [Test]
        public void CannotCalculateInstallmentWithCorruptedData()
        {
            //arrange
            var sampleData = new CreditDto[]
            {
                new CreditDto() { CreditAmount = 0},
                new CreditDto() { CreditAmount = -10},
                new CreditDto() { CreditAmount = 10000000},
                new CreditDto() { CreditAmount = 10010000},
                new CreditDto() { CreditAmount = 10000, PercentageRate = 0},
                new CreditDto() { CreditAmount = 10000, PercentageRate = -5},
                new CreditDto() { CreditAmount = 10000, PercentageRate = 120},
                new CreditDto() { CreditAmount = 10000, PercentageRate = 5, InstallmentCount = 0},
                new CreditDto() { CreditAmount = 10000, PercentageRate = 5, InstallmentCount = -4},
                new CreditDto() { CreditAmount = 10000, PercentageRate = 5, InstallmentCount = 124},
                new CreditDto() { CreditAmount = 10000, PercentageRate = 5, InstallmentCount = 65},
            };

            var expected = new string[]
            {
                "The entered amount has to be greater than 0",
                "The entered amount has to be greater than 0",
                "The entered amount has to be less than 10 million",
                "The entered amount has to be less than 10 million",
                "Percentage rate has to be between 0 and 100%",
                "Percentage rate has to be between 0 and 100%",
                "Percentage rate has to be between 0 and 100%",
                "We do not support credits for such periods",
                "We do not support credits for such periods",
                "We do not support credits for such periods",
                "We do not support credits for such periods",
            };

            var service = InstantiateService();
            var results = new List<ResultDto>();

            //act
            foreach (var creditDto in sampleData)
                results.Add(service.GetCalculatedInstallment(creditDto));

            //assert
            for (int i = 0; i < sampleData.Length; i++)
            {
                Assert.IsFalse(results[i].Success);
                Assert.AreEqual(expected[i], results[i].Message);
            }

            Assert.AreEqual(sampleData.Length, expected.Length);
        }

        [Test]
        public void CanCalculatePercentageRate()
        {
            //arrange
            var sampleData = new CreditDto() { UserId = 1, CreditAmount = 10000 };
            var mockedBankAccount = new BankAccount()
            {
                BankAccountType = new BankAccountType() { Name = "Regular" }
            };

            _bankAccountRepository.Setup(m => m.GetSingle(It.Is<int>(a => a.Equals(sampleData.UserId)), b => b.BankAccountType)).Returns(new BankAccount()
            {
                BankAccountType = new BankAccountType() { Name = "Regular" }
            });

            var service = InstantiateService();

            //act
            var result = service.GetCalculatedPercentageRate(sampleData);

            //assert
            Assert.AreEqual(12, result.Data);
        }

        [Test]
        public void CannotCalculatePercentageRateWithCorruptedData()
        {
            //arrange
            var creditDto = new CreditDto() { UserId = 1, PercentageRate = -5 };
            var mockedBankAccount = new BankAccount()
            {
                BankAccountType = new BankAccountType() { Name = "Corporate" }
            };
            _bankAccountRepository.Setup(b => b.GetSingle(It.Is<int>(x => x.Equals(creditDto.UserId)), a => a.BankAccountType)).Returns(mockedBankAccount);
            var service = InstantiateService();

            //act
            var result = service.GetCalculatedPercentageRate(creditDto);

            //assert
            Assert.AreEqual("Unsupported credit amount", result.Message);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void CanGetCreditInfo()
        {
            //arrange
            int userId = 10, creditId = 5;

            var mockedCredit = new Credit()
            {
                Id = creditId,
                BankAccountId = userId
            };

            _creditRepository.Setup(c => c.GetSingle(It.Is<int>(x => x.Equals(creditId)))).Returns(mockedCredit);

            var service = InstantiateService();

            //act
            var result = service.GetCreditInfo(userId, creditId);

            //assert
            Assert.IsTrue(result.Success);
            Assert.IsInstanceOf<CreditDto>(result.Data);
        }

        [Test]
        public void CannotGetCreditInfoForAnotherUsersCredit()
        {
            //assert
            int askingUserId = 1, userIfForCredit = 2, creditId = 10;
            var mockedCredit = new Credit()
            {
                Id = creditId,
                BankAccountId = userIfForCredit
            };
            _creditRepository.Setup(c => c.GetSingle(It.Is<int>(x => x.Equals(creditId)))).Returns(mockedCredit);
            var service = InstantiateService();

            //act
            var result = service.GetCreditInfo(askingUserId, creditId);

            //assert
            Assert.AreEqual("The credit does not exist or you don't have permission to access it", result.Message);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void CannotGetCreditInfoForNotExistingCredit()
        {
            //arrange
            int userId = 10, creditId = 5;

            Credit mockedCredit = null;

            _creditRepository.Setup(c => c.GetSingle(It.Is<int>(x => x.Equals(creditId)))).Returns(mockedCredit);
            var service = InstantiateService();

            //act
            var result = service.GetCreditInfo(userId, creditId);

            //assert
            Assert.AreEqual("The credit does not exist or you don't have permission to access it", result.Message);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void CanGetInstallmentsForCredit()
        {
            //arrange
            int userId = 1, creditId = 2;
            var mockedCredit = new Credit()
            {
                Id = creditId,
                Installments = new Collection<CreditInstallment>(),
                BankAccountId = userId
            };
            _creditRepository.Setup(c => c.GetSingle(It.Is<int>(x => x.Equals(creditId)), a => a.Installments)).Returns(mockedCredit);
            var service = InstantiateService();

            //act
            var result = service.GetInstallmentsForCredit(userId, creditId);

            //assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Installments fetched successfully", result.Message);
            Assert.IsInstanceOf<IEnumerable<CreditInstallmentDto>>(result.Data);
        }

        [Test]
        public void CannotGetInstallmentsForAnotherUsersCredit()
        {
            //arrange
            int askingUserId = 1, userIdForCredit = 2, creditId = 10;
            var mockedCredit = new Credit()
            {
                Id = creditId,
                BankAccountId = userIdForCredit
            };
            _creditRepository.Setup(c => c.GetSingle(It.Is<int>(x => x.Equals(creditId)), a => a.Installments)).Returns(mockedCredit);
            var service = InstantiateService();

            //act
            var result = service.GetInstallmentsForCredit(askingUserId, creditId);

            //assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("The credit does not exist or you don't have permission to access it", result.Message);
        }

        [Test]
        public void CannotGetInstallmentsForNotExistingCredit()
        {
            //arrange
            int askingUserId = 1, creditId = 10;
            Credit mockedCredit = null;
            _creditRepository.Setup(c => c.GetSingle(It.Is<int>(x => x.Equals(creditId)), a => a.Installments)).Returns(mockedCredit);
            var service = InstantiateService();

            //act
            var result = service.GetInstallmentsForCredit(askingUserId, creditId);

            //assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("The credit does not exist or you don't have permission to access it", result.Message);
        }

        [Test]
        public void CanPayInstallment()
        {
            //arrange
            int userId = 1, creditId = 2, installmentId = 3;
            decimal installmentAmount = 100;

            var installmentDto = new CreditInstallmentDto()
            {
                CreditId = creditId,
                Id = installmentId
            };

            var mockedCredit = new Credit()
            {
                BankAccountId = userId,
                Installments = new Collection<CreditInstallment>()
                {
                    new CreditInstallment()
                    {
                        Id = installmentId,
                        InstallmentAmount = installmentAmount,
                        PaymentDeadline = DateTime.Now
                    }
                },
                InstallmentsAlreadyPaid = 10,
                TotalInstallments = 20,
                NextInstallmentDate = DateTime.Now,
            };

            _creditRepository.Setup(c => c.GetSingle(It.Is<int>(x => x.Equals(creditId)), a => a.Installments)).Returns(mockedCredit);
            _bankAccountService.Setup(b => b.TakeCash(It.Is<decimal>(a => a.Equals(installmentAmount)), It.Is<int>(x => x.Equals(userId)))).Returns(true);

            var service = InstantiateService();

            //act
            var result = service.PayInstallment(userId, installmentDto);

            //assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("You have successfully paid an installment", result.Message);
            _creditInstallmentRepository.Verify(c => c.Update(It.IsAny<CreditInstallment>()), Times.Once());
            _creditInstallmentRepository.Verify(c => c.Create(It.IsAny<CreditInstallment>()), Times.Once());
            _creditRepository.Verify(c => c.Update(It.IsAny<Credit>()), Times.Once());
        }

        [Test]
        public void CanPayLateInstallment()
        {
            //arrange
            int userId = 1, creditId = 2, installmentId = 3;
            decimal installmentAmount = 100;

            var installmentDto = new CreditInstallmentDto()
            {
                CreditId = creditId,
                Id = installmentId
            };

            var mockedCredit = new Credit()
            {
                BankAccountId = userId,
                Installments = new Collection<CreditInstallment>()
                {
                    new CreditInstallment()
                    {
                        Id = installmentId,
                        InstallmentAmount = installmentAmount,
                        PaymentDeadline = (DateTime.Now - TimeSpan.FromDays(2)).Date
                    }
                },
                InstallmentsAlreadyPaid = 10,
                TotalInstallments = 20,
                NextInstallmentDate = DateTime.Now,
            };

            _creditRepository.Setup(c => c.GetSingle(It.Is<int>(x => x.Equals(creditId)), a => a.Installments)).Returns(mockedCredit);
            _bankAccountService.Setup(b => b.TakeCash(It.Is<decimal>(a => a.Equals(installmentAmount * 1.1m)), It.Is<int>(x => x.Equals(userId)))).Returns(true);

            var service = InstantiateService();

            //act
            var result = service.PayInstallment(userId, installmentDto);

            //assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("You have successfully paid an installment", result.Message);
            _creditInstallmentRepository.Verify(c => c.Update(It.IsAny<CreditInstallment>()), Times.Once());
            _creditInstallmentRepository.Verify(c => c.Create(It.IsAny<CreditInstallment>()), Times.Once());
            _creditRepository.Verify(c => c.Update(It.IsAny<Credit>()), Times.Once());
        }

        [Test]
        public void CannotPayInstallmentForAnotherUsersCredit()
        {
            //arrange
            int userId = 1, creditId = 2, installmentId = 3, askingUserId = 4;
            decimal installmentAmount = 100;

            var installmentDto = new CreditInstallmentDto()
            {
                CreditId = creditId,
                Id = installmentId
            };

            var mockedCredit = new Credit()
            {
                BankAccountId = userId,
                Installments = new Collection<CreditInstallment>()
                {
                    new CreditInstallment()
                    {
                        Id = installmentId,
                        InstallmentAmount = installmentAmount,
                        PaymentDeadline = DateTime.Now
                    }
                },
                InstallmentsAlreadyPaid = 10,
                TotalInstallments = 20,
                NextInstallmentDate = DateTime.Now,
            };

            _creditRepository.Setup(c => c.GetSingle(It.Is<int>(x => x.Equals(creditId)), a => a.Installments)).Returns(mockedCredit);

            var service = InstantiateService();

            //act
            var result = service.PayInstallment(askingUserId, installmentDto);

            //assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("The credit does not exist or you don't have access to it", result.Message);
        }

        [Test]
        public void CannotPayNotExistingInstallment()
        {
            //arrange
            int userId = 1, creditId = 2, installmentId = 3;

            var installmentDto = new CreditInstallmentDto()
            {
                CreditId = creditId,
                Id = installmentId
            };

            var mockedCredit = new Credit()
            {
                BankAccountId = userId,
                Installments = new Collection<CreditInstallment>(),
                InstallmentsAlreadyPaid = 10,
                TotalInstallments = 20,
                NextInstallmentDate = DateTime.Now,
            };

            _creditRepository.Setup(c => c.GetSingle(It.Is<int>(x => x.Equals(creditId)), a => a.Installments)).Returns(mockedCredit);

            var service = InstantiateService();

            //act
            var result = service.PayInstallment(userId, installmentDto);

            //assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Cannot find the installment", result.Message);
        }

        [Test]
        public void CannotPayInstallmentWithoutMoney()
        {
            //arrange
            int userId = 1, creditId = 2, installmentId = 3;
            decimal installmentAmount = 100;

            var installmentDto = new CreditInstallmentDto()
            {
                CreditId = creditId,
                Id = installmentId
            };

            var mockedCredit = new Credit()
            {
                BankAccountId = userId,
                Installments = new Collection<CreditInstallment>()
                {
                    new CreditInstallment()
                    {
                        Id = installmentId,
                        InstallmentAmount = installmentAmount,
                        PaymentDeadline = DateTime.Now
                    }
                },
                InstallmentsAlreadyPaid = 10,
                TotalInstallments = 20,
                NextInstallmentDate = DateTime.Now,
            };

            _creditRepository.Setup(c => c.GetSingle(It.Is<int>(x => x.Equals(creditId)), a => a.Installments)).Returns(mockedCredit);
            _bankAccountService.Setup(b => b.TakeCash(It.Is<decimal>(a => a.Equals(installmentAmount)), It.Is<int>(x => x.Equals(userId)))).Returns(false);

            var service = InstantiateService();

            //act
            var result = service.PayInstallment(userId, installmentDto);

            //assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Cannot finish transaction", result.Message);
        }

        [Test]
        public void CanGetInstallmentWithPenalty()
        {
            //arrange
            int userId = 1, creditId = 2, installmentId = 3;
            var installmentDto = new CreditInstallmentDto()
            {
                CreditId = creditId,
                Id = installmentId
            };

            var mockedCredit = new Credit()
            {
                BankAccountId = userId,
                Installments = new Collection<CreditInstallment>()
                {
                    new CreditInstallment()
                    {
                        Id = installmentId,
                        PaymentDeadline = (DateTime.Now - TimeSpan.FromDays(2)).Date,
                        InstallmentAmount = 100
                    }
                }
            };

            _creditRepository.Setup(c => c.GetSingle(It.Is<int>(x => x.Equals(creditId)), a => a.Installments)).Returns(mockedCredit);
            var service = InstantiateService();

            //act
            var result = service.GetInstallmentWithPenalty(installmentDto, userId);

            //assert
            Assert.AreEqual("Successfully calculated penalty", result.Message);
            Assert.IsTrue(result.Success);
            Assert.IsInstanceOf<InstallmentPenaltyDto>(result.Data);
            Assert.AreEqual(10, result.Data.PenaltyPercentage);
        }

        [Test]
        public void CanDetermineWhetherCreditIsFullyPaid()
        {
            //arrange
            int userId = 1, paidCreditId = 2, notPaidCreditId = 3;
            var mockedPaidCredit = new Credit()
            {
                Id = paidCreditId,
                PaidInFull = true,
                BankAccountId = userId
            };

            var mockedNotPaidCredit = new Credit()
            {
                Id = notPaidCreditId,
                PaidInFull = false,
                BankAccountId = userId
            };
            _creditRepository.Setup(c => c.GetSingle(It.Is<int>(x => x.Equals(paidCreditId)))).Returns(mockedPaidCredit);
            _creditRepository.Setup(c => c.GetSingle(It.Is<int>(x => x.Equals(notPaidCreditId)))).Returns(mockedNotPaidCredit);

            var service = InstantiateService();

            //act
            var resultPaid = service.IsFullyPaid(paidCreditId, userId);
            var resultNotPaid = service.IsFullyPaid(notPaidCreditId, userId);

            //assert
            Assert.IsTrue(resultPaid.Success);
            Assert.AreEqual("Congratulations, you have successfully repaid whole credit. It will be now moved to credit history. You can still access it there.", resultPaid.Message);
            Assert.IsFalse(resultNotPaid.Success);
        }

        [Test]
        public void CannotDetermineWhetherIsFullyPaidForAnotherUser()
        {
            //arrange
            int userId = 1, creditId = 2, askingUserId = 3;
            var mockedCredit = new Credit()
            {
                Id = creditId,
                PaidInFull = true,
                BankAccountId = userId
            };

            _creditRepository.Setup(c => c.GetSingle(It.Is<int>(x => x.Equals(creditId)))).Returns(mockedCredit);

            var service = InstantiateService();

            //act
            var result = service.IsFullyPaid(creditId, askingUserId);

            //assert
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void CanGetCredits()
        {
            //arrange
            int userId = 1;
            var mockedCreditsCollection = new Collection<Credit>();
            _creditRepository.Setup(c => c.GetAll()).Returns(mockedCreditsCollection.AsQueryable());
            var service = InstantiateService();

            //act
            var result = service.GetCredits(userId);

            //assert
            Assert.IsInstanceOf<IEnumerable<CreditDto>>(result.Data);
        }

        [Test]
        public void CanCheckForActiveCredit()
        {
            //arrange
            int userIdActive = 1, userIdInactive = 2;

            var mockedCredits = new Collection<Credit>()
            {
                new Credit()
                {
                    BankAccountId = userIdActive,
                    PaidInFull = false
                },
                new Credit()
                {
                    BankAccountId = userIdInactive,
                    PaidInFull = true
                }
            };

            _creditRepository.Setup(c => c.GetAll()).Returns(mockedCredits.AsQueryable());
            var service = InstantiateService();

            //act
            var resultActive = service.HasActiveCredit(userIdActive);
            var resultInactive = service.HasActiveCredit(userIdInactive);

            //assert
            Assert.IsTrue(resultActive.Success);
            Assert.IsInstanceOf<CreditDto>(resultActive.Data);
            Assert.IsFalse(resultInactive.Success);
            Assert.AreEqual("Couldn't find the requested credit", resultInactive.Message);
        }
    }
}
