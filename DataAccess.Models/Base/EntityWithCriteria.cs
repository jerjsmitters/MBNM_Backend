using DataAccess.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Base
{
    public class EntityWithCriteria : EntityBase
    {
        public List<MatchingCriteria> CriteriaToMatchOn { get; set; }
    }
}
