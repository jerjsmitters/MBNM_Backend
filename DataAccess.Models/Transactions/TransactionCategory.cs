using DataAccess.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Transactions
{
    public class TransactionCategory : EntityWithCriteria
    {
        public string Description { get; set; }
    }
}
