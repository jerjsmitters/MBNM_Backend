using DataAccess.Abstractions.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstractions.Domain
{
    public class Transaction : EntityBase
    {
        public decimal Amount { get; set;  }
        public DateOnly Date { get; set; }
        public short Sequence { get; set; }
        public bool Deleted { get; set; }
        public TransactionCategory Category { get; set; }
        public List<TransactionEvidence> Evidence { get; set; }
    }
}
