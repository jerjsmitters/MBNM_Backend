using Common.Dto.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Base
{
    public record EntityWithCriteriaDto : EntityBaseDto
    {
        public List<MatchingCriteriaDto> CriteriaToMatchOn { get; init; }
    }
}
