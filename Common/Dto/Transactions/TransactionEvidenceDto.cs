using Common.Dto.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Common.Dto.Transactions
{
    public record TransactionEvidenceDto : EntityBaseDto
    {
        public string FileLocation { get; set; } //use local for now, can move to S3 eventually
        public TransactionDto Transaction { get; set; }
    }
}
