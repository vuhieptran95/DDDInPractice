using System;
using System.Threading.Tasks;
using Autofac;
using DDDInPractice.Domains;
using DDDInPractice.Persistence.Infrastructure.DomainEvents;
using DDDInPractice.Persistence.Infrastructure.Requests;
using ResponsibilityChain;

namespace DDDInPractice.Persistence.Infrastructure
{
    public interface IMediator
    {
        Task DispatchAsync(DomainEvent domainEvent);
        Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request);
    }

    public class Mediator: IMediator
    {
        private readonly ILifetimeScope _scope;

        public Mediator(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public async Task DispatchAsync(DomainEvent domainEvent)
        {
            var handlerType = typeof(DomainEventHandler<>).MakeGenericType(domainEvent.GetType());

            var eventHandler = _scope.Resolve(handlerType);

            await (Task) eventHandler.GetType().GetMethod("HandleAsync").Invoke(eventHandler, new[] {domainEvent});
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            var requestType = request.GetType();
            
            var handlerType = typeof(RequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
            
            var requestHandler = _scope.Resolve(handlerType);

            await (Task) requestHandler.GetType().GetMethod("HandleAsync").Invoke(requestHandler, new[] {request});

            return request.Response;
        }
    }
}