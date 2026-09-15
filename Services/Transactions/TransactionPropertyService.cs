using Common.Dto.Transactions;
using Common.Enums;
using DataAccess.EF.DbContextImpl;
using DataAccess.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces.Common;
using Services.Interfaces.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Transactions
{
    public class TransactionPropertyService(EfDbContext dbContext, ITransactionPropertyMapper mappingService) : ITransactionPropertyService
    {
        public async Task<TransactionPropertyDto?> Get(int id)
        {
            var property = await dbContext.TransactionProperties.FindAsync(id);
            if (property == null) 
            {
                return null;
            }

            return mappingService.MapPropertyToDto(property);

        }

        public async Task<List<TransactionPropertyDto>> GetAll()
        {
            var properties = await dbContext.TransactionProperties.ToListAsync();

            return properties
                .Select(mappingService.MapPropertyToDto)
                .ToList();
        }

        public async Task<TransactionPropertyDto> Add(TransactionPropertyDto transactionDto)
        {
            if (transactionDto.Id.HasValue)
            {
                throw new Exception("Cannot add an entity that already has an ID");
            }

            var entity = mappingService.MapDtoToProperty(transactionDto);
            var result = await dbContext.AddAsync(entity);

            await dbContext.SaveChangesAsync();

            return mappingService.MapPropertyToDto(result.Entity);
        }


        public async Task<TransactionPropertyDto?> Update(TransactionPropertyDto transactionDto)
        {
            if (!transactionDto.Id.HasValue)
            {
                throw new Exception("Cannot update an entity without an ID");
            }

            var entity = await dbContext.TransactionProperties.FindAsync(transactionDto.Id.Value);
            if (entity == null)
            {
                return null;
            }

            var entityWithUpdatedValues = mappingService.ProjectDtoToProperty(transactionDto, entity);
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
    }
}
