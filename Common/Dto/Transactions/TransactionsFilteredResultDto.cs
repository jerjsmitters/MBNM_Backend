using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Transactions
{
    public record TransactionsFilteredResultDto(List<TransactionDto> Transactions, int TotalResults);
}
