using Common.Dto.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.Transactions;
using System.Net.WebSockets;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionPropertyController(ITransactionPropertyService transactionPropertyService) : ControllerBase
    {
        [HttpGet("{transactionPropertyId}")]
        public async Task<ActionResult<TransactionPropertyDto?>> Get([FromRoute] int transactionPropertyId)
        {
            var property = await transactionPropertyService.Get(transactionPropertyId);
            if (property == null)
            {
                return NotFound();
            }

            return Ok(property);
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionPropertyDto>>> GetAll()
        {
            var results = await transactionPropertyService.GetAll();
            return Ok(results);
        }

        [HttpPost]
        public async Task<ActionResult<List<TransactionPropertyDto>>> Add([FromBody] TransactionPropertyDto transactionProperty)
        {
            var createdEntity = await transactionPropertyService.Add(transactionProperty);
            return Ok(createdEntity);
        }

        [HttpPut]
        public async Task<ActionResult<List<TransactionPropertyDto>>> Update([FromBody] TransactionPropertyDto transactionProperty)
        {
            var createdEntity = await transactionPropertyService.Update(transactionProperty);
            return Ok(createdEntity);
        }

        [HttpDelete("/{transactionPropertyId}")]
        public async Task<ActionResult> Delete([FromRoute] int transactionPropertyId)
        {
            await transactionPropertyService.Delete(transactionPropertyId);
            return Ok();
        }
    }
}
