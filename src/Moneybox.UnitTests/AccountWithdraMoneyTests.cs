using Moneybox.Domain;
using Moneybox.Domain.Domain;
using Moneybox.Domain.Domain.Events;
using Moneybox.UnitTests.SeedWork;
using System;
using System.Linq;
using Xunit;

namespace Moneybox.UnitTests
{
    public class AccountWithdrawMoneyTests
    {
        [Fact]
        public void WithdrawMoney_HighBalance_Suscessfull()
        {
            Account fromAccount = new Account(Guid.NewGuid(), new User(Guid.NewGuid(), "bob", "bob@test.com"), 120m, 80m, 0m);

            fromAccount.WithdrawMoney(10m);
            Assert.Equal(110m, fromAccount.Balance);
            Assert.Equal(70m, fromAccount.Withdrawn);
        }

        [Fact]
        public void WithdrawMoney_LowBalance_ThrowException()
        {
            Account fromAccount = new Account(Guid.NewGuid(), new User(Guid.NewGuid(), "bob", "bob@test.com"), 120m, 80m, 0m);
            Assert.Throws<InsufficientFundsOperationException>(() => fromAccount.WithdrawMoney(150m));
        }

        [Fact]
        public void TransferMoney_LowBalanceSendNotification_Suscessfully()
        { 
            Account fromAccount = new Account(Guid.NewGuid(), new User(Guid.NewGuid(), "bob", "bob@test.com"), 0, 80m, 0m);
            fromAccount = new Account(Guid.NewGuid(), new User(Guid.NewGuid(), "bob", "bob@test.com"), fromAccount.NotificationThresold, 80m, 0m);
            
            fromAccount.WithdrawMoney(10);

            AggregateTest.AssertSingleDomainEvent<AccountWithdrawnDomainEvent>(fromAccount, x => {
                Assert.Equal(10m, x.Amount);
                Assert.Equal(fromAccount, x.Account);
            });

            AggregateTest.AssertSingleDomainEvent<FundsLowDomainEvent>(fromAccount, x => {
                Assert.Equal(fromAccount, x.Account);
            });
            /*
            Assert.Collection(fromAccount.DomainEvents, 
                x => Assert.Equal(typeof(AccountWithdrawnDomainEvent),x.GetType()),
                x => Assert.Equal(typeof(FundsLowDomainEvent), x.GetType()));*/
        }
    }
}
