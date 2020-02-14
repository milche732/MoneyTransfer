using MediatR;
using Microsoft.Extensions.Logging;
using Moneybox.Domain.Domain.Events;
using Moneybox.Domain.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Moneybox.App.DomainEventHandlers
{
    public class AccountWithdrawnDomainEventHandler : INotificationHandler<AccountWithdrawnDomainEvent>
    {
        private readonly INotificationService notificationService;
        private readonly ILogger<AccountWithdrawnDomainEventHandler> logger;

        public AccountWithdrawnDomainEventHandler(INotificationService notificationService, ILogger<AccountWithdrawnDomainEventHandler> logger)
        {
            this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            this.logger = logger;
        }

        public Task Handle(AccountWithdrawnDomainEvent notification, CancellationToken cancellationToken)
        {
            logger.LogDebug($"Account withdrawn. {notification.Account}, Amount - {notification.Amount} ");
            return Task.CompletedTask;
        }
    }
}
