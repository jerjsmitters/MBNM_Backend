using DataAccess.Abstractions.Domain;
using DataAccess.Abstractions.Repositories;
using DataAccess.EF.DbContextImpl;

namespace DataAccess.EF.RepositoriesImpl
{
    public class TransactionRepository(EfDbContext context) : GenericRepository<Transaction>(context), ITransactionRepository
    {
        public List<Transaction> GetTransactionsForProposedTransactionChange(DateOnly startDate, DateOnly endDate)
        {
            throw new NotImplementedException();
        }
    }
}
