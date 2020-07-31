using DDDInPractice.Persistence.Infrastructure.Requests.Authorizations;
using DDDInPractice.Persistence.Infrastructure.Requests.Caching;
using DDDInPractice.Persistence.Infrastructure.Requests.Executions;
using DDDInPractice.Persistence.Infrastructure.Requests.Logging;
using DDDInPractice.Persistence.Infrastructure.Requests.RequestContexts;
using DDDInPractice.Persistence.Infrastructure.Requests.RequestEvents;
using DDDInPractice.Persistence.Infrastructure.Requests.Validations;
using ResponsibilityChain;

namespace DDDInPractice.Persistence.Infrastructure
{
    public class RequestHandler<TRequest, TResponse> : Handler<TRequest, TResponse> where TRequest: Request<TResponse>
    {
        public RequestHandler(
            LoggingHandler<TRequest, TResponse> loggingHandler,
            AuthorizationConfigBase<TRequest, TResponse> authorizationConfigBase,
            AuthorizationHandler<TRequest, TResponse> authorizationHandler,
            ValidationHandlerBase<TRequest, TResponse> validationHandlerBase,
            RequestEventHandler<TRequest, TResponse> requestEventHandler,
            CacheHandler<TRequest, TResponse> cacheHandler,
            ExecutionHandlerBase<TRequest, TResponse> executionHandlerBase)
        {
            AddHandler(loggingHandler);
            AddHandler(authorizationConfigBase);
            AddHandler(authorizationHandler);
            AddHandler(validationHandlerBase);
            AddHandler(requestEventHandler);
            AddHandler(cacheHandler);
            AddHandler(executionHandlerBase);
        }
    }
}