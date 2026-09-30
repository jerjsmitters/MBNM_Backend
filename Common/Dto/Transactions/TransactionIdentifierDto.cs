using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Transactions
{
    public record TransactionIdentifierDto(DateOnly Date, int Sequence, decimal Amount, string Reference);
}
