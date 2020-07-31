using System;
using System.Threading.Tasks;
using Autofac;
using DDDInPractice.Domains;
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
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task DispatchAsync(DomainEvent domainEvent)
        {
            var handlerType = typeof(DomainEventHandler<>).MakeGenericType(domainEvent.GetType());

            var eventHandler = _serviceProvider.GetService(handlerType);

            await (Task) eventHandler.GetType().GetMethod("HandleAsync").Invoke(eventHandler, new[] {domainEvent});
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            var requestType = request.GetType();
            
            var handlerType = typeof(RequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
            
            var requestHandler = _serviceProvider.GetService(handlerType);

            await (Task) requestHandler.GetType().GetMethod("HandleAsync").Invoke(requestHandler, new[] {request});

            return request.Response;
        }
    }
}