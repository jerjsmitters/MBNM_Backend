using Common.Enums;
using DataAccess.Abstractions.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstractions.Domain
{
    public class ProposedTransactionListChange : EntityBase
    {
        public Transaction Transaction { get; set; }
        public ProposedChangeOperation ProposedChangeOperation { get; set; }
    }
}
