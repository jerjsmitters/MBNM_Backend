using Common.Dto.Base;
using Common.Dto.ChangeRequest.Line;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Common.Dto.ChangeRequest
{
    public record ChangeRequestDto : EntityBaseDto
    {
        public List<ChangeRequestCreateLineDto> CreateLines { get; init; }
        public List<ChangeRequestDeleteLineDto> DeleteLines { get; init; }

        public bool IsCommitted => CreateLinesCommitted && DeleteLinesCommitted;
        public bool NothingToDo => !CreateLines.Any() && !DeleteLines.Any();

        private bool CreateLinesCommitted => (!CreateLines?.Any() ?? false) || (CreateLines?.All(x => x.RequestState != Common.Enums.ChangeRequestState.PENDING) ?? false);
        private bool DeleteLinesCommitted => (!DeleteLines?.Any() ?? false) || (DeleteLines?.All(x => x.RequestState != Common.Enums.ChangeRequestState.PENDING) ?? false);
    }
}
