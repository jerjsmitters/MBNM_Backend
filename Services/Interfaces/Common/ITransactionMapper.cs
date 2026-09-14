using Common.Dto.Transactions;
using DataAccess.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Common
{
    public interface ITransactionMapper
    {
        TransactionDto MapTransactionToDto(Transaction transaction);
        Transaction MapDtoToTransaction(TransactionDto transaction);
    }
}
