using Common.Dto.Transactions;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.ChangeRequest
{
    public record ChangeRequestCommitResultDto(TransactionDto? TransactionBefore, TransactionDto? TransactionAfter, ChangeRequestCommitOperation OperationPerformed);
}
