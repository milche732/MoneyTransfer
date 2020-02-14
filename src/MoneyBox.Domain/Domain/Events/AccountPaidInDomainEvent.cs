using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moneybox.Domain.Domain.Events
{
    /// <summary>
    /// Happens when account has been withdrawn
    /// </summary>
    public class AccountPaidInDomainEvent : INotification
    {
        public Account Account { get; }
        public decimal Amount { get; }

        public AccountPaidInDomainEvent(Account account, decimal amount)
        {
            Account = account;
            Amount = amount;
        }
    }
}
