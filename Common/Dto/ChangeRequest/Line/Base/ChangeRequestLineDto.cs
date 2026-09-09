using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.ChangeRequest.Line.Base
{
    public abstract record ChangeRequestLineDto
    {
        public ChangeRequestState RequestState { get; init; }

    }
}
