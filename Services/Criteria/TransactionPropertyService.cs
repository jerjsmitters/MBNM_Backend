using Common.Dto.Criteria;
using Common.Dto.Transactions;
using Common.Enums;
using DataAccess.EF.DbContextImpl;
using DataAccess.Models.Criteria;
using Microsoft.EntityFrameworkCore;
using Services.Common;
using Services.Interfaces.Common;
using Services.Interfaces.Transactions;

namespace Services.Criteria
{
    public class TransactionPropertyService(EfDbContext dbContext, ITransactionPropertyMapper mappingService) : ITransactionPropertyService
    {
        public async Task<TransactionPropertyDto?> Get(int id)
        {
            var property = await FindWithCriteriaAsync(id);

            if (property == null) 
            {
                return null;
            }

            return mappingService.MapPropertyToDto(property);
        }

        public async Task<List<TransactionPropertyDto>> GetAll()
        {
            var properties = await dbContext
                .TransactionProperties
                .Where(x => !x.IsDeleted)
                .Include(t => t.CriteriaToMatchOn)
                .ToListAsync();

            return properties
                .Select(mappingService.MapPropertyToDto)
                .ToList();
        }

        public async Task<TransactionPropertyDto> Add(TransactionPropertyDto transactionDto)
        {
            ValidationService.ThrowIfInvalidForCreate(transactionDto);

            var entity = mappingService.MapDtoToProperty(transactionDto);
            var result = await dbContext.AddAsync(entity);

            await dbContext.SaveChangesAsync();

            return mappingService.MapPropertyToDto(result.Entity);
        }


        public async Task<TransactionPropertyDto?> Update(TransactionPropertyDto updatedPropertyDto)
        {
            ValidationService.ThrowIfInvalidForCreate(updatedPropertyDto);

            var property = await FindWithCriteriaAsync(updatedPropertyDto.Id.Value);

            if (updatedPropertyDto == null)
            {
                return null;
            }

            var entityWithUpdatedValues = mappingService.ProjectDtoToProperty(updatedPropertyDto, property);
            await dbContext.SaveChangesAsync();

            return mappingService.MapPropertyToDto(entityWithUpdatedValues);
        }

        public async Task Delete(int id)
        {
            var entity = await dbContext.TransactionProperties.FindAsync(id);
            if (entity == null)
            {
                return;
            }

            entity.IsDeleted = true;
            await dbContext.SaveChangesAsync();
        }

        private Task<TransactionProperty?> FindWithCriteriaAsync(int id)
        {
            return dbContext
                .TransactionProperties
                .Include(tp => tp.CriteriaToMatchOn)
                .FirstOrDefaultAsync(tp => tp.Id == id);
        }
    }
}
