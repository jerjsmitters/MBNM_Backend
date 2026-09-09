using DataAccess.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Transactions
{
    public class TransactionProperty : EntityBase
    {
        public List<MatchingCriteria> MatchingCriteria { get; set; }
    }
}
