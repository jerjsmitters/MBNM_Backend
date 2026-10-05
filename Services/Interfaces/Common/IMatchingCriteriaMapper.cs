using Common.Dto.Base;
using Common.Dto.ChangeRequest;
using Common.Dto.Criteria;
using DataAccess.Models.Base;
using DataAccess.Models.ChangeRequestEntities;
using DataAccess.Models.Criteria;
using Services.Interfaces.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Common
{
    public interface IMatchingCriteriaMapper : IMapper<MatchingCriteria, MatchingCriteriaDto>
    {
        ENT ProjectDtoToEntityWithCriteria<ENT, DTO>(DTO incoming, ENT entity)
            where ENT : EntityWithCriteria
            where DTO : EntityWithCriteriaDto;
    }
}
