using Common.Dto.Base;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Criteria
{
    public record MatchingCriteriaDto : EntityBaseDto
    {
        public string SearchText { get; init; }
        public MatchingCriteriaMode MatchingCriteriaMode { get; init; }
    }
}
