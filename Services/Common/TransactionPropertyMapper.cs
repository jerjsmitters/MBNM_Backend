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
        public TransactionPropertyDto MapPropertyToDto(TransactionProperty property)
        {
            return new()
            {
                Id = property.Id,
                Description = property.Description,
                CreatedOn = property.CreatedOn,
                UpdatedOn = property.UpdatedOn,
                CriteriaToMatchOn = property.CriteriaToMatchOn.Select(matchingCriteriaMapper.MapMatchingCriteriaToDto).ToList()
            };
        }

        public TransactionProperty MapDtoToProperty(TransactionPropertyDto property)
        {
            return new TransactionProperty
            {
                Id = property.Id,
                Description = property.Description,
                CreatedOn = property.CreatedOn,
                UpdatedOn = property.UpdatedOn,
                CriteriaToMatchOn = property.CriteriaToMatchOn.Select(matchingCriteriaMapper.MapDtoToMatchingCriteria).ToList()
            };
        }

        public TransactionProperty ProjectDtoToProperty(TransactionPropertyDto incoming, TransactionProperty entity)
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
