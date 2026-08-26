using Common.Dto;

namespace Services.Common.Interfaces
{
    internal interface ITransactionMatchingService
    {
        bool InputTransactionMatchesPersisted(TransactionDto transactionOne, TransactionDto transactionTwo);
        bool NeedsUpdating(TransactionDto inputTranscation, TransactionDto persistedDto);
    }
}