using Common.Dto.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Input
{
    public record InputTransactionsDto(DateTime InputDateTime, List<InputTransactionDto> Transactions);
}
