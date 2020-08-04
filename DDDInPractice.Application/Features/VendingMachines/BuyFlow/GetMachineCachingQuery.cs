using System;
using System.Threading.Tasks;
using DDDInPractice.Domains;
using DDDInPractice.Persistence.Infrastructure.Requests.Caching;
using DDDInPractice.Persistence.Infrastructure.Requests.Executions;
using DDDInPractice.Persistence.Infrastructure.Requests.RequestContexts;
using Microsoft.EntityFrameworkCore;

namespace DDDInPractice.Persistence.Features.VendingMachines.BuyFlow
{
    public class GetMachineCachingQuery : Request<VendingMachine>
    {
        public GetMachineCachingQuery(int machineId)
        {
            MachineId = machineId;
        }

        public int MachineId { get; }

        public class CacheConfig : ICacheConfig<GetMachineCachingQuery>
        {
            public bool IsCacheEnabled { get; } = true;
            public DateTimeOffset CacheDateTimeOffset { get; } = DateTimeOffset.Now.AddDays(1);

            public string GetCacheKey(GetMachineCachingQuery request)
            {
                return nameof(GetMachineCachingQuery) + "/" + request.MachineId;
            }
        }

        internal class Handler : IExecution<GetMachineCachingQuery, VendingMachine>
        {
            private readonly AppDbContext _dbContext;

            public Handler(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task HandleAsync(GetMachineCachingQuery request)
            {
                var machine = await _dbContext.VendingMachines
                    .Include(m => m.Slots)
                    .FirstOrDefaultAsync(m => m.Id == request.MachineId);

                if (machine == null)
                {
                    throw new Exception("Machine does not exist. Machine Id: " + request.MachineId);
                }

                request.Response = machine;
            }
        }
    }
}