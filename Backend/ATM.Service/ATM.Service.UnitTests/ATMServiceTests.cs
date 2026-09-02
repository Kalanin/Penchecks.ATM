using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ATM.Service.Models;
using ATM.Service.Services;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ATM.Service.UnitTests
{
    public class ATMServiceTests
    {

        [TestFixture]
        public class RegisterTests
        {
            private ATMService _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new ATMService();
            }

            [Test]
            public void ShouldSetUpBasicAccount()
            {

                var accounts = _sut.GetCurrentBalance();

                Assert.IsNotNull(accounts);
                Assert.AreEqual(2, accounts.Count);
                Assert.AreEqual(0, accounts[0].Amount);
                Assert.AreEqual(0, accounts[1].Amount);
            }

            [Test]
            public void ShouldSetPresetDataAndWipeHistory()
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
    }
}
