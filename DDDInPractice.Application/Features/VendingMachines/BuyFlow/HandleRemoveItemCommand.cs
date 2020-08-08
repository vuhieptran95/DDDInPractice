using System.Threading.Tasks;
using DDDInPractice.Persistence.Infrastructure;
using DDDInPractice.Persistence.Infrastructure.Requests.Executions;
using DDDInPractice.Persistence.Infrastructure.Requests.RequestContexts;
using ResponsibilityChain;

namespace DDDInPractice.Persistence.Features.VendingMachines.BuyFlow
{
    public class CommitTransationCommand : Request<Nothing>
    {
        public CommitTransationCommand(int machineId)
        {
            MachineId = machineId;
        }

        public int MachineId { get; private set; }
        public class Handler: IExecution<CommitTransationCommand, Nothing>
        {
            private readonly AppDbContext _dbContext;
            private readonly IMediator _mediator;

            public Handler(AppDbContext dbContext, IMediator mediator)
            {
                _dbContext = dbContext;
                _mediator = mediator;
            }
            public async Task HandleAsync(CommitTransationCommand request)
            {
                var machine = await _mediator.SendAsync(new GetMachineCachingQuery(request.MachineId));

                if (machine.IsAbleToReturnMoney())
                {
                    machine.MachineDropItems(out var isDropItemsSuccess);
                    
                    machine.CalculateReturnMoney();
                    
                    machine.MachineDropChange(out var isDropChangeSuccess);
                }
            }
        }
    }
    public class HandleRemoveItemCommand : Request<Nothing>
    {
        public HandleRemoveItemCommand(int machineId)
        {
            MachineId = machineId;
        }
        public int MachineId { get; }
        
        public class Handler: IExecution<HandleRemoveItemCommand, Nothing>
        {
            private readonly IMediator _mediator;

            public Handler(IMediator mediator)
            {
                _mediator = mediator;
            }
            public async Task HandleAsync(HandleRemoveItemCommand request)
            {
                var machine = await _mediator.SendAsync(new GetMachineCachingQuery(request.MachineId));
                machine.RemoveLastSelectedSlot();
                
            }
        }
    }
}