using Common.Dto.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Input.Parsing
{
    internal class CsvParsingService : IParsingService
    {
        public InputTransactions ParseInputData(string data)
        {
            return ParseCsv(data);
        }

        private InputTransactions ParseCsv(string csvContent)
        {
            throw new NotImplementedException();
        }
    }
}
