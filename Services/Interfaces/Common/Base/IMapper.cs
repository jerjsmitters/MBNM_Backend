using Common.Dto.Base;
using DataAccess.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Common.Base
{
    public interface IMapper<ENT,DTO> 
        where ENT : EntityBase 
        where DTO : EntityBaseDto
    {
        DTO MapEntityToDto(ENT property);

        /// <summary>
        /// All navigation properties will NOT be tracked and will need to be attached later!
        /// </summary>
        ENT MapDtoToEntity(DTO property);
    }
}
