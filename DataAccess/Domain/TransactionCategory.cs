using DataAccess.Abstractions.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstractions.Domain
{
    public class TransactionCategory : EntityBase
    {
        public int Description { get; set; }
    }
}
