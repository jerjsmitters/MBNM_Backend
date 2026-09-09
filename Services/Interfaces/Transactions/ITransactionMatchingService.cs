using Common.Dto.Input;
using Common.Dto.Transactions;

namespace Services.Interfaces.Transactions
{
    public interface ITransactionMatchingService
    {
        bool InputTransactionMatchesPersisted(InputTransactionDto inputTransaction, TransactionDto persistedTransaction);
        bool PersistedTransactionNeedsUpdatingByInput(InputTransactionDto inputTranscation, TransactionDto persistedDto);
    }
}