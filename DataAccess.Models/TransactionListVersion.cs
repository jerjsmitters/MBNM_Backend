using DataAccess.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class TransactionListVersion : EntityBase
    {
        //TBI: ONCE FIGURED OUT HOW ITS GONNA WORK 
        public List<Transaction> Transactions { get; set; }
    }
}
