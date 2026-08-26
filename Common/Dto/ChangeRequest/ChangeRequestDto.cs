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
    }
}
