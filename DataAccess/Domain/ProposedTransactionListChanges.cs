using DataAccess.Abstractions.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstractions.Domain
{
    public class ProposedTransactionListChanges : EntityBase
    {
        public List<ProposedTransactionListChange> ProposedChanges {  get; set; }
    }
}
