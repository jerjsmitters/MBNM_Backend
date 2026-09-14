using Common.Dto;
using Common.Dto.ChangeRequest;
using Common.Dto.Input;
using DataAccess.EF.DbContextImpl;
using DataAccess.Models;
using Services.Common;
using Services.Interfaces.Common;
using Services.Interfaces.Input;
using Services.Interfaces.Transactions;
using System.Data;

namespace Services.Transactions
{
    public class TransactionService(EfDbContext dbContext, IChangeRequestService proposedTransactionChangeService, ITransactionMapper mappingService) : ITransactionService
    {
        
    }
}
