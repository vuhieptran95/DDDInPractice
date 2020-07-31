using System.Threading.Tasks;
using ResponsibilityChain;

namespace DDDInPractice.Persistence.Infrastructure.DomainEvents.EventExecutions
{
    public class EventExecutionBase<TEvent> : Handler<TEvent, Nothing>
        where TEvent : IRequest<Nothing>
    {
        private readonly IEventExecution<TEvent>[] _executions;

        public EventExecutionBase(IEventExecution<TEvent>[] executions)
        {
            _executions = executions;
        }

        public override async Task HandleAsync(TEvent domainEvent)
        {
            foreach (var execution in _executions)
            {
                await execution.HandleAsync(domainEvent);
            }
        }
    }

    public interface IEventExecution<TEvent> : IHandler<TEvent, Nothing> where TEvent : IRequest<Nothing>
    {
    }
}