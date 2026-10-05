using Common.Dto.Api;
using Common.Dto.Transactions;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.Transactions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionEvidenceController(ITransactionEvidenceService transactionEvidenceService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<TransactionEvidenceDto>> Post([FromBody] CreateEvidenceDto createEvidenceDto)
        {
            if ((createEvidenceDto?.Bytes?.Length ?? 0) == 0)
            {
                return BadRequest();
            }

            var created = await transactionEvidenceService.Create(createEvidenceDto);
            return Ok(created);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await transactionEvidenceService.Delete(id);
            return Ok();
        }
    }
}
