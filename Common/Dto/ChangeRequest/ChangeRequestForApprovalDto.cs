using Common.Dto.Base;
using Common.Dto.ChangeRequest.Line;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.ChangeRequest
{
    public record ChangeRequestDto : EntityBaseDto
    {
        public ChangeRequestDto()
        {
            CreateLines = [];
            DeleteLines = [];
        }

        public List<ChangeRequestCreateLineDto> CreateLines { get; set; }
        public List<ChangeRequestDeleteLineDto> DeleteLines { get; set; }

        public bool IsCommitted => CreateLinesCommitted && DeleteLinesCommitted;
        public bool NothingToDo => !CreateLines.Any() && !DeleteLines.Any();

        private bool CreateLinesCommitted => !CreateLines.Any() || CreateLines.All(x => x.RequestState != Enums.ChangeRequestState.PENDING);
        private bool DeleteLinesCommitted => !DeleteLines.Any() || DeleteLines.All(x => x.RequestState != Enums.ChangeRequestState.PENDING);
    }
}
