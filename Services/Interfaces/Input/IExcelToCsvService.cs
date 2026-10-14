using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Input
{
    public interface IExcelToCsvService
    {
        string ConvertToCsv(IFormFile file);
    }
}
