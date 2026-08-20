using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Input
{
    public record InputTransactionDto
    {
        public DateTime TransactionDate { get; init; } //might need sequence if transaction only has dates and not times
        public string Reference { get; init; }
        public decimal TransactionAmount { get; init; }
    }
}
