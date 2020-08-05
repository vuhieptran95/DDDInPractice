using System.Threading.Tasks;
using DDDInPractice.Persistence.Infrastructure;
using DDDInPractice.Persistence.Infrastructure.Requests.Executions;
using DDDInPractice.Persistence.Infrastructure.Requests.RequestContexts;
using ResponsibilityChain;

namespace DDDInPractice.Persistence.Features.VendingMachines.BuyFlow
{
    public class HandleSelectItemCommand : Request<Nothing>
    {
        public HandleSelectItemCommand(int machineId, string slotPosition)
        {
            MachineId = machineId;
            SlotPosition = slotPosition;
        }
        public int MachineId { get; }
        public string SlotPosition { get; }
        
        public class Handler : IExecution<HandleSelectItemCommand, Nothing>
        {
            private readonly IMediator _mediator;

            public Handler(IMediator mediator)
            {
                _mediator = mediator;
            }
            public async Task HandleAsync(HandleSelectItemCommand request)
            {
                var machine = await _mediator.SendAsync(new GetMachineCachingQuery(request.MachineId));
                
                machine.HandleSelectItems(request.SlotPosition);
                
            }
        }
    }
}