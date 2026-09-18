using Common.Dto.Criteria;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Base
{
    public abstract record TransactionDataChangeRequestLineDto : ChangeRequestLineDto
    {
        public decimal ProposedAmount { get; init; }
        public DateOnly ProposedDate { get; init; }
        public int ProposedSequence { get; init; }
        public string ProposedReference { get; init; }
        public List<TransactionCategoryDto> ProposedCategories { get; init; }
        public List<TransactionPropertyDto> ProposedProperties { get; init; }
    }
}
