using System;

namespace Moneybox.Domain.Features
{
    public interface ITransferMoney
    {
        bool Execute(Guid fromAccountId, Guid toAccountId, decimal amount);
    }
}