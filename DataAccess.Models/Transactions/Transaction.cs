using DataAccess.Models.Base;
using DataAccess.Models.Criteria;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Transactions
{
    public class Transaction : EntityBase
    {
        public decimal Amount { get; set; }
        public DateOnly Date { get; set; }
        public int Sequence { get; set; }
        public string Reference { get; set; }
        public bool EvidencePending { get; set;  }

        public List<TransactionCategory> AssignedCategories { get; set; }
        public List<TransactionProperty> AssignedProperties { get; set; }
        public List<TransactionEvidence> Evidence { get; set; }
    }
}
