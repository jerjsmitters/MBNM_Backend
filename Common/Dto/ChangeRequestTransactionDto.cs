using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto
{
    public record ChangeRequestTransactionDto
    {
        public ChangeRequestTransactionDto(TransactionDto transaction, ChangeRequestCommitOperation proposedChangeOperation)
        {
            this.Transaction = transaction;
            this.ProposedChangeOperation = proposedChangeOperation;
        }

        public int? Id { get; init; }
        public TransactionDto Transaction { get; init; }
        public ChangeRequestCommitOperation ProposedChangeOperation { get; init; }
    }
}
