using Common.Dto.Api;
using Common.Dto.Criteria;
using Common.Dto.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.Transactions;
using System.Net.WebSockets;
using System.Runtime.InteropServices;

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

        //[HttpGet]
        //public async Task<ActionResult<List<TransactionDto>>> GetAll()
        //{
        //    var results = await transactionService.GetAll();
        //    return Ok(results);
        //}

        [HttpGet]
        public async Task<ActionResult<List<TransactionDto>>> GetAllFiltered(
            [FromQuery] int pageNumber,
            [FromQuery] int pageLength,
            [FromQuery] [Optional] List<int>? categoryIds,
            [FromQuery] [Optional] List<int>? propertyIds,
            [FromQuery] [Optional] string? searchText,
            [FromQuery] [Optional] DateOnly? startDate,
            [FromQuery] [Optional] DateOnly? endDate
            )
        {
            var transactionFilter = new TransactionEntityFilter(pageNumber, pageLength, categoryIds, propertyIds, searchText, startDate, endDate);
            var results = await transactionService.GetAllFiltered(transactionFilter);
            Response.Headers["Total-Results"] = results.TotalResults.ToString();

            return Ok(results.Transactions);
        }

        [HttpPut]
        public async Task<ActionResult<List<TransactionDto>>> Update([FromBody] TransactionDto transaction)
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
