
using Common.Dto.Output;
using DataAccess.Models.Output;
using Services.Interfaces.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Common
{
    public interface IExportFileMapper : IMapper<ExportFile, ExportFileDto> {}
}
