using Common.Dto.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Transactions
{
    public record TransactionPropertyDto : EntityBaseDto
    {
        public string Description { get; set; }
        public List<MatchingCriteriaDto> MatchingCriteria { get; set; }

    }
}
