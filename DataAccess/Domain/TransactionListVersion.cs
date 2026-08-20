using DataAccess.Abstractions.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstractions.Domain
{
    public class TransactionListVersion : EntityBase
    {
        //TBI: ONCE FIGURED OUT HOW ITS GONNA WORK 
        public List<Transaction> Transactions { get; set; }
    }
}
