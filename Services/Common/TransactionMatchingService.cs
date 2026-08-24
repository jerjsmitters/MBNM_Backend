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
            };

            return transactionOne.Date == transactionTwo.Date
                && transactionOne.Sequence == transactionTwo.Sequence;
        }
    }
}
