using Common.Dto.Transactions;
using System;
using System.Collections.Generic;
using System.Text;
using Common.Dto.Filters;

namespace Services.Interfaces.Output
{
    public interface IGenerateExportFileService
    {
        Task<FileInfo> GenerateExportFile(TransactionEntityFilter filtersUsedToGetTransactions, List<TransactionDto> transactions);
    }
}
