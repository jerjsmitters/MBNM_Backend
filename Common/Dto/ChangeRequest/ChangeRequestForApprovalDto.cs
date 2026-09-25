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
            UpdateLines = [];
            DeleteLines = [];
        }

        public List<ChangeRequestCreateLineDto> CreateLines { get; set; }
        public List<ChangeRequestUpdateLineDto> UpdateLines { get; set; }
        public List<ChangeRequestDeleteLineDto> DeleteLines { get; set; }

        public bool IsCommited => CreateLinesCommitted && UpdateLinesCommitted && DeleteLinesCommitted;

        private bool CreateLinesCommitted => !CreateLines.Any() || CreateLines.All(x => x.RequestState != Enums.ChangeRequestState.PENDING);
        private bool UpdateLinesCommitted => !UpdateLines.Any() || UpdateLines.All(x => x.RequestState != Enums.ChangeRequestState.PENDING);
        private bool DeleteLinesCommitted => !DeleteLines.Any() || DeleteLines.All(x => x.RequestState != Enums.ChangeRequestState.PENDING);
    }
}
