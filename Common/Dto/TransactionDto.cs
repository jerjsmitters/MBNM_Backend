using Common.Dto.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto
{
    public record TransactionDto : EntityBaseDto
    {
        public DateOnly Date { get; set; }
        public short Sequence { get; set; }
        public decimal Amount { get; init; }
        public string Reference { get; init; }
    }
}
