using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Base
{
    public abstract record TransactionEntityBaseDto : EntityBaseDto
    {
        public DateOnly Date { get; init; }
        public int Sequence { get; init; }
        public decimal Amount { get; init; }
        public decimal Balance { get; init; }
        public string Reference { get; init; }
    }
}
