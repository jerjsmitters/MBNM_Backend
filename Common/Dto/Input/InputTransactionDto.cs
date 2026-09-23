using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Input
{
    public record InputTransactionDto(DateOnly Date, int Sequence, decimal Amount, string Reference);
}
