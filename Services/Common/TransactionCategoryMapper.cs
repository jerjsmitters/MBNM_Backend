using Common.Dto.Criteria;
using DataAccess.Models.Criteria;
using Services.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    public class TransactionCategoryMapper(IMatchingCriteriaMapper matchingCriteriaMapper) : ITransactionCategoryMapper
    {
        public TransactionCategoryDto MapEntityToDto(TransactionCategory category)
        {
            return new()
            {
                Id = category.Id,
                Description = category.Description,
                CriteriaToMatchOn = category.CriteriaToMatchOn?.Select(matchingCriteriaMapper.MapEntityToDto).ToList() ?? [],
                
                CreatedOn = category.CreatedOn,
                UpdatedOn = category.UpdatedOn,
                IsDeleted = category.IsDeleted
            };
        }

        public TransactionCategory MapDtoToEntity(TransactionCategoryDto category)
        {
            return new()
            {
                Id = category.Id,
                Description = category.Description,
                CriteriaToMatchOn = category.CriteriaToMatchOn?.Select(matchingCriteriaMapper.MapDtoToEntity).ToList() ?? [], //UNTRACKED

                CreatedOn = category.CreatedOn,
                UpdatedOn = category.UpdatedOn,
                IsDeleted = category.IsDeleted
            };
        }

        public TransactionCategory ProjectDtoToEntity(TransactionCategoryDto incoming, TransactionCategory entity)
        {
            entity.Id = incoming.Id;
            entity.Description = incoming.Description;
            matchingCriteriaMapper.ProjectDtoToEntityWithCriteria(incoming, entity);

            return entity;
        }
    }
}
