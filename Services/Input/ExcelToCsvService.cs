using Microsoft.AspNetCore.Http;
using Services.Interfaces.Input;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Input
{
    public class ExcelToCsvService : IExcelToCsvService
    {
        private const string TemporaryFile = "C:\\FileStore\\MBNM\\{0}.csv";


        public string ConvertToCsv(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            
            var workbook = new Workbook();
            workbook.LoadFromStream(stream);

            var worksheet = workbook.Worksheets[0];

            var tempFileName = string.Format(TemporaryFile, Guid.NewGuid().ToString());
            worksheet.SaveToFile(tempFileName, ",", Encoding.UTF8);

            var csvFile = new FileInfo(tempFileName);
            var csvText = csvFile.OpenText().ReadToEnd();
         
            return csvText;
        }
    }
}
