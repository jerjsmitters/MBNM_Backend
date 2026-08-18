using Common.Dto.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Input.Parsing
{
    internal interface IParsingService
    {
        public InputTransactions ParseInputData(string data);
    }
}
