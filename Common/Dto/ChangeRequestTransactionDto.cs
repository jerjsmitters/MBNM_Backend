using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto
{
    public record ChangeRequestTransactionDto
    {
        public ChangeRequestTransactionDto(TransactionDto transaction, ChangeRequestOperation proposedChangeOperation)
        {
            this.Transaction = transaction;
            this.ProposedChangeOperation = proposedChangeOperation;
        }

        public int? Id { get; init; }
        public TransactionDto Transaction { get; init; }
        public ChangeRequestOperation ProposedChangeOperation { get; init; }
    }
}
