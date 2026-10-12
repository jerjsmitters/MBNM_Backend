using Common.Dto.Output;
using DataAccess.Models.Output;
using Services.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    public class ExportFileMapper : IExportFileMapper
    {
        public ExportFile MapDtoToEntity(ExportFileDto property) => new()
        {
            Id = property.Id,
            FilePath = property.FilePath,

            CreatedOn = property.CreatedOn,
            UpdatedOn = property.UpdatedOn,
            IsDeleted = property.IsDeleted
        };

        public ExportFileDto MapEntityToDto(ExportFile property) => new()
        {
            Id = property.Id,
            FilePath = property.FilePath,

            CreatedOn = property.CreatedOn,
            UpdatedOn = property.UpdatedOn,
            IsDeleted = property.IsDeleted
        };
    }
}
