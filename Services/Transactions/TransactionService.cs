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
                .Include(t => t.AssignedCategories)
                .Include(t => t.AssignedProperties)
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
                .Include(t => t.AssignedCategories)
                .Include(t => t.AssignedProperties)
                .ToListAsync();

            return entities
                .Select(mappingService.MapTransactionToDto)
                .ToList();

        }

        public async Task<List<TransactionDto>> GetAllFiltered(EntityFilter filter)
        {
            IQueryable<Transaction> query = dbContext
                .Transactions
                .Include(t => t.AssignedCategories)
                .Include(t => t.AssignedProperties);

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

        public async Task<TransactionDto> Update(TransactionDto update)
        {
            var entity = await dbContext
                .Transactions
                .Include(x => x.AssignedCategories)
                .Include(x => x.AssignedProperties)
                .FirstOrDefaultAsync(x => x.Id == update.Id) 
                ?? throw new InvalidOperationException("doesnt exist");

            entity = mappingService.ProjectDtoToTransaction(update, entity);

            //attach many to many
            entity = AttachManyToManyEntities(entity);

            await dbContext.SaveChangesAsync();

            return mappingService.MapTransactionToDto(entity);
        }

        private Transaction AttachManyToManyEntities(Transaction transaction)
        {
            var categoryIds = transaction.AssignedCategories.Select(x => x.Id);
            var propertyIds = transaction.AssignedProperties.Select(x => x.Id);

            var categories = dbContext.TransactionCategories.Where(x => categoryIds.Contains(x.Id)).ToList();
            var properties = dbContext.TransactionProperties.Where(x => propertyIds.Contains(x.Id)).ToList();

            transaction.AssignedCategories = categories;
            transaction.AssignedProperties = properties;

            return transaction;
        }
    }
}
