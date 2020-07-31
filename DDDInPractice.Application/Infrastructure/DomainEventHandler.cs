using DDDInPractice.Domains;
using DDDInPractice.Persistence.Infrastructure.DomainEvents.EventExecutions;
using ResponsibilityChain;

namespace DDDInPractice.Persistence.Infrastructure
{
    public class DomainEventHandler<TEvent> : Handler<TEvent, Nothing> where TEvent : DomainEvent
    {
        public DomainEventHandler(EventExecutionBase<TEvent> eventExecutionBase)
        {
            AddHandler(eventExecutionBase);
        }
    }
}