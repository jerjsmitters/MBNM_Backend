using Common.Dto;
using Common.Dto.Api;
using Common.Dto.ChangeRequest;
using Common.Dto.Input;
using Common.Dto.Transactions;
using DataAccess.EF.DbContextImpl;
using DataAccess.Models;
using DataAccess.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Services.Common;
using Services.Interfaces.Common;
using Services.Interfaces.Input;
using Services.Interfaces.Transactions;
using System.Data;

namespace Services.Transactions
{
    public class TransactionService(EfDbContext dbContext, IChangeRequestService proposedTransactionChangeService, ITransactionMapper mappingService) : ITransactionService
    {

        public async Task<TransactionDto?> Get(int id)
        {
            var transaction = await dbContext
                .Transactions
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return null;
            }

            return mappingService.MapTransactionToDto(transaction);
        }

        public async Task<List<TransactionDto>> GetAll()
        {
            var entities = await dbContext
                .Transactions
                .ToListAsync();

            return entities
                .Select(mappingService.MapTransactionToDto)
                .ToList();

        }

        public async Task<List<TransactionDto>> GetAllFiltered(EntityFilter filter)
        {
            IQueryable<Transaction> query = dbContext.Transactions;

            if (filter.StartDate.HasValue)
            {
                query = query.Where(q => q.Date >= filter.StartDate);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(q => q.Date <= filter.EndDate);
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                query = query.Where(q => q.Reference.Contains(filter.SearchText));
            }

            var startPosition = filter.PageNumber * filter.PageLength;
            query = query.Skip(startPosition);
            query = query.Take(filter.PageLength);

            var entities = await query.ToListAsync();

            return entities
                .Select(mappingService.MapTransactionToDto)
                .ToList();
        }

        public async Task Delete(int id)
        {
            var transaction = await dbContext
                .Transactions
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return;
            }

            transaction.IsDeleted = true;
        }

    }
}
