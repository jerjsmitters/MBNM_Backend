using Common.Dto.Criteria;
using DataAccess.Models.Criteria;
using Services.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    public class TransactionPropertyMapper(IMatchingCriteriaMapper matchingCriteriaMapper) : ITransactionPropertyMapper
    {
        public TransactionPropertyDto MapEntityToDto(TransactionProperty property)
        {
            return new()
            {
                Id = property.Id,
                Description = property.Description,
                CriteriaToMatchOn = property.CriteriaToMatchOn?.Select(matchingCriteriaMapper.MapEntityToDto).ToList() ?? [],

                CreatedOn = property.CreatedOn,
                UpdatedOn = property.UpdatedOn,
                IsDeleted = property.IsDeleted
            };
        }

        public TransactionProperty MapDtoToEntity(TransactionPropertyDto property)
        {
            return new TransactionProperty
            {
                Id = property.Id,
                Description = property.Description,
                CriteriaToMatchOn = property.CriteriaToMatchOn?.Select(matchingCriteriaMapper.MapDtoToEntity).ToList() ?? [], //UNTRACKED

                CreatedOn = property.CreatedOn,
                UpdatedOn = property.UpdatedOn,
                IsDeleted = property.IsDeleted
            };
        }

        public TransactionProperty ProjectDtoToEntity(TransactionPropertyDto incoming, TransactionProperty entity)
        {
            entity.Id = incoming.Id;
            entity.Description = incoming.Description;
            matchingCriteriaMapper.ProjectDtoToEntityWithCriteria(incoming, entity);

            return entity;
        }
    }
}
