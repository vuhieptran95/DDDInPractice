using System.Threading.Tasks;
using DDDInPractice.Persistence.Infrastructure.Requests.Executions;
using DDDInPractice.Persistence.Infrastructure.Requests.RequestContexts;
using ResponsibilityChain;

namespace DDDInPractice.Persistence.Features
{
    public class TestCommand : Request<int>
    {
        public class Execute : IExecution<TestCommand, int>
        {
            public Task HandleAsync(TestCommand request)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}