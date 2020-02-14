using System;

namespace Moneybox.Domain.Features
{
    public interface IWithdrawMoney
    {
        bool Execute(Guid fromAccountId, decimal amount);
    }
}