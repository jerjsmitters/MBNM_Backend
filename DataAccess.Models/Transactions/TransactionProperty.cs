using DataAccess.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Transactions
{
    public class TransactionProperty : EntityBase
    {
        public string Description { get; set; }
        public List<MatchingCriteria> MatchingCriteria { get; set; }
    }
}
