using DataAccess.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Criteria
{
    public class TransactionProperty : EntityWithCriteria
    {
        public string Description { get; set; }
    }
}
