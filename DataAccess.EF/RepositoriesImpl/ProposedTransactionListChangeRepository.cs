using DataAccess.Abstractions.Domain;
using DataAccess.Abstractions.Repositories;
using DataAccess.EF.DbContextImpl;

namespace DataAccess.EF.RepositoriesImpl
{
    public class ProposedTransactionListChangeRepository(EfDbContext context) : GenericRepository<ProposedTransactionListChange>(context), IProposedTransactionListChangeRepository
    {
    }
}
