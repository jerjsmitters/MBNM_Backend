using DataAccess.Abstractions.Domain;
using DataAccess.Abstractions.Repositories;
using DataAccess.EF.DbContextImpl;

namespace DataAccess.EF.RepositoriesImpl
{
    public class TransactionEvidenceRepository(EfDbContext context) : GenericRepository<TransactionEvidence>(context), ITransactionEvidenceRepository
    {
    }
}
