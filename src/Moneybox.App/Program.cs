using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monebox.Infrastructure;
using Moneybox.App.DomainEventHandlers;
using Moneybox.Domain.DataAccess;
using Moneybox.Domain.Domain.Events;
using Moneybox.Domain.Domain.Services;
using Moneybox.Domain.Features;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moneybox.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
            .AddTransient<IAccountRepository, AccountRepository>()
            .AddTransient<INotificationService, NotificationService>()
            .AddTransient<ITransferMoney, TransferMoney>()
            .AddTransient<IWithdrawMoney, WithdrawMoney>()
            .AddLogging(opt => 
             {
                 opt.AddConsole();
             })
             .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information)
            .AddMediatR(typeof(ApproachingPayInLimitDomainEventHandler).Assembly)
            .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger<Program>>();
            logger.LogDebug("Starting application");

            IAccountRepository accountRepository = serviceProvider.GetService<IAccountRepository>();

            var fromAccount = accountRepository.GetAccountById(Guid.Parse("7511ca49-059e-4eae-9c8f-caf29bc61063"));
            var toAccount = accountRepository.GetAccountById(Guid.Parse("b3388bd0-de5d-40aa-8582-4571b4223d36"));
            Console.WriteLine("Send 10");
            Console.WriteLine("From: " + fromAccount.ToString());
            Console.WriteLine("To: "+toAccount.ToString());
            var transferMoney = serviceProvider.GetService<ITransferMoney>();

            transferMoney.Execute(fromAccount.Id, toAccount.Id, 10);
        }
    }
}