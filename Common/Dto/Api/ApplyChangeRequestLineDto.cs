using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Api
{
    public record ApplyChangeRequestLineDto
    {
        public int LineId { get; set; }
        public ChangeRequestState DesiredState { get; set; }
    }
}
