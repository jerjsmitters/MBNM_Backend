using DataAccess.Abstractions.Domain;
using DataAccess.Abstractions.Repositories;
using DataAccess.EF.DbContextImpl;

namespace DataAccess.EF.RepositoriesImpl
{
    public class ProposedTransactionListChangesRepository(EfDbContext context) : GenericRepository<ChangeRequest>(context), IChangeRequestRepository
    {
    }

}
