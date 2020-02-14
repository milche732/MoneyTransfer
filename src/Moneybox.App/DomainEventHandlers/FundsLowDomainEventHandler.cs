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
    public class FundsLowDomainEventHandler : INotificationHandler<FundsLowDomainEvent>
    {
        private readonly INotificationService notificationService;
        private readonly ILogger<FundsLowDomainEventHandler> logger;

        public FundsLowDomainEventHandler(INotificationService notificationService, ILogger<FundsLowDomainEventHandler> logger)
        {
            this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            this.logger = logger;
        }

        public Task Handle(FundsLowDomainEvent notification, CancellationToken cancellationToken)
        {
            logger.LogDebug($"Funds Low Account. {notification.Account}");
            notificationService.NotifyFundsLow(notification.Account.User.Email);
            return Task.CompletedTask;
        }
    }
}
