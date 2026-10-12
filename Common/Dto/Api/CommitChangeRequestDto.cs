using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Api
{
    public record CommitChangeRequestDto(int ChangeRequestId, List<ApplyChangeRequestLineDto> CreateLines, List<ApplyChangeRequestLineDto> DeleteLines);
}
