using Common.Dto.ChangeRequest;
using Common.Dto.Transactions;
using DataAccess.Models.Base;
using DataAccess.Models.ChangeRequestEntities;
using DataAccess.Models.Transactions;
using Services.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    public class MatchingCriteriaMapper : IMatchingCriteriaMapper
    {


        public MatchingCriteria MapDtoToMatchingCriteria(MatchingCriteriaDto matchingCriteria)
        {
            return new()
            {
                CreatedOn = matchingCriteria.CreatedOn,
                Id = matchingCriteria.Id,
                UpdatedOn = matchingCriteria.UpdatedOn,
                MatchingCriteriaMode = matchingCriteria.MatchingCriteriaMode,
                SearchText = matchingCriteria.SearchText
            };
        }


        public MatchingCriteriaDto MapMatchingCriteriaToDto(MatchingCriteria matchingCriteria)
        {
            return new()
            {
                CreatedOn = matchingCriteria.CreatedOn,
                Id = matchingCriteria.Id,
                UpdatedOn = matchingCriteria.UpdatedOn,
                MatchingCriteriaMode = matchingCriteria.MatchingCriteriaMode,
                SearchText = matchingCriteria.SearchText
            };
        }
    }
}
