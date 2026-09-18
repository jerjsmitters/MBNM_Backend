using Common.Enums;
using DataAccess.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Criteria
{
    public class MatchingCriteria : EntityBase
    {
        public List<TransactionProperty> TransactionProperties{ get; set; }
        public List<TransactionCategory> TransactionCategories { get; set; }
        public string SearchText { get; set; }
        public MatchingCriteriaMode MatchingCriteriaMode { get; set; }
    }
}
