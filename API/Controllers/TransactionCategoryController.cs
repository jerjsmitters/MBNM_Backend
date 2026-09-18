using Common.Dto.Criteria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.Transactions;
using System.Net.WebSockets;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionCategoryController(ITransactionCategoryService transactionCategoryService) : ControllerBase
    {
        [HttpGet("{transactionCategoryId}")]
        public async Task<ActionResult<TransactionCategoryDto?>> Get([FromRoute] int transactionCategoryId)
        {
            var category = await transactionCategoryService.Get(transactionCategoryId);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionCategoryDto>>> GetAll()
        {
            var results = await transactionCategoryService.GetAll();
            return Ok(results);
        }

        [HttpPost]
        public async Task<ActionResult<List<TransactionCategoryDto>>> Add([FromBody] TransactionCategoryDto transactionCategory)
        {
            var createdEntity = await transactionCategoryService.Add(transactionCategory);
            return Ok(createdEntity);
        }

        [HttpPut]
        public async Task<ActionResult<List<TransactionCategoryDto>>> Update([FromBody] TransactionCategoryDto transactionCategory)
        {
            var createdEntity = await transactionCategoryService.Update(transactionCategory);
            return Ok(createdEntity);
        }

        [HttpDelete("{transactionCategoryId}")]
        public async Task<ActionResult> Delete([FromRoute] int transactionCategoryId)
        {
            await transactionCategoryService.Delete(transactionCategoryId);
            return Ok();
        }
    }
}
