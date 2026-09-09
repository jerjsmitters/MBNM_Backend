using Common.Dto.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Input
{
    public record InputTransactionsDto
    {
        public DateTime InputDateTime { get; init; }
        public List<InputTransactionDto> Transactions { get; init; }
    }
}
