using Common.Dto.Input;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Common;
using Services.Interfaces.Transactions;
using DataAccess.EF.DbContextImpl;
using System;
using Common.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Dto.Criteria;
using DataAccess.Models.Criteria;
using Services.Common;

namespace Services.Criteria
{
    public class TransactionCategoryService(EfDbContext dbContext, ITransactionCategoryMapper mappingService) : ITransactionCategoryService
    {
        public async Task<TransactionCategoryDto?> Get(int id)
        {
            var category = await FindWithCriteriaAsync(id);

            if (category == null)
            {
                return null;
            }

            return mappingService.MapCategoryToDto(category);
        }

        public async Task<List<TransactionCategoryDto>> GetAll()
        {
            var categories = await dbContext
                .TransactionCategories
                .Where(x => !x.IsDeleted)
                .Include(x => x.CriteriaToMatchOn)
                .ToListAsync();

            return categories
                .Select(mappingService.MapCategoryToDto)
                .ToList();
        }

        public async Task<TransactionCategoryDto> Add(TransactionCategoryDto transcationCategoryDto)
        {
            ValidationService.ThrowIfInvalidForCreate(transcationCategoryDto);

            var entity = mappingService.MapDtoToCategory(transcationCategoryDto);
            var result = await dbContext.AddAsync(entity);

            await dbContext.SaveChangesAsync();

            return mappingService.MapCategoryToDto(result.Entity);
        }


        public async Task<TransactionCategoryDto?> Update(TransactionCategoryDto transcationCategoryDto)
        {
            ValidationService.ThrowIfInvalidForUpdate(transcationCategoryDto);


            var category = await FindWithCriteriaAsync(transcationCategoryDto.Id.Value);
            if (category == null)
            {
                return null;
            }

            var entityWithUpdatedValues = mappingService.ProjectDtoToCategory(transcationCategoryDto, category);
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

        private Task<TransactionCategory?> FindWithCriteriaAsync(int id)
        {
            return dbContext.TransactionCategories
                .Include(x => x.CriteriaToMatchOn)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
