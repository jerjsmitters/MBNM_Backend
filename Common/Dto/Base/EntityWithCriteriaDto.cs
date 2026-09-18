using Common.Dto.Criteria;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Base
{
    public record EntityWithCriteriaDto : EntityBaseDto
    {
        public string Description { get; init; }
        public List<MatchingCriteriaDto> CriteriaToMatchOn { get; init; }
    }
}
