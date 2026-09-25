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
        public TransactionCategoryDto MapCategoryToDto(TransactionCategory category)
        {
            return new()
            {
                CreatedOn = category.CreatedOn,
                Id = category.Id,
                UpdatedOn = category.UpdatedOn,
                Description = category.Description,
                CriteriaToMatchOn = category.CriteriaToMatchOn?.Select(matchingCriteriaMapper.MapMatchingCriteriaToDto).ToList() ?? []
            };
        }

        public TransactionCategory MapDtoToCategory(TransactionCategoryDto category)
        {
            return new()
            {
                Id = category.Id,
                Description = category.Description,
                CreatedOn = category.CreatedOn,
                UpdatedOn = category.UpdatedOn,
                CriteriaToMatchOn = category.CriteriaToMatchOn?.Select(matchingCriteriaMapper.MapDtoToMatchingCriteria).ToList() ?? []
            };
        }

        public TransactionCategory ProjectDtoToCategory(TransactionCategoryDto incoming, TransactionCategory entity)
        {
            entity.Id = incoming.Id;
            entity.Description = incoming.Description;
            entity.CreatedOn = incoming.CreatedOn;
            entity.UpdatedOn = incoming.UpdatedOn;

            matchingCriteriaMapper.ProjectDtoToEntityWithCriteria(incoming, entity);

            return entity;
        }
    }
}
