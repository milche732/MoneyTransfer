using Microsoft.Extensions.Logging;
using Moneybox.Domain.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monebox.Infrastructure
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> logger;

        public NotificationService(ILogger<NotificationService> logger) {
            this.logger = logger;
        }
        public void NotifyApproachingPayInLimit(string emailAddress)
        {
            Console.WriteLine($"NOTIFICATION: {emailAddress}, you are approaching paid in limit.");
        }

        public void NotifyFundsLow(string emailAddress)
        {
            Console.WriteLine($"NOTIFICATION: {emailAddress}, your funds are low.");
        }
    }
}
