using Common.Dto;
using Common.Dto.Input;
using DataAccess.Abstractions.Domain;
using DataAccess.Abstractions.Repositories;
using Services.Common.Interfaces;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.Common
{
    public class TransactionService(ITransactionRepository transactionRepository, IChangeRequestService proposedTransactionChangeService) : ITransactionService
    {
        public ChangeRequestDto AddTransactions(InputTransactionsDto dataInput)
        {
            if (dataInput == null || (dataInput.Transactions?.Count ?? 0) == 0)
            {
                return new ChangeRequestDto();
            }
            
            var dateRange = GetDateRangeOfInputTransactions(dataInput.Transactions);

            List<TransactionDto> persistedTransactionsForDateRange = GetTransactionsForProposedTransactionChange(dateRange.Item1, dateRange.Item2);

            var proposedTransactionChangeLog = proposedTransactionChangeService.GenerateChangeRequest(dataInput.Transactions, persistedTransactionsForDateRange);
            proposedTransactionChangeService.SaveChangeRequest(proposedTransactionChangeLog);

            return proposedTransactionChangeLog;

        }

        private List<TransactionDto> GetTransactionsForProposedTransactionChange(DateOnly startDate, DateOnly endDate)
        {
            return transactionRepository
                .GetTransactionsForProposedTransactionChange(startDate, endDate)
                .Select(t => MapTransactionToDto(t))
                .ToList();
        }

        private static Tuple<DateOnly, DateOnly> GetDateRangeOfInputTransactions(List<TransactionDto> inputs)
        {
            var startDate = inputs.Max(i => i.Date);
            var endDate = inputs.Min(i => i.Date);
            
            return Tuple.Create(startDate, endDate);
        }

        private TransactionDto MapTransactionToDto(Transaction transaction) 
        {
            return new TransactionDto
            {
                Id = transaction.Id,
                CreatedOn = transaction.CreatedOn,
                Date = transaction.Date,
                Amount = transaction.Amount
            };
        }


    }
}
