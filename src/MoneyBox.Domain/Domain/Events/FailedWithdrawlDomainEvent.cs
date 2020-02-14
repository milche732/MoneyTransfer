using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moneybox.Domain.Domain.Events
{
    public class FailedWithdrawlDomainEvent : INotification
    {
        public Account Account { get; }
        public decimal Amount { get; }

        public FailedWithdrawlDomainEvent(Account account,decimal  amount )
        {
            Account = account;
            Amount = amount;
        }
    }
}
