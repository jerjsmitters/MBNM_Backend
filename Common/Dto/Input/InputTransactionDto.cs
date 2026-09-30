using Common.Dto.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Input
{
    public record InputTransactionDto(DateOnly Date, int Sequence, decimal Amount, string Reference)
    {
        public TransactionIdentifierDto GetIdentifier() => new(Date, Sequence, Amount, Reference);
    }
}
