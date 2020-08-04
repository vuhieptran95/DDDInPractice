using System.Threading.Tasks;
using DDDInPractice.Domains;
using DDDInPractice.Persistence.Infrastructure;
using DDDInPractice.Persistence.Infrastructure.Requests.Executions;
using DDDInPractice.Persistence.Infrastructure.Requests.RequestContexts;
using ResponsibilityChain;

namespace DDDInPractice.Persistence.Features.VendingMachines.BuyFlow
{
    public class TakeUserMoneyCommand : Request<Nothing>
    {
        public TakeUserMoneyCommand(int machineId, Money money)
        {
            MachineId = machineId;
            Money = money;
        }

        public int MachineId { get; }
        public Money Money { get; }

        public class Handler : IExecution<TakeUserMoneyCommand, Nothing>
        {
            private readonly IMediator _mediator;

            public Handler(IMediator mediator)
            {
                _mediator = mediator;
            }

            public async Task HandleAsync(TakeUserMoneyCommand request)
            {
                var machine = await _mediator.SendAsync(new GetMachineCachingQuery(request.MachineId));

                machine.TakeMoney(request.Money);

            }
        }
    }
}