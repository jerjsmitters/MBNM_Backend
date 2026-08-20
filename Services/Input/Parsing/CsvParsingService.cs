using Common.Dto.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Input.Parsing
{
    internal class CsvParsingService : IParsingService
    {
        public InputTransactionsDto ParseInputData(string data)
        {
            return ParseCsv(data);
        }

        private InputTransactionsDto ParseCsv(string csvContent)
        {
            throw new NotImplementedException();
        }
    }
}
