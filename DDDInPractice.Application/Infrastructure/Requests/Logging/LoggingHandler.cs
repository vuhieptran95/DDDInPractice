using System;
using System.Threading.Tasks;
using DDDInPractice.Persistence.Infrastructure.Requests.RequestContexts;
using ResponsibilityChain;
using Serilog;

namespace DDDInPractice.Persistence.Infrastructure.Requests.Logging
{
    public class LoggingHandler<TRequest, TResponse> : Handler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;
        private readonly RequestContext _requestContext;

        public LoggingHandler(ILogger logger, RequestContext requestContext)
        {
            _logger = logger;
            _requestContext = requestContext;
        }

        public override async Task HandleAsync(TRequest request)
        {
            try
            {
                if (request is IRequiredRequestContext req)
                {
                    req.RequestContext = _requestContext;
                }

                _logger.Information(
                    $"Logging request {request.GetType()} " + "{@request}." + "User: " + "{@username}.", request,
                    _requestContext.Username);
                await base.HandleAsync(request);
                _logger.Information(
                    $"Logging response: {request.Response?.GetType()} " + "{@response}." + "User: " + "{@username}.",
                    request.Response, _requestContext.Username);
            }
            catch (Exception e)
            {
                _logger.Error(
                    $"Error occurred for request {request.GetType()} - " + "User: " + "{@username}." +
                    " Exception: {@exception}", _requestContext.Username, e);

                throw;
            }
        }
    }
}