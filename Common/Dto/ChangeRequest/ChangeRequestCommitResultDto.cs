using Common.Dto.Transactions;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.ChangeRequest
{
    public record ChangeRequestCommitResultDto
    {
        public TransactionDto? TransactionBefore { get; init; }
        public TransactionDto? TransactionAfter { get; init; }
        public ChangeRequestCommitOperation OperationPerformed { get; init; }
    }
}
