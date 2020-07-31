using ResponsibilityChain;

namespace DDDInPractice.Persistence.Infrastructure.Requests.Validations
{
    public class ValidationHandlerBase<TRequest, TResponse> : Handler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public ValidationHandlerBase(IPreValidation<TRequest, TResponse>[] preValidations,
            IPostValidation<TRequest, TResponse>[] postValidations)
        {
            foreach (var preValidation in preValidations)
            {
                AddHandler(preValidation);
            }

            foreach (var postValidation in postValidations)
            {
                AddHandler(postValidation);
            }
        }
    }

    public interface IPreValidation<TRequest, TResponse> : IPreHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
    }

    public interface IPostValidation<TRequest, TResponse> : IPostHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
    }
}