using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Input
{
    public record InputTransactionDto
    {
        public DateOnly Date { get; init; }
        public int Sequence { get; init; }
        public decimal Amount { get; init; }
        public string Reference { get; init; }
    }
}
