using Common.Dto;
using Common.Dto.Input;
using DataAccess.Abstractions.Domain;
using DataAccess.Abstractions.Repositories;
using Services.Common.Interfaces;
using System.Data;

namespace Services.Common
{
    public class TransactionService(ITransactionRepository transactionRepository, IProposedTransactionChangeService proposedTransactionChangeService) : ITransactionService
    {
        public ProposedTransactionChangesDto AddTransactions(InputTransactionsDto dataInput)
        {
            if (dataInput == null || (dataInput.Transactions?.Count ?? 0) == 0)
            {
                return new ProposedTransactionChangesDto();
            }
            
            var dateRange = GetDateRangeOfInputTransactions(dataInput.Transactions);

            List<TransactionDto> persistedTransactionsForDateRange = GetTransactionsForProposedTransactionChange(dateRange.Item1, dateRange.Item2);

            var proposedTransactionChangeLog = proposedTransactionChangeService.GenerateProposedChanges(dataInput.Transactions, persistedTransactionsForDateRange);
            proposedTransactionChangeService.SaveProposedChanges(proposedTransactionChangeLog);

            return proposedTransactionChangeLog;

        }

        private List<TransactionDto> GetTransactionsForProposedTransactionChange(DateTime startDate, DateTime endDate)
        {
            return transactionRepository
                .GetTransactionsForProposedTransactionChange(startDate, endDate)
                .Select(t => MapTransactionToDto(t))
                .ToList();
        }

        private static Tuple<DateTime, DateTime> GetDateRangeOfInputTransactions(List<InputTransactionDto> inputs)
        {
            var startDate = inputs.Max(i => i.TransactionDate);
            var endDate = inputs.Min(i => i.TransactionDate);
            
            return Tuple.Create(startDate, endDate);
        }

        private TransactionDto MapTransactionToDto(Transaction transaction) 
        {
            throw new NotImplementedException();
        }


    }
}
