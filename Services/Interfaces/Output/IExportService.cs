using Common.Dto.Filters;
using Common.Dto.Output;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Output
{
    public interface IExportService 
    {
        Task<ExportFileDto> CreateExportFileForTransactions(TransactionEntityFilter transactionFilter);
        Task<ExportFileDto?> GetExportFile(int id);
        Task<List<ExportFileDto>> GetAllExportFiles();
    }
}