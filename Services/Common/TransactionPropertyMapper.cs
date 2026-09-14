using Common.Dto.Transactions;
using DataAccess.Models.Transactions;
using Services.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    public class TransactionPropertyMapper : ITransactionPropertyMapper
    {
        public TransactionPropertyDto MapPropertyToDto(TransactionProperty property)
        {
            return new()
            {
                Id = property.Id,
                Description = property.Description,
                CreatedOn = property.CreatedOn,
                UpdatedOn = property.UpdatedOn
            };
        }

        public TransactionProperty MapDtoToProperty(TransactionPropertyDto property)
        {
            return new TransactionProperty
            {
                Id = property.Id,
                Description = property.Description,
                CreatedOn = property.CreatedOn,
                UpdatedOn = property.UpdatedOn
            };
        }

        public TransactionProperty ProjectDtoToProperty(TransactionPropertyDto incoming, TransactionProperty entity)
        {
            entity.Id = incoming.Id;
            entity.Description = incoming.Description;
            entity.CreatedOn = incoming.CreatedOn;
            entity.UpdatedOn = incoming.UpdatedOn;
            
            return entity;
        }
    }
}
