using Common.Dto.Base;
using Common.Dto.Criteria;
using DataAccess.Models.Base;
using DataAccess.Models.Criteria;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Common.Base
{
    public interface IMapperWithProjection<ENT,DTO> : IMapper<ENT,DTO>
        where ENT : EntityBase
        where DTO : EntityBaseDto
    {
        /// <summary>
        /// All navigation properties will NOT be tracked and will need to be attached later!
        /// </summary>
        ENT ProjectDtoToEntity(DTO incoming, ENT entity);
    }
}
