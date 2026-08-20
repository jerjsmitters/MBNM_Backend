using DataAccess.Abstractions.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstractions.Domain
{
    public class TransactionEvidence : EntityBase
    {
        public string FileLocation { get; set; } //use local for now, can move to S3 eventually
    }
}
