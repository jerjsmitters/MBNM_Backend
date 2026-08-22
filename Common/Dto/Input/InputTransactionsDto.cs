using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Input
{
    public record InputTransactionsDto
    {
        public DateTime InputDateTime { get; init; }
        public List<TransactionDto> Transactions { get; init; }
    }
}
