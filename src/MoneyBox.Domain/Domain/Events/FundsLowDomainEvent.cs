using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moneybox.Domain.Domain.Events
{
    public class FundsLowDomainEvent : INotification
    {
        public Account Account { get; private set; }

        public FundsLowDomainEvent(Account account)
        {
            Account = account;
        }
    }
}
