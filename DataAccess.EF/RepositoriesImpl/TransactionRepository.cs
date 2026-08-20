using DataAccess.Abstractions.Domain;
using DataAccess.Abstractions.Repositories;
using DataAccess.EF.DbContextImpl;

namespace DataAccess.EF.RepositoriesImpl
{
    public class TransactionRepository(EfDbContext context) : GenericRepository<Transaction>(context), ITransactionRepository
    {
        public List<Transaction> GetTransactionsForProposedTransactionChange(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
