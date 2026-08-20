using DataAccess.Abstractions.Domain;
using DataAccess.Abstractions.Repositories;
using DataAccess.EF.DbContextImpl;

namespace DataAccess.EF.RepositoriesImpl
{
    public class TransactionCategoryRepository(EfDbContext context) : GenericRepository<TransactionCategory>(context), ITransactionCategoryRepository
    {
    }
}
