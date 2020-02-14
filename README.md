In order to quick test the solution please RUN console app Moneybox.App.

DDD practices were implemented during the refactoring.
Entity.AddDomainEvent is used to push domain events. Actual execution of events should be triggered on entity Update. You can check simple implementation of IAccountRepository at Monebox.Infrastructure.AccountRepository.

MediatR library has been used, also Specification pattern was implemented, but can be changed to Decision Table.

There are 6 domain events ApproachingPayInLimitDomainEvent, FundsLowDomainEvent, FailedPayInDomainEvent,FailedWithdrawlDomainEvent, AccountPaidInDomainEvent, AccountWithdrawnDomainEvent.

I would use Decision Table pattern https://en.wikipedia.org/wiki/Decision_table to customize some business rules inside of Account entity (you can find my comments inside)


Thank you for your review.
