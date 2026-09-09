using Common.Dto.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Input
{
    public interface IParsingService
    {
        public InputTransactionsDto ParseInputData(string data);
    }
}
