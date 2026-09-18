using Common.Dto.Base;
using Common.Dto.Criteria;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Transactions
{
    public record TransactionDto : EntityBaseDto
    {
        public DateOnly Date { get; set; }
        public int Sequence { get; set; }
        public decimal Amount { get; init; }
        public string Reference { get; init; }
        public List<TransactionCategoryDto> Categories { get; init; }
        public List<TransactionPropertyDto> Properties { get; init; }
    }
}
