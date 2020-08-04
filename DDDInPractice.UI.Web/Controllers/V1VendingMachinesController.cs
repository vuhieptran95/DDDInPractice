using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDInPractice.Domains;
using DDDInPractice.Persistence;
using DDDInPractice.Persistence.Features;
using DDDInPractice.Persistence.Features.VendingMachines;
using DDDInPractice.Persistence.Features.VendingMachines.BuyFlow;
using DDDInPractice.Persistence.Infrastructure;
using DDDInPractice.Persistence.Infrastructure.Requests.RequestContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DDDInPractice.UI.Web.Controllers
{
    [ApiController]
    [Route("api/v1/vending-machines")]
    public partial class V1VendingMachinesController : ControllerBase
    {
        private readonly ILogger<V1VendingMachinesController> _logger;
        private readonly IMediator _mediator;

        public V1VendingMachinesController(ILogger<V1VendingMachinesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        // [HttpGet]
        // public async Task<ActionResult> GetMachines()
        // {
        //     var dto = await _mediator.SendAsync(new GetMachinesQuery());
        // }

        [HttpGet("{machineId}")]
        public async Task<ActionResult> GetMachine([FromRoute]int machineId)
        {
            var dto = await _mediator.SendAsync(new GetMachineCachingQuery(machineId));

            return Ok(dto);
        }

        [HttpPost("{machineId}/transactions")]
        public async Task<ActionResult> StartTransaction([FromRoute]int machineId)
        {
            var dto = await _mediator.SendAsync(new StartTransactionCommand(machineId));
            
            return Ok();
        }

        [HttpPut("{machineId}/transactions/money")]
        public async Task<ActionResult> TakeUserMoney([FromRoute]int machineId, [FromBody]MoneyViewModel money)
        {
            var dto = await _mediator.SendAsync(new TakeUserMoneyCommand(machineId, money.CreateMoney()));
            return Ok();
        }

        [HttpPut("{machineId}/transactions/items/{position}")]
        public async Task<ActionResult> HandleSelectItem([FromRoute]int machineId, [FromRoute]string position)
        {
            var dto = await _mediator.SendAsync(new HandleSelectItemCommand(machineId, position));
            return Ok();
        }
        
        [HttpDelete("{machineId}/transactions/items")]
        public async Task<ActionResult> HandleRemoveLastItem([FromRoute]int machineId)
        {
            var dto = await _mediator.SendAsync(new HandleRemoveItemCommand(machineId));
            return Ok();
        }
        
        [HttpPut("{machineId}/transactions")]
        public async Task<ActionResult> HandleBuy()
        {
            return Ok();
        }
    }
}