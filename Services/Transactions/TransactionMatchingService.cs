using Common.Dto.Input;
using Common.Dto.Transactions;
using Services.Interfaces.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Transactions
{
    public class TransactionMatchingService : ITransactionMatchingService
    {
        public bool InputTransactionMatchesPersisted(InputTransactionDto inputTransaction, TransactionDto persistedTransaction)
        {
            if (inputTransaction == null || persistedTransaction == null)
            {
                return false;
            }
            return InputTransactionMatches(inputTransaction, persistedTransaction);
        }

        public bool PersistedTransactionNeedsUpdatingByInput(InputTransactionDto inputTranscation, TransactionDto persistedTransaction)
        {
            if (inputTranscation == null || persistedTransaction == null)
            {
                return false;
            }

            var valueHasChanged = inputTranscation.Reference != persistedTransaction.Reference
                    || inputTranscation.Amount != persistedTransaction.Amount;

            return InputTransactionMatches(inputTranscation, persistedTransaction)
                && valueHasChanged;


        }


        private bool InputTransactionMatches(InputTransactionDto transactionOne, TransactionDto transactionTwo)
        {
            return transactionOne.Date == transactionTwo.Date
                && transactionOne.Sequence == transactionTwo.Sequence;
        }
    }
}
