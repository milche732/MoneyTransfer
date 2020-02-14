using MediatR;
using Moneybox.Domain;
using Moneybox.Domain.DataAccess;
using System;
using System.Linq;

namespace Monebox.Infrastructure
{
    public class AccountRepository : IAccountRepository
    {
        private Account[] accounts = new Account[]
        {
            new Account(Guid.Parse("7511ca49-059e-4eae-9c8f-caf29bc61063"), new User(Guid.NewGuid(), "alice", "alice@test.com"), 100m, 90m, 0m),
            new Account(Guid.Parse("b3388bd0-de5d-40aa-8582-4571b4223d36"), new User(Guid.NewGuid(), "bob", "bob@test.com"), 120m, 80m, 3990m)
        };

        private readonly IMediator mediator;

        public AccountRepository(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public Account GetAccountById(Guid accountId)
        {
            return accounts.Where(x => x.Id == accountId).FirstOrDefault();
        }

        public void Update(Account account)
        {
            var events = account.DomainEvents?.ToList();

            if (events != null)
            {
                account.ClearDomainEvents();

                foreach (var @event in events)
                {
                    mediator.Publish(@event);
                }
            }
            //todo: save account here
        }
    }
}
