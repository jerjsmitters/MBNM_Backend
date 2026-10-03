using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Api
{
    public record CommitChangeRequestDto
    {
        public int ChangeRequestId { get; init;  }
        public List<ApplyChangeRequestLineDto> CreateLines { get; init; }
        public List<ApplyChangeRequestLineDto> DeleteLines { get; init; }
    }
}
