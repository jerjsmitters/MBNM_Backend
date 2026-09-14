using Common.Dto.Input;
using Common.Dto.Transactions;
using DataAccess.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Common;
using Services.Interfaces.Transactions;
using DataAccess.EF.DbContextImpl;
using System;
using Common.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Transactions
{
    public class TransactionCategoryService(EfDbContext dbContext, IMappingService mappingService) : ITransactionCategoryService
    {
        public async Task<TransactionCategoryDto?> Get(int id)
        {
            var category = await dbContext.TransactionCategories.FindAsync(id);
            if (category == null)
            {
                return null;
            }

            return mappingService.MapCategoryToDto(category);
        }

        public async Task<List<TransactionCategoryDto>> GetAll()
        {
            var categories = await dbContext.TransactionCategories.ToListAsync();

            return categories
                .Select(mappingService.MapCategoryToDto)
                .ToList();
        }

        public List<TransactionCategoryDto> GetTransactionCategoriesByReference(string reference)
        {
            return dbContext
                .TransactionCategories
                .Where(tc => tc.MatchingCriteria.Any(mc => mc.MatchingCriteriaMode == MatchingCriteriaMode.CONTAINS && reference.Contains(mc.SearchText)))
                .Select(mappingService.MapCategoryToDto)
                .ToList();
        }

        public async Task<TransactionCategoryDto> Add(TransactionCategoryDto transactionDto)
        {
            if (transactionDto.Id.HasValue)
            {
                throw new Exception("Cannot add an entity that already has an ID");
            }

            var entity = mappingService.MapDtoToCategory(transactionDto);
            var result = await dbContext.AddAsync(entity);

            await dbContext.SaveChangesAsync();

            return mappingService.MapCategoryToDto(result.Entity);
        }


        public async Task<TransactionCategoryDto?> Update(TransactionCategoryDto transactionDto)
        {
            if (!transactionDto.Id.HasValue)
            {
                throw new Exception("Cannot update an entity without an ID");
            }

            var entity = await dbContext.TransactionCategories.FindAsync(transactionDto.Id.Value);
            if (entity == null)
            {
                return null;
            }

            var entityWithUpdatedValues = mappingService.ProjectDtoToCategory(transactionDto, entity);
            await dbContext.SaveChangesAsync();

            return mappingService.MapCategoryToDto(entityWithUpdatedValues);
        }

        public async Task Delete(int id)
        {
            var entity = await dbContext.TransactionCategories.FindAsync(id);
            if (entity == null)
            {
                return;
            }

            entity.IsDeleted = true;
            await dbContext.SaveChangesAsync();
        }
    }
}
