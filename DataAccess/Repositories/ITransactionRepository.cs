using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Abstractions.Domain;

namespace DataAccess.Abstractions.Repositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        /// <summary>
        /// Get all transactions within date-range, plus any transactions directly before and after which have the same category as the first and last entries respectively
        /// </summary>
        List<Transaction> GetTransactionsForProposedTransactionChange(DateTime startDate, DateTime endDate);
    }
}
