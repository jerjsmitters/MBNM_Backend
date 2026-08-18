using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Input
{
    public record InputTransactions
    {
        public DateTime InputDateTime { get; init; }
        public List<InputTransaction> Transactions { get; init; }
    }
}
