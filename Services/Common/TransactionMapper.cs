using Common.Dto.Transactions;
using DataAccess.Models.Transactions;
using Services.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    public class TransactionMapper(ITransactionCategoryMapper categoryMapper, ITransactionPropertyMapper propertyMapper) : ITransactionMapper
    {
        public Transaction MapDtoToTransaction(TransactionDto transaction)
        {
            return new()
            {
                Id = transaction.Id,
                Reference = transaction.Reference,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Sequence = transaction.Sequence,

                AssignedCategories = transaction.Categories.Select(categoryMapper.MapDtoToCategory).ToList(), //UNTRACKED
                AssignedProperties = transaction.Properties.Select(propertyMapper.MapDtoToProperty).ToList(), //UNTRACKED

                CreatedOn = transaction.CreatedOn,
                UpdatedOn = transaction.UpdatedOn
            };
        }

        public TransactionDto MapTransactionToDto(Transaction transaction)
        {
            return new TransactionDto
            {
                Id = transaction.Id,
                Reference = transaction.Reference,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Sequence = transaction.Sequence,

                Categories = transaction.AssignedCategories.Select(categoryMapper.MapCategoryToDto).ToList(),
                Properties = transaction.AssignedProperties.Select(propertyMapper.MapPropertyToDto).ToList(),

                CreatedOn = transaction.CreatedOn,
                UpdatedOn= transaction.UpdatedOn
            };
        }

        public Transaction ProjectDtoToTransaction(TransactionDto incoming, Transaction entity)
        {
            entity.Amount = incoming.Amount;
            entity.Date = incoming.Date;
            entity.Reference = incoming.Reference;
            entity.Sequence = incoming.Sequence;

            entity.AssignedCategories = incoming.Categories.Select(categoryMapper.MapDtoToCategory).ToList(); //untracked
            entity.AssignedProperties = incoming.Properties.Select(propertyMapper.MapDtoToProperty).ToList(); //untracked

            return entity;
        }
    }
}
