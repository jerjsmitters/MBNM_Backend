using Common.Dto.Api;
using Common.Dto.ChangeRequest;
using Common.Dto.Transactions;
using Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using Services.Interfaces.Input;
using Services.Transactions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangeRequestController(IChangeRequestService changeRequestService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<ChangeRequestDto?>> Get(int id)
        {
            var changeRequest = await changeRequestService.GetForApproval(id);

            return Ok(changeRequest);
        }

        [HttpGet("")]
        public async Task<ActionResult<List<ChangeRequestDto>>> GetAll()
        {
            var changeRequests = await changeRequestService.GetAll();

            return Ok(changeRequests);
        }

        [HttpPost("commit")]
        public async Task<ActionResult<List<ChangeRequestCommitResultDto>>> ApplyChangeRequest(CommitChangeRequestDto commitChangeRequestDto)
        {
            var nothingToDo = ListHelpers.IsNullOrEmpty(commitChangeRequestDto?.CreateLines) 
                && ListHelpers.IsNullOrEmpty(commitChangeRequestDto?.DeleteLines);

            if (nothingToDo) 
            {
                return BadRequest();
            }

            var results = await changeRequestService.ApplyChangeRequest(commitChangeRequestDto);

            return Ok(results);
        }
    }
}
