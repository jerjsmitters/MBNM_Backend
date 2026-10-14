using Common.Dto.Api;
using Common.Dto.ChangeRequest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.Input;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InputController (IInputService inputService) : ControllerBase
    {
        [HttpPost]
        [Route("input-csv")]
        public async Task<ActionResult<ChangeRequestDto>> InputCsv([FromForm]IFormFile file)
        {
            try
            {
                var results = await inputService.AddTransactions(file);

                return Ok(results);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            
        }
    }
}
