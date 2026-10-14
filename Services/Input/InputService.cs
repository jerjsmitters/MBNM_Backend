using Common.Dto.ChangeRequest;
using Common.Dto.Input;
using Microsoft.AspNetCore.Http;
using Services.Interfaces.Input;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Input
{
    public class InputService(IExcelToCsvService excelToCsvService, IParsingService parsingService, IChangeRequestService changeRequestService) : IInputService
    {
        public async Task<ChangeRequestDto> AddTransactions(IFormFile file)
        {
            var inputData = excelToCsvService.ConvertToCsv(file);

            if (string.IsNullOrWhiteSpace(inputData))
            {
                throw new ArgumentNullException(nameof(inputData));
            }

            var parsedInputData = parsingService.ParseInputData(inputData);

            if (parsedInputData.Transactions.Count == 0)
            {
                return new();
            }

            var changeRequests = await changeRequestService.GenerateChangeRequestFromInputTransactions(parsedInputData);
            return changeRequests;
        }
    }
}
