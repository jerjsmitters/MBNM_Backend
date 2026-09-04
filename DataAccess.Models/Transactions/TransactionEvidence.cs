using DataAccess.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Transactions
{
    public class TransactionEvidence : EntityBase
    {
        public string FileLocation { get; set; } //use local for now, can move to S3 eventually
        public Transaction Transaction { get; set; }
    }
}
