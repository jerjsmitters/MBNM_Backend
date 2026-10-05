using Common.Dto.Api;
using Common.Dto.Transactions;
using DataAccess.EF.DbContextImpl;
using Services.Interfaces.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Transactions
{
    //TBI
    public class TransactionEvidenceService(EfDbContext dbContext) : ITransactionEvidenceService
    {
        public Task<TransactionEvidenceDto> Create(CreateEvidenceDto createEvidenceDto)
        {

            throw new NotImplementedException();
        }

        public Task Delete(int transactionEvidenceId)
        {
            throw new NotImplementedException();
        }
    }
}
 