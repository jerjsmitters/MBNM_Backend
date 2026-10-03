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
    public class TransactionService(EfDbContext dbContext, ITransactionMapper mappingService) : ITransactionService
    {
        private const int NoAssignedPropertyId = 0;
        private const int NoAssignedCategoryId = 0;
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
            var entities = await GetNonDeletedSequenced().ToListAsync();

            return entities
                .Select(mappingService.MapTransactionToDto)
                .ToList();

        }

        public async Task<TransactionsFilteredResultDto> GetAllFiltered(TransactionEntityFilter filter)
        {
            var query = GetNonDeletedSequenced();

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

            query = query.Where(q => 
                (filter.PropertyIds.Contains(NoAssignedPropertyId) && !q.AssignedProperties.Any())
                || q.AssignedProperties.Any(x => filter.PropertyIds.Contains(x.Id.Value))
            );

            query = query.Where(q =>
                   (filter.CategoryIds.Contains(NoAssignedPropertyId) && !q.AssignedCategories.Any())
                   || q.AssignedCategories.Any(x => filter.CategoryIds.Contains(x.Id.Value))
               );

            var startPosition = (filter.PageNumber - 1) * filter.PageLength;

            var entities = await query.Skip(startPosition).Take(filter.PageLength).ToListAsync();
            var dtos = entities.Select(mappingService.MapTransactionToDto).ToList();

            var totalCount = await query.CountAsync();

            return new(dtos, totalCount);
                
        }

        public async Task Delete(int id)
        {
            await using var efDbTransaction = await dbContext.Database.BeginTransactionAsync();

            var transaction = await dbContext
                .Transactions
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return;
            }

            transaction.IsDeleted = true;
            await dbContext.SaveChangesAsync();

            DateOnly 
                startDate = transaction.Date, 
                endDate = transaction.Date;

            await AdjustSequenceOfTransactionsByDateInternal(startDate, endDate);
            await dbContext.SaveChangesAsync();

            await efDbTransaction.CommitAsync();
        }

        public async Task<TransactionDto> Update(TransactionDto update)
        {
            var entity = await dbContext
                .Transactions
                .FirstOrDefaultAsync(x => x.Id == update.Id) 
                ?? throw new InvalidOperationException("doesnt exist");

            entity = mappingService.ProjectDtoToTransaction(update, entity);

            //attach many to many
            entity = AttachManyToManyEntities(entity);

            await dbContext.SaveChangesAsync();

            return mappingService.MapTransactionToDto(entity);
        }

        public Task AdjustSequenceOfTransactionsByDate(DateOnly startDate, DateOnly endDate)
        {
            return AdjustSequenceOfTransactionsByDateInternal(startDate, endDate);
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
        
        private async Task AdjustSequenceOfTransactionsByDateInternal(DateOnly startDate, DateOnly endDate)
        {
            var transactionsForDateRange = await dbContext
                .Transactions
                .Where(x => x.Date >= startDate && x.Date <= endDate)
                .Where(x => !x.IsDeleted)
                .ToListAsync();

            var sequence = 0;

            DateOnly? dateForSequence = null;
            foreach (var transactionLine in transactionsForDateRange)
            {
                var transactionLineDate = transactionLine.Date;

                if (!dateForSequence.HasValue || !dateForSequence.Equals(transactionLineDate))
                {
                    dateForSequence = transactionLineDate;
                    sequence = 0;
                }

                transactionLine.Sequence = sequence; ;
                sequence++;
            }
        }

        private IQueryable<Transaction> GetNonDeletedSequenced() => 
            dbContext
                .Transactions
                .Where(t => !t.IsDeleted)
                .OrderBy(t => t.Date)
                .ThenBy(t => t.Sequence);
    }
}
