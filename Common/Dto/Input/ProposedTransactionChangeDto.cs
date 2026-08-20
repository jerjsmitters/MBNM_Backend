using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Input
{
    public record ProposedTransactionChangeDto
    {
        public int Id { get; init; }
        public ProposedChangeOperation ProposedChangeOperation { get; init; }
        public int TransactionId { get; init; }
    }
}
