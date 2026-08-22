using Common.Dto.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto
{
    public record ChangeRequestDto : EntityBaseDto
    {
        public ChangeRequestDto()
        {
            ProposedChanges = [];
        }

        //other metadata
        public List<ChangeRequestTransactionDto> ProposedChanges;
    }
}
