using Common.Dto.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Common.Dto.Transactions
{
    public record TransactionEvidenceDto : EntityBaseDto
    {
        public string FileLocation { get; init; }
        public TransactionDto Transaction { get; init; }

    } //use local for now, can move to S3 eventually
}
