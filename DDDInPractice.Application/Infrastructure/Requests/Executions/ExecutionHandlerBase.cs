using System.Threading.Tasks;
using ResponsibilityChain;

namespace DDDInPractice.Persistence.Infrastructure.Requests.Executions
{
    public class ExecutionHandlerBase<TRequest, TResponse> : Handler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IExecution<TRequest, TResponse>[] _executions;

        public ExecutionHandlerBase(IExecution<TRequest, TResponse>[] executions)
        {
            _executions = executions;
        }
        public override async Task HandleAsync(TRequest request)
        {
            foreach (var execution in _executions)
            {
                await execution.HandleAsync(request);
            }
        }
    }

    public interface IExecution<TRequest, TResponse> : IHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        
    }

}