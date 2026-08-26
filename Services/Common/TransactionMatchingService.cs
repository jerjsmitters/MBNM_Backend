using Common.Dto;
using Services.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    public class TransactionMatchingService : ITransactionMatchingService
    {
        public bool InputTransactionMatchesPersisted(TransactionDto transactionOne, TransactionDto transactionTwo)
        {
            if (transactionOne == null || transactionTwo == null)
            {
                return false;
            }
            return InputTransactionMatchesPersistedInternal(transactionOne, transactionTwo);
        }

        public bool NeedsUpdating(TransactionDto inputTranscation, TransactionDto persistedDto)
        {
            if (inputTranscation == null || persistedDto == null)
            {
                return false;
            }

            var valueHasChanged = inputTranscation.Reference != persistedDto.Reference
                    || inputTranscation.Amount != persistedDto.Amount;

            return InputTransactionMatchesPersistedInternal(inputTranscation, persistedDto)
                && valueHasChanged;


        }


        private bool InputTransactionMatchesPersistedInternal(TransactionDto transactionOne, TransactionDto transactionTwo)
        {
            return transactionOne.Date == transactionTwo.Date
                && transactionOne.Sequence == transactionTwo.Sequence;
        }
    }
}
