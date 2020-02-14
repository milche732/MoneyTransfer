using Moneybox.Domain.DataAccess;
using Moneybox.Domain.Features;
using System;
using Xunit;

namespace Moneybox.IntegrationTests
{
    public class TransferMoneyTests
    {
        [Fact]
        public void TransferMoney_Succesfully()
        {
            Guid from = Guid.NewGuid();
            Guid to = Guid.NewGuid();


            var fromAccount = new Domain.Account(from, new Domain.User(Guid.NewGuid(), "alice", "alice@test.com"), 100m, 90m, 0m);
            var toAccount = new Domain.Account(to, new Domain.User(Guid.NewGuid(), "bob", "bob@test.com"), 120m, 80m, 0m);

            Moq.Mock<IAccountRepository> moq = new Moq.Mock<IAccountRepository>();

            moq.Setup(x => x.GetAccountById(from)).Returns(fromAccount);
            moq.Setup(x => x.GetAccountById(to)).Returns(toAccount);

            IAccountRepository repository = moq.Object;

            TransferMoney transferMoney = new TransferMoney(repository);

            bool result = transferMoney.Execute(from, to, 10);

            Assert.True(result);
            Assert.Equal(90m, fromAccount.Balance);
            Assert.Equal(130m, toAccount.Balance);

            moq.Verify(v => v.Update(fromAccount));
            moq.Verify(v => v.Update(toAccount));
        }

        [Fact]
        public void TransferMoney_Failed()
        {
            Guid from = Guid.NewGuid();
            Guid to = Guid.NewGuid();

            var fromAccount = new Domain.Account(from, new Domain.User(Guid.NewGuid(), "alice", "alice@test.com"), 100m, 90m, 0m);
            var toAccount = new Domain.Account(to, new Domain.User(Guid.NewGuid(), "bob", "bob@test.com"), 120m, 80m, 0m);

            Moq.Mock<IAccountRepository> moq = new Moq.Mock<IAccountRepository>();
            moq.Setup(x => x.GetAccountById(from)).Returns(fromAccount);
            moq.Setup(x => x.GetAccountById(to)).Returns(toAccount);

            IAccountRepository repository = moq.Object;
            TransferMoney transferMoney = new TransferMoney(repository);

            bool result = transferMoney.Execute(from, to, 130);

            Assert.False(result);
            Assert.Equal(100m, fromAccount.Balance);
            Assert.Equal(120m, toAccount.Balance);
        }

    }
}
