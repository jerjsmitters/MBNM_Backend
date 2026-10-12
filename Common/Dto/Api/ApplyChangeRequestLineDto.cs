using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Api
{
    public record ApplyChangeRequestLineDto(int LineId, ChangeRequestState DesiredState);
}