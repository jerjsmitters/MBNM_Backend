using Common.Dto.ChangeRequest;
using Common.Dto.Input;
using Services.Interfaces.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Input
{
    public class InputService(IParsingService parsingService, IChangeRequestService changeRequestService) : IInputService
    {
        public async Task<ChangeRequestDto> AddTransactions(string inputData)
        {
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
