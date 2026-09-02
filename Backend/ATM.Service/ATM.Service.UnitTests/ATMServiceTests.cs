using ATM.Service.Models;
using ATM.Service.Services;
using Microsoft.VisualBasic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ATM.Service.UnitTests
{
    public class ATMServiceTests
    {

        [TestFixture]
        public class AccountRegisterTests : ATMServiceTests
        {
            private ATMService _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new ATMService();
            }

            [Test]
            public void Should_SetUpBasicAccount()
            {

                var accounts = _sut.GetCurrentBalance();

                Assert.IsNotNull(accounts);
                Assert.AreEqual(2, accounts.Count);
                Assert.AreEqual(0, accounts[0].Amount);
                Assert.AreEqual(0, accounts[1].Amount);
            }

            [Test]
            public void Should_SetPresetData_And_WipeHistory()
            {
                var presetData = new List<Account>()
                {
                    new Account()
                    {
                        Name = "Checking",
                        Amount = 1000
                    },
                    new Account()
                    {
                        Name = "Savings",
                        Amount = 0
                    },
                    new Account()
                    {
                        Name = "Extra",
                        Amount = 3000
                    }
                };

                _sut.DepositFunds(5000, "Checking");

                _sut.RegisterAccountData(presetData);

                var accounts = _sut.GetCurrentBalance();

                Assert.IsNotNull(accounts);
                Assert.AreEqual(3, accounts.Count);
                Assert.AreEqual(1000, accounts[0].Amount);
                Assert.AreEqual(0, accounts[1].Amount);
                Assert.AreEqual(3000, accounts[2].Amount);

                var history = _sut.GetTransactionHistory();

                Assert.IsNotNull(history);
                Assert.IsEmpty(history);
            }
        }

        [TestFixture]
        public class AccountFeatureTests : ATMServiceTests
        {
            private ATMService _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new ATMService();

                var presetData = new List<Account>()
                {
                    new Account()
                    {
                        Name = "Checking",
                        Amount = 10000
                    },
                    new Account()
                    {
                        Name = "Savings",
                        Amount = 0
                    },
                };

                _sut.RegisterAccountData(presetData);
            }

            #region Deposit Tests

            [Test]
            public void Should_DepositAmount()
            {
                _sut.DepositFunds(500.45m, "Checking");

                var accounts = _sut.GetCurrentBalance();

                var checking = accounts.First(a => a.Name == "Checking");

                Assert.IsNotNull(checking);
                Assert.AreEqual(10500.45m, checking.Amount);

                var history = _sut.GetTransactionHistory();
                Assert.IsNotNull(history);
                Assert.IsNotEmpty(history);
                Assert.AreEqual("Deposited $500.45 to Checking. New balance: $10500.45", history[0].Log);
                Assert.AreEqual(nameof(HistoryType.Deposit), history[0].Type);
            }


            [Test]
            public void Should_DepositAmount_RoundToTwoDecimalPlaces()
            {
                _sut.DepositFunds(500.456m, "Checking");

                var accounts = _sut.GetCurrentBalance();

                var checking = accounts.First(a => a.Name == "Checking");

                Assert.IsNotNull(checking);
                Assert.AreEqual(10500.45m, checking.Amount);

                var history = _sut.GetTransactionHistory();
                Assert.IsNotNull(history);
                Assert.IsNotEmpty(history);
                Assert.AreEqual("Deposited $500.45 to Checking. New balance: $10500.45", history[0].Log);
                Assert.AreEqual(nameof(HistoryType.Deposit), history[0].Type);
            }

            [Test]
            public void Should_ThrowException_WhenDepositInputIsNegative()
            {
                Assert.Throws<ArgumentException>(() => _sut.DepositFunds(-500.45m, "Checking"), "Amount must be positive.");

                var accounts = _sut.GetCurrentBalance();

                var checking = accounts.First(a => a.Name == "Checking");

                Assert.IsNotNull(checking);
                Assert.AreEqual(10000, checking.Amount);

                var history = _sut.GetTransactionHistory();
                Assert.IsNotNull(history);
                Assert.IsEmpty(history);
            }

            [Test]
            public void Should_ThrowException_WhenDepositInputIsZero()
            {
                Assert.Throws<ArgumentException>(() => _sut.DepositFunds(0, "Checking"), "Amount must be positive.");

                var accounts = _sut.GetCurrentBalance();

                var checking = accounts.First(a => a.Name == "Checking");

                Assert.IsNotNull(checking);
                Assert.AreEqual(10000, checking.Amount);

                var history = _sut.GetTransactionHistory();
                Assert.IsNotNull(history);
                Assert.IsEmpty(history);
            }

            [Test]
            public void Should_ThrowException_WhenDepositAccountIsntFound()
            {
                Assert.Throws<ArgumentException>(() => _sut.DepositFunds(500.45m, "Check"), "Account Check does not exist.");

                var accounts = _sut.GetCurrentBalance();

                var checking = accounts.First(a => a.Name == "Checking");

                Assert.IsNotNull(checking);
                Assert.AreEqual(10000, checking.Amount);

                var history = _sut.GetTransactionHistory();
                Assert.IsNotNull(history);
                Assert.IsEmpty(history);
            }
            #endregion


            #region Withdraw Tests

            [Test]
            public void Should_WithdrawAmount()
            {
                _sut.WithdrawFunds(500.45m, "Checking");

                var accounts = _sut.GetCurrentBalance();

                var checking = accounts.First(a => a.Name == "Checking");

                Assert.IsNotNull(checking);
                Assert.AreEqual(9499.55m, checking.Amount);

                var history = _sut.GetTransactionHistory();
                Assert.IsNotNull(history);
                Assert.IsNotEmpty(history);
                Assert.AreEqual("Withdrew $500.45 from Checking. New balance: $9499.55", history[0].Log);
                Assert.AreEqual(nameof(HistoryType.Withdrawal), history[0].Type);
            }


            [Test]
            public void Should_WithdrawAmount_RoundToTwoDecimalPlaces()
            {
                _sut.WithdrawFunds(500.456m, "Checking");

                var accounts = _sut.GetCurrentBalance();

                var checking = accounts.First(a => a.Name == "Checking");

                Assert.IsNotNull(checking);
                Assert.AreEqual(9499.55m, checking.Amount);

                var history = _sut.GetTransactionHistory();
                Assert.IsNotNull(history);
                Assert.IsNotEmpty(history);
                Assert.AreEqual("Withdrew $500.45 from Checking. New balance: $9499.55", history[0].Log);
                Assert.AreEqual(nameof(HistoryType.Withdrawal), history[0].Type);
            }

            [Test]
            public void Should_ThrowException_WhenWithdrawInputIsNegative()
            {
                Assert.Throws<ArgumentException>(() => _sut.WithdrawFunds(-500.45m, "Checking"), "Amount must be positive.");

                var accounts = _sut.GetCurrentBalance();

                var checking = accounts.First(a => a.Name == "Checking");

                Assert.IsNotNull(checking);
                Assert.AreEqual(10000, checking.Amount);

                var history = _sut.GetTransactionHistory();
                Assert.IsNotNull(history);
                Assert.IsEmpty(history);
            }

            [Test]
            public void Should_ThrowException_WhenWithdrawInputIsZero()
            {
                Assert.Throws<ArgumentException>(() => _sut.WithdrawFunds(0, "Checking"), "Amount must be positive.");

                var accounts = _sut.GetCurrentBalance();

                var checking = accounts.First(a => a.Name == "Checking");

                Assert.IsNotNull(checking);
                Assert.AreEqual(10000, checking.Amount);

                var history = _sut.GetTransactionHistory();
                Assert.IsNotNull(history);
                Assert.IsEmpty(history);
            }

            [Test]
            public void Should_ThrowException_WhenWithdrawInputIsOverAccountBalance()
            {
                Assert.Throws<ArgumentException>(() => _sut.WithdrawFunds(500, "Savings"), "Insufficient funds in account Savings. Current balance: $0.00");

                var accounts = _sut.GetCurrentBalance();

                var checking = accounts.First(a => a.Name == "Savings");

                Assert.IsNotNull(checking);
                Assert.AreEqual(0, checking.Amount);

                var history = _sut.GetTransactionHistory();
                Assert.IsNotNull(history);
                Assert.IsEmpty(history);
            }

            [Test]
            public void Should_ThrowException_WhenWithdrawAccountIsntFound()
            {
                Assert.Throws<ArgumentException>(() => _sut.WithdrawFunds(500.45m, "Check"), "Account Check does not exist.");

                var accounts = _sut.GetCurrentBalance();

                var checking = accounts.First(a => a.Name == "Checking");

                Assert.IsNotNull(checking);
                Assert.AreEqual(10000, checking.Amount);

                var history = _sut.GetTransactionHistory();
                Assert.IsNotNull(history);
                Assert.IsEmpty(history);
            }
            #endregion


            #region Transfer Tests

            [Test]
            public void Should_TransferAmount()
            {
                _sut.TransferFunds(500.45m, "Checking", "Savings");

                var accounts = _sut.GetCurrentBalance();

                var checking = accounts.First(a => a.Name == "Checking");
                var savings = accounts.First(a => a.Name == "Savings");

                Assert.IsNotNull(checking);
                Assert.AreEqual(9499.55m, checking.Amount);

                Assert.IsNotNull(savings);
                Assert.AreEqual(500.45m, savings.Amount);

                var history = _sut.GetTransactionHistory();
                Assert.IsNotNull(history);
                Assert.IsNotEmpty(history);
                Assert.AreEqual("Transferred $500.45 from Checking to Savings. New balances: Checking: $9499.55, Savings: $500.45", history[0].Log);
                Assert.AreEqual(nameof(HistoryType.Transfer), history[0].Type);
            }


            [Test]
            public void Should_TransferAmount_RoundToTwoDecimalPlaces()
            {
                _sut.TransferFunds(500.456m, "Checking", "Savings");

                var accounts = _sut.GetCurrentBalance();

                var checking = accounts.First(a => a.Name == "Checking");
                var savings = accounts.First(a => a.Name == "Savings");

                Assert.IsNotNull(checking);
                Assert.AreEqual(9499.55m, checking.Amount);

                Assert.IsNotNull(savings);
                Assert.AreEqual(500.45m, savings.Amount);

                var history = _sut.GetTransactionHistory();
                Assert.IsNotNull(history);
                Assert.IsNotEmpty(history);
                Assert.AreEqual("Transferred $500.45 from Checking to Savings. New balances: Checking: $9499.55, Savings: $500.45", history[0].Log);
                Assert.AreEqual(nameof(HistoryType.Transfer), history[0].Type);
            }

            [Test]
            public void Should_ThrowException_WhenTransferInputIsNegative()
            {
                Assert.Throws<ArgumentException>(() => _sut.TransferFunds(-500.45m, "Checking", "Savings"), "Amount must be positive.");

                var accounts = _sut.GetCurrentBalance();

                var checking = accounts.First(a => a.Name == "Checking");

                Assert.IsNotNull(checking);
                Assert.AreEqual(10000, checking.Amount);

                var history = _sut.GetTransactionHistory();
                Assert.IsNotNull(history);
                Assert.IsEmpty(history);
            }

            [Test]
            public void Should_ThrowException_WhenTransferInputIsZero()
            {
                Assert.Throws<ArgumentException>(() => _sut.TransferFunds(0, "Checking", "Savings"), "Amount must be positive.");

                var accounts = _sut.GetCurrentBalance();

                var checking = accounts.First(a => a.Name == "Checking");

                Assert.IsNotNull(checking);
                Assert.AreEqual(10000, checking.Amount);

                var history = _sut.GetTransactionHistory();
                Assert.IsNotNull(history);
                Assert.IsEmpty(history);
            }

            [Test]
            public void Should_ThrowException_WhenTransferInputIsOverAccountBalance()
            {
                Assert.Throws<ArgumentException>(() => _sut.TransferFunds(500, "Savings", "Checking"), "Insufficient funds in account Savings. Current balance: $0.00");

                var accounts = _sut.GetCurrentBalance();

                var checking = accounts.First(a => a.Name == "Savings");

                Assert.IsNotNull(checking);
                Assert.AreEqual(0, checking.Amount);

                var history = _sut.GetTransactionHistory();
                Assert.IsNotNull(history);
                Assert.IsEmpty(history);
            }

            [Test]
            public void Should_ThrowException_WhenFromAccountIsntFound()
            {
                Assert.Throws<ArgumentException>(() => _sut.TransferFunds(500.45m, "Check", "Savings"), "Account Check does not exist.");

                var accounts = _sut.GetCurrentBalance();

                var checking = accounts.First(a => a.Name == "Checking");

                Assert.IsNotNull(checking);
                Assert.AreEqual(10000, checking.Amount);

                var history = _sut.GetTransactionHistory();
                Assert.IsNotNull(history);
                Assert.IsEmpty(history);
            }

            [Test]
            public void Should_ThrowException_WhenToAccountIsntFound()
            {
                Assert.Throws<ArgumentException>(() => _sut.TransferFunds(500.45m, "Checking", "Check"), "Account Check does not exist.");

                var accounts = _sut.GetCurrentBalance();

                var checking = accounts.First(a => a.Name == "Checking");

                Assert.IsNotNull(checking);
                Assert.AreEqual(10000, checking.Amount);

                var history = _sut.GetTransactionHistory();
                Assert.IsNotNull(history);
                Assert.IsEmpty(history);
            }
            #endregion
        }
    }
}
