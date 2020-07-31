using ResponsibilityChain;

namespace DDDInPractice.Persistence.Infrastructure.Requests.Authorizations
{
    public class AuthorizationHandler<TRequest, TResponse> : Handler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
    }

    public interface IPreAuthorizationRule<TRequest, TResponse> : IPreHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
    }

    public interface IPostAuthorizationRule<TRequest, TResponse> : IPreHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
    }
}