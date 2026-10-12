using Common.Dto.Api;
using Common.Dto.Filters;
using Common.Dto.Output;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.Output;
using System.Runtime.InteropServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController(IExportService exportService) : ControllerBase
    {
        [HttpPost("generate")]
        public async Task<IActionResult> Export([FromBody] TransactionEntityFilter filters)
        {
            var file = await exportService.CreateExportFileForTransactions(filters);
            return PhysicalFile(file.FilePath, "text/csv");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExportFile(int id)
        {
            var exportFile = await exportService.GetExportFile(id);

            if (exportFile == null)
            {
                return NotFound();
            }

            return PhysicalFile(exportFile.FilePath, "text/csv");
        }

        [HttpGet]
        public async Task<ActionResult<List<ExportFileDto>>> GetExportFiles()
        {
            var files = await exportService.GetAllExportFiles();
            return Ok(files);
        }
    }
}
