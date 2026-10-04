using Common.Dto.ChangeRequest;
using Common.Dto.Input;
using Common.Dto.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Input
{
    public interface IGenerateChangeRequestService
    {
        ChangeRequestDto GenerateChangeRequest(List<InputTransactionDto> inputTransactions, List<TransactionDto> persistedTransactionsForDateRange);
    }
}
