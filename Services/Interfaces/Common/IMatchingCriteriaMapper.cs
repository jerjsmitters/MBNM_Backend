using Common.Dto.Base;
using Common.Dto.ChangeRequest;
using Common.Dto.Transactions;
using DataAccess.Models.Base;
using DataAccess.Models.ChangeRequestEntities;
using DataAccess.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Common
{
    public interface IMatchingCriteriaMapper
    {
        MatchingCriteria MapDtoToMatchingCriteria(MatchingCriteriaDto matchingCriteria);
        MatchingCriteriaDto MapMatchingCriteriaToDto(MatchingCriteria matchingCriteria);
        MatchingCriteria ProjectDtoToMatchingCriteria(MatchingCriteriaDto incoming, MatchingCriteria entity);
        ENT ProjectDtoToEntityWithCriteria<ENT, DTO>(DTO incoming, ENT entity)
            where ENT : EntityWithCriteria
            where DTO : EntityWithCriteriaDto;
    }
}
