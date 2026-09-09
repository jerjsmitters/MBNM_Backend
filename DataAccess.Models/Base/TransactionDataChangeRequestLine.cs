using Common.Enums;
using DataAccess.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Base
{
    public abstract class TransactionDataChangeRequestLine : ChangeRequestLine
    {
        public decimal Amount { get; set; }
        public DateOnly Date { get; set; }
        public short Sequence { get; set; }
        public string Reference { get; set; }
        public List<TransactionCategory> TransactionCategories { get; set; }   
        public List<TransactionProperty> TransactionProperties { get; set; }
    }
}
