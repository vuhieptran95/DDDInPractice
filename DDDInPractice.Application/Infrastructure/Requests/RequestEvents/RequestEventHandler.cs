using ResponsibilityChain;

namespace DDDInPractice.Persistence.Infrastructure.Requests.RequestEvents
{
    public class RequestEventHandler<TRequest, TResponse> : Handler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public RequestEventHandler(IPreRequestEvent<TRequest, TResponse>[] preEvents, IPostRequestEvent<TRequest, TResponse>[] postEvents)
        {
            foreach (var preEvent in preEvents)
            {
                AddHandler(preEvent);
            }

            foreach (var postEvent in postEvents)
            {
                AddHandler(postEvent);
            }
        }
    }

    public interface IPreRequestEvent<TRequest, TResponse> : IPreHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
    }
    
    public interface IPostRequestEvent<TRequest, TResponse> : IPostHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
    }
}