using Moneybox.Domain;
using Moneybox.Domain.Domain;
using Moneybox.Domain.Domain.Events;
using System;
using System.Linq;
using Xunit;

namespace Moneybox.UnitTests
{
    public class AccountTransferMoneyTests
    {
        [Fact]
        public void CanWithdraw_LowBalance_False()
        {
            Account toAccount = new Account(Guid.NewGuid(), new User(Guid.NewGuid(), "alice", "alice@test.com"), 100m, 90m, 0m);
            Account fromAccount = new Account(Guid.NewGuid(), new User(Guid.NewGuid(), "bob", "bob@test.com"), 120m, 80m, 0m);

            Assert.False(fromAccount.CanWithdraw(150));
        }

        [Fact]
        public void CanWithdraw_HighBalance_True()
        {
            Account fromAccount = new Account(Guid.NewGuid(), new User(Guid.NewGuid(), "bob", "bob@test.com"), 120m, 80m, 0m);

            Assert.True(fromAccount.CanWithdraw(50m));
        }

        [Fact]
        public void TransferMoney_HighBalance_Suscessfull()
        {
            Account toAccount = new Account(Guid.NewGuid(), new User(Guid.NewGuid(), "alice", "alice@test.com"), 100m, 90m, 0m);
            Account fromAccount = new Account(Guid.NewGuid(), new User(Guid.NewGuid(), "bob", "bob@test.com"), 120m, 80m, 0m);

            toAccount.TransferMoney(fromAccount, 50);
            Assert.Equal(150m, toAccount.Balance);
            Assert.Equal(50m, toAccount.PaidIn);

            Assert.Equal(70m, fromAccount.Balance);
            Assert.Equal(30m, fromAccount.Withdrawn);
        }

        [Fact]
        public void TransferMoney_LowBalance_ThrowException()
        {
            Account toAccount = new Account(Guid.NewGuid(), new User(Guid.NewGuid(), "alice", "alice@test.com"), 100m, 90m, 0m);
            Account fromAccount = new Account(Guid.NewGuid(), new User(Guid.NewGuid(), "bob", "bob@test.com"), 120m, 80m, 0m);

            Assert.Throws<InsufficientFundsOperationException>(() => toAccount.TransferMoney(fromAccount, 150));
        }

        [Fact]
        public void TransferMoney_PayInExceeded_ThrowException()
        {
            Account toAccount = new Account(Guid.NewGuid(), new User(Guid.NewGuid(), "alice", "alice@test.com"), 100m, 90m, 0m);
            Account fromAccount = new Account(Guid.NewGuid(), new User(Guid.NewGuid(), "bob", "bob@test.com"), toAccount.PayInLimit+100m, toAccount.PayInLimit+100m, 0m);

            Assert.Throws<PayInLimitOperationException>(() => toAccount.TransferMoney(fromAccount, toAccount.PayInLimit+10));
        }

        [Fact]
        public void TransferMoney_LowBalanceSendNotification_Suscessfully()
        {
            Account toAccount = new Account(Guid.NewGuid(), new User(Guid.NewGuid(), "alice", "alice@test.com"), 100m, 90m, 0m);
            Account fromAccount = new Account(Guid.NewGuid(), new User(Guid.NewGuid(), "bob", "bob@test.com"), toAccount.NotificationThresold, 80m, 0m);

            
            toAccount.TransferMoney(fromAccount, 150);

            Assert.Collection(fromAccount.DomainEvents,
              x => Assert.Equal(typeof(AccountWithdrawnDomainEvent), x.GetType()),
              x => Assert.Equal(typeof(FundsLowDomainEvent), x.GetType()));
        }

        [Fact]
        public void TransferMoney_ApprocahingPaidInSendNotification_Suscessfully()
        {
            Account fromAccount = new Account(Guid.NewGuid(), new User(Guid.NewGuid(), "bob", "bob@test.com"), 80m, 80m, 0m);
            Account toAccount = new Account(Guid.NewGuid(), new User(Guid.NewGuid(), "alice", "alice@test.com"), 100m, 90m, fromAccount.PayInLimit - 10);


            toAccount.TransferMoney(fromAccount, 5);

            Assert.Collection(toAccount.DomainEvents,
               x => Assert.Equal(typeof(AccountPaidInDomainEvent), x.GetType()),
                  x => Assert.Equal(typeof(ApproachingPayInLimitDomainEvent), x.GetType()));
        }

    }
}
