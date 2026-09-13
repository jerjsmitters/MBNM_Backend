using API.Dto;
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
        [Route("")]
        public async Task<ActionResult<ChangeRequestDto>> InputCsv([FromBody] InputCsvDto inputCsv)
        {
            if (inputCsv == null || string.IsNullOrWhiteSpace(inputCsv.CsvLines)) 
            {
                return BadRequest();
            }

            try
            {
                var results = await inputService.AddTransactions(inputCsv.CsvLines);

                return Ok(results);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            
        }
    }
}
