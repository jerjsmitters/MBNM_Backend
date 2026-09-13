using Common.Dto.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Transactions
{
    public record TransactionCategoryDto : EntityBaseDto
    {
        public string Description { get; init; }
        public List<MatchingCriteriaDto> MatchingCriteria { get; set; }
    }
}
 