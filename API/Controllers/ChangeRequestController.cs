using Common.Dto.Api;
using Common.Dto.ChangeRequest;
using Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.Input;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangeRequestController(IChangeRequestService changeRequestService) : ControllerBase
    {
        [HttpPost("apply")]
        public async Task<ActionResult<List<ChangeRequestCommitResultDto>>> ApplyChangeRequest(ApplyChangeRequestDto applyChangeRequest)
        {
            var nothingToDo = ListHelpers.IsNullOrEmpty(applyChangeRequest?.CreateLines) 
                && ListHelpers.IsNullOrEmpty(applyChangeRequest?.UpdateLines) 
                && ListHelpers.IsNullOrEmpty(applyChangeRequest?.DeleteLines);

            if (nothingToDo) 
            {
                return BadRequest();
            }

            var results = await changeRequestService.ApplyChangeRequest(applyChangeRequest);

            return Ok(results);
        }
    }
}
