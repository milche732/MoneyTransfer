using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moneybox.Domain.Domain.Events
{
    public class FailedPayInDomainEvent : INotification
    {
        public Account Account { get; }
        public decimal Amount { get; }

        public FailedPayInDomainEvent(Account account,decimal  amount )
        {
            Account = account;
            Amount = amount;
        }
    }
}
