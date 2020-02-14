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
    public class ApproachingPayInLimitDomainEventHandler : INotificationHandler<ApproachingPayInLimitDomainEvent>
    {
        private readonly INotificationService notificationService;
        private readonly ILogger<ApproachingPayInLimitDomainEventHandler> logger;

        public ApproachingPayInLimitDomainEventHandler(INotificationService notificationService, ILogger<ApproachingPayInLimitDomainEventHandler> logger)
        {
            this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            this.logger = logger;
        }

        public Task Handle(ApproachingPayInLimitDomainEvent notification, CancellationToken cancellationToken)
        {
            logger.LogTrace($"Approaching PayIn Limit. {notification.Account}");
            notificationService.NotifyApproachingPayInLimit(notification.Account.User.Email);
            return Task.CompletedTask;
        }
    }
}
