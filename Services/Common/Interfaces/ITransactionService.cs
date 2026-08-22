using Common.Dto;
using Common.Dto.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common.Interfaces
{
    internal interface ITransactionService
    {
        ChangeRequestDto AddTransactions(InputTransactionsDto dataInput);
    }
}
