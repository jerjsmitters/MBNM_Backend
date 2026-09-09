using Common.Dto.Input;
using Services.Interfaces.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Input
{
    public class InputService : IInputService
    {
        private IParsingService _parsingService;
        private ChangeRequestService _changeRequestService;


        public async Task AddTransactions(string inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData))
            {
                return;
            }

            var parsedInputData = _parsingService.ParseInputData(inputData);

            if (parsedInputData.Transactions.Count == 0)
            {
                return;
            }

            await _changeRequestService.GenerateChangeRequestFromInputTransactions(parsedInputData);
        }
    }
}
