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
    public class AccountPaidInDomainEventHandler : INotificationHandler<AccountPaidInDomainEvent>
    {
        private readonly INotificationService notificationService;
        private readonly ILogger<AccountPaidInDomainEventHandler> logger;

        public AccountPaidInDomainEventHandler(INotificationService notificationService, ILogger<AccountPaidInDomainEventHandler> logger)
        {
            this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            this.logger = logger;
        }

        public Task Handle(AccountPaidInDomainEvent notification, CancellationToken cancellationToken)
        {
            logger.LogDebug($"Account Paid In. {notification.Account}, Amount - {notification.Amount}");
            return Task.CompletedTask;
        }
    }
}
