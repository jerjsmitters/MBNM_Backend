using Common.Dto.Input;
using Services.Common;
using Services.Input.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Input
{
    internal class InputService : IInputService
    {
        private IParsingService _parsingService;
        private TransactionService _transactionService;


        public void AddTransactions(string inputData)
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

            _transactionService.AddTransactions(parsedInputData);
        }
    }
}
