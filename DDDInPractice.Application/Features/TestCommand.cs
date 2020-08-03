using System.Threading.Tasks;
using DDDInPractice.Persistence.Infrastructure.Requests.Executions;
using DDDInPractice.Persistence.Infrastructure.Requests.RequestContexts;
using Microsoft.EntityFrameworkCore;
using ResponsibilityChain;

namespace DDDInPractice.Persistence.Features
{
    public class TestCommand : Request<int>
    {
        public class Execute : IExecution<TestCommand, int>
        {
            private readonly AppDbContext _dbContext;

            public Execute(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }
            public async Task HandleAsync(TestCommand request)
            {
                var machine = await _dbContext.VendingMachines.ToListAsync();
                
            }
        }
    }
}