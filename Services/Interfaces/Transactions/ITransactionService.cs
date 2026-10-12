using Common.Dto.ChangeRequest;
using Common.Dto.Filters;
using Common.Dto.Input;
using Common.Dto.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Transactions
{
    public interface ITransactionService
    {
        Task<TransactionDto?> Get(int id);

        Task<List<TransactionDto>> GetAll();

        Task<TransactionsFilteredResultDto> GetAllFiltered(TransactionEntityFilter entityFilter);

        Task<TransactionDto> Update(TransactionDto update);

        Task Delete(int id);

        Task AdjustSequenceOfTransactionsByDate(DateOnly startDate, DateOnly endDate);

        Task<bool> ToggleNeedsEvidence(int id);

        Task RecalculateBalances(List<int> modifiedTransactionIds);
    }
}
