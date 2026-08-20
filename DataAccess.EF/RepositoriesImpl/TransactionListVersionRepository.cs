using DataAccess.Abstractions.Domain;
using DataAccess.Abstractions.Repositories;
using DataAccess.EF.DbContextImpl;

namespace DataAccess.EF.RepositoriesImpl
{
    public class TransactionListVersionRepository(EfDbContext context) : GenericRepository<TransactionListVersion>(context), ITransactionListVersionRepository
    {
    }
}
