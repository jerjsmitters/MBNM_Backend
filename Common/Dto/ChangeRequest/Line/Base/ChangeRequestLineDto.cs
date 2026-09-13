using Common.Dto.Base;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.ChangeRequest.Line.Base
{
    public abstract record ChangeRequestLineDto : EntityBaseDto
    {
        public ChangeRequestState RequestState { get; init; }

    }
}
