using Common.Dto.Api;
using Common.Dto.Transactions;
using DataAccess.EF.DbContextImpl;
using DataAccess.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Services.Common;
using Services.Interfaces.Common;
using Services.Interfaces.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Transactions
{
    //TBI
    public class TransactionEvidenceService(EfDbContext dbContext, ITransactionEvidenceMapper mapper) : ITransactionEvidenceService
    {
        public async Task<string> GetFilePath(int id)
        {
            var evidence = await dbContext.TransactionEvidence.FirstOrDefaultAsync(x => x.Id == id) ?? throw new InvalidOperationException("Can't find evidence");

            return evidence.FileLocation;
        }

        public async Task<TransactionEvidenceDto> Create(CreateEvidenceDto createEvidenceDto)
        {
            var filePath = Path.Combine(Constants.FileRootLocation, createEvidenceDto.File.FileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await createEvidenceDto.File.CopyToAsync(fileStream);
            }

            try
            {
                var relevantTransaction = await dbContext.Transactions.FirstOrDefaultAsync(x => x.Id == createEvidenceDto.TransactionId)
                    ?? throw new InvalidOperationException("Cannot find transaction");

                var createEvidenceModel = new TransactionEvidence
                {
                    FileLocation = filePath,
                    Transaction = relevantTransaction
                };

                var result = await dbContext.AddAsync(createEvidenceModel);
                await dbContext.SaveChangesAsync();
                
                return mapper.MapEntityToDto(result.Entity);

            }
            catch
            {
                File.Delete(filePath);
                throw;
            }
        }

        public async Task Delete(int transactionEvidenceId)
        {
            var persisted = await dbContext
                .TransactionEvidence
                .FirstOrDefaultAsync(e => e.Id == transactionEvidenceId);

            if (persisted == null)
            {
                return;
            }

            persisted.IsDeleted = true;
            await dbContext.SaveChangesAsync();
        }

    }
}
 