using MediatR;
using Moneybox.Domain.SeedWork;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Moneybox.UnitTests.SeedWork
{
    public static class AggregateTest
    {
        public static void AssertSingleDomainEventOfType<TEvent>(Entity aggregate)
            where TEvent : INotification
        {
            var uncommittedEvents = GetDomainEventsOf<TEvent>(aggregate);

            Assert.Single(uncommittedEvents);
            Assert.IsType<TEvent>(uncommittedEvents.First());
        }

        public static void AssertSingleDomainEvent<TEvent>(Entity aggregate, Action<TEvent> assertions)
            where TEvent : INotification
        {
            AssertSingleDomainEventOfType<TEvent>(aggregate);
            assertions(GetDomainEventsOf<TEvent>(aggregate).Single());
        }

        public static void ClearDomainEvents(Entity aggregate)
        {
            (aggregate).ClearDomainEvents();
        }

        public static IEnumerable<TEvent> GetDomainEventsOf<TEvent>(Entity aggregate)
        {
            return (aggregate).DomainEvents.Where(x=>x.GetType() == typeof(TEvent)).Select(x=>(TEvent)x);
        }
    }
}
