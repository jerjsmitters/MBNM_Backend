using Common.Dto.Api;
using Common.Dto.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Transactions
{
    public interface ITransactionEvidenceService
    {
        Task<string> GetFilePath(int id);
        Task<TransactionEvidenceDto> Create(CreateEvidenceDto createEvidenceDto);
        Task Delete(int transactionEvidenceId);
    }
}
