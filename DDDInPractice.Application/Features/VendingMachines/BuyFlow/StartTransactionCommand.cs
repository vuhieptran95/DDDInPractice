using System.Threading.Tasks;
using DDDInPractice.Persistence.Infrastructure;
using DDDInPractice.Persistence.Infrastructure.Requests.Executions;
using DDDInPractice.Persistence.Infrastructure.Requests.RequestContexts;
using ResponsibilityChain;

namespace DDDInPractice.Persistence.Features.VendingMachines.BuyFlow
{
    public class GetMachinesQuery : Request<GetMachinesQuery.Dto>
    {
        public class Dto
        {
            
        }
        
        public class Machine
        {
            
        }
        
        public class Slot
        {
            
        }
    }
    public class StartTransactionCommand : Request<Nothing>
    {
        public StartTransactionCommand(int machineId)
        {
            MachineId = machineId;
        }

        public int MachineId { get; }
        
        public class Handler : IExecution<StartTransactionCommand, Nothing>
        {
            private readonly IMediator _mediator;

            public Handler(IMediator mediator)
            {
                _mediator = mediator;
            }
            public async Task HandleAsync(StartTransactionCommand request)
            {
                var machine = await _mediator.SendAsync(new GetMachineCachingQuery(request.MachineId));

                machine.StartTransaction();
                
            }
        }
    }
}