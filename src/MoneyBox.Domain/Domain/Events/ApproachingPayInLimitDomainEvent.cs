using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moneybox.Domain.Domain.Events
{
    public class ApproachingPayInLimitDomainEvent : INotification
    {
        public Account Account { get; private set; }

        public ApproachingPayInLimitDomainEvent(Account account)
        {
            this.Account = account;
        }
    }
}
