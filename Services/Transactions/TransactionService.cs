using Common.Dto;
using Common.Dto.ChangeRequest;
using Common.Dto.Filters;
using Common.Dto.Input;
using Common.Dto.Transactions;
using DataAccess.EF.DbContextImpl;
using DataAccess.Models;
using DataAccess.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Common;
using Services.Interfaces.Common;
using Services.Interfaces.Input;
using Services.Interfaces.Transactions;
using System.Data;

namespace Services.Transactions
{
    public class TransactionService(EfDbContext dbContext, ITransactionMapper mappingService, IMatchingCriteriaService matchingCriteriaService) : ITransactionService
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

            return mappingService.MapEntityToDto(transaction);
        }

        public async Task<List<TransactionDto>> GetAll()
        {
            var entities = await GetNonDeletedSequenced().ToListAsync();

            return entities
                .Select(mappingService.MapEntityToDto)
                .ToList();
        }

        public Task<TransactionsFilteredResultDto> GetAllFiltered(TransactionEntityFilter filter) => GetTransactionsFilteredInternal(filter);

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

            await RecalculateBalancesInternal([id]);

            await efDbTransaction.CommitAsync();
        }

        public Task<TransactionDto> Update(TransactionDto update) => UpdateInternal(update);


        public Task AdjustSequenceOfTransactionsByDate(DateOnly startDate, DateOnly endDate)
        {
            return AdjustSequenceOfTransactionsByDateInternal(startDate, endDate);
        }

        public async Task<bool> ToggleNeedsEvidence(int id)
        {
            var transaction = await dbContext.Transactions.FirstOrDefaultAsync(x => x.Id == id) ?? throw new InvalidOperationException("Cannot find transaction");
            transaction.EvidencePending = !transaction.EvidencePending;

            await dbContext.SaveChangesAsync();

            return transaction.EvidencePending;

        }

        public async Task RecalculateBalances(List<int> modifiedTransactionIds)
        {
            await RecalculateBalancesInternal(modifiedTransactionIds);

            await dbContext.SaveChangesAsync();
        }

        public async Task ReapplyMatchingCriteriaEntities(TransactionEntityFilter forFilters)
        {
            var dbTrans = await dbContext.Database.BeginTransactionAsync();

            var transactionsForFilter = await GetTransactionsFilteredInternal(forFilters);

            var allInputReferences = transactionsForFilter.Transactions.Select(it => it.Reference);
            var propertiesMap = matchingCriteriaService.GetMatchingPropertiesMapByReferences(allInputReferences);
            var categoriesMap = matchingCriteriaService.GetMatchingCategoriesMapByReferences(allInputReferences);
            foreach (var transaction in transactionsForFilter.Transactions)
            {
                var newCategories = categoriesMap[transaction.Reference];
                var newProperties = propertiesMap[transaction.Reference];

                if (!newCategories.Any() && !newProperties.Any())
                {
                    continue;
                }

                var missingCategories = newCategories.Where(nc => !transaction.Categories.Any(c => nc.Id == c.Id));
                var missingProperties = newProperties.Where(np => !transaction.Properties.Any(p => np.Id == p.Id));

                if (!missingCategories.Any() && !newProperties.Any())
                {
                    continue;
                }

                transaction.Categories.AddRange(missingCategories);
                transaction.Properties.AddRange(missingProperties);

                await Update(transaction);
            }

            await dbTrans.CommitAsync();
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

        private async Task RecalculateBalancesInternal(List<int> modifiedTransactionIds)
        {
            var earliestModifiedTransactionId = modifiedTransactionIds.Min();

            var startingBalance = dbContext
                .Transactions
                .Where(x => x.Id < earliestModifiedTransactionId && !x.IsDeleted)
                .OrderByDescending(x => x.Id)
                .Take(1)
                .FirstOrDefault()
                ?.Balance ?? 0;

            var transactionsToRecalc = dbContext
                .Transactions
                .Where(x => x.Id >= earliestModifiedTransactionId)
                .OrderBy(x => x.Id);

            decimal? rollingBalance = null;

            foreach (var transaction in transactionsToRecalc)
            {
                if (!rollingBalance.HasValue)
                {
                    rollingBalance = startingBalance;
                }

                transaction.Balance = rollingBalance.Value + transaction.Amount;
                rollingBalance = transaction.Balance;
            }
        }

        private async Task<TransactionDto> UpdateInternal(TransactionDto update)
        {
            //hacky solution for EF not exposing transaction propagation
            var isNestedTransaction = dbContext.Database.CurrentTransaction != null;
            IDbContextTransaction currentMethodTransaction = null;

            if (!isNestedTransaction)
            {
                currentMethodTransaction = await dbContext.Database.BeginTransactionAsync();
            }

            var entity = await dbContext
                .Transactions
                .FirstOrDefaultAsync(x => x.Id == update.Id)
                ?? throw new InvalidOperationException("doesnt exist");

            var balanceNeedsRecalc = update.Amount != entity.Amount;

            entity = mappingService.ProjectDtoToEntity(update, entity);

            //attach many to many
            entity = AttachManyToManyEntities(entity);
            await dbContext.SaveChangesAsync();

            if (balanceNeedsRecalc)
            {
                await RecalculateBalancesInternal([entity.Id.Value]);
            }

            if (!isNestedTransaction && currentMethodTransaction != null)
            {
                await currentMethodTransaction.CommitAsync();
                await currentMethodTransaction.DisposeAsync();
            }

            return mappingService.MapEntityToDto(entity);
        }

        private async Task<TransactionsFilteredResultDto> GetTransactionsFilteredInternal(TransactionEntityFilter filter)
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
                   (filter.CategoryIds.Contains(NoAssignedCategoryId) && !q.AssignedCategories.Any())
                   || q.AssignedCategories.Any(x => filter.CategoryIds.Contains(x.Id.Value))
               );

            var startPosition = (filter.PageNumber - 1) * filter.PageLength;

            var entities = await query.Skip(startPosition).Take(filter.PageLength).ToListAsync();
            var dtos = entities.Select(mappingService.MapEntityToDto).ToList();

            var totalCount = await query.CountAsync();

            return new(dtos, totalCount);
        }

        private IQueryable<Transaction> GetNonDeletedSequenced() => 
            dbContext
                .Transactions
                .Where(t => !t.IsDeleted)
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.Sequence);     
    }
}
