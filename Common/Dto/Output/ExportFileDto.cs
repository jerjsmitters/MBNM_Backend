using Common.Dto.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Output
{
    public record ExportFileDto : EntityBaseDto
    {
        public string FilePath { get; init; }
    }
}
