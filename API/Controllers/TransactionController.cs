using Common.Dto.Criteria;
using Common.Dto.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.Transactions;
using System.Net.WebSockets;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController(ITransactionService transactionService) : ControllerBase
    {

        [HttpGet("{transactionId}")]
        public async Task<ActionResult<TransactionDto>> Get([FromRoute] int transactionId)
        {
            var property = await transactionService.Get(transactionId);
            if (property == null)
            {
                return NotFound();
            }

            return Ok(property);
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionDto>>> GetAll()
        {
            var results = await transactionService.GetAll();
            return Ok(results);
        }

        [HttpPut]
        public async Task<ActionResult<List<TransactionDto>>> GetAll([FromBody] TransactionDto transaction)
        {
            var results = await transactionService.Update(transaction);
            return Ok(results);
        }


        [HttpDelete("{transactionId}")]
        public async Task<ActionResult> Delete([FromRoute] int transactionId)
        {
            await transactionService.Delete(transactionId);
            return Ok();
        }
    }
}
