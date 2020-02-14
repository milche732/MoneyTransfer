using Moneybox.Domain.DataAccess;
using Moneybox.Domain.Features;
using System;
using Xunit;

namespace Moneybox.IntegrationTests
{
    public class WithdrawMoneyTests
    {
        [Fact]
        public void WithdrawMoney_Succesfully()
        {
            Guid from = Guid.NewGuid();
            var fromAccount = new Domain.Account(from, new Domain.User(Guid.NewGuid(), "bob", "bob@test.com"), 120m, 80m, 0m);
            Moq.Mock<IAccountRepository> moq = new Moq.Mock<IAccountRepository>();
            moq.Setup(x => x.GetAccountById(from)).Returns(fromAccount);

            IAccountRepository repository = moq.Object;
            WithdrawMoney withdrawMoney = new WithdrawMoney(repository);

            bool result = withdrawMoney.Execute(from, 10);

            Assert.True(result);
            Assert.Equal(110m, fromAccount.Balance);
            moq.Verify(v => v.Update(fromAccount));
        }

        [Fact]
        public void WithdrawMoney_Failed()
        {
            Guid from = Guid.NewGuid();
            var fromAccount = new Domain.Account(from, new Domain.User(Guid.NewGuid(), "bob", "bob@test.com"), 120m, 80m, 0m);
            Moq.Mock<IAccountRepository> moq = new Moq.Mock<IAccountRepository>();
            moq.Setup(x => x.GetAccountById(from)).Returns(fromAccount);
            moq.Setup(x => x.Update(fromAccount)).Throws(new Exception("Shouldn't be called."));

            IAccountRepository repository = moq.Object;
            WithdrawMoney withdrawMoney = new WithdrawMoney(repository);

            bool result = withdrawMoney.Execute(from, 130m);

            Assert.False(result);
            Assert.Equal(120m, fromAccount.Balance);
        }

    }
}
