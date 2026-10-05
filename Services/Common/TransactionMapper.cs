using Common.Dto.Transactions;
using DataAccess.Models.Transactions;
using Services.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    public class TransactionMapper(ITransactionCategoryMapper categoryMapper, ITransactionPropertyMapper propertyMapper, ITransactionEvidenceMapper transactionEvidenceMapper) : ITransactionMapper
    {
        public Transaction MapDtoToEntity(TransactionDto transaction)
        {
            return new()
            {
                Id = transaction.Id,
                Reference = transaction.Reference,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Sequence = transaction.Sequence,
                EvidencePending = transaction.EvidencePending,
                Evidence = transaction.Evidence?.Select(transactionEvidenceMapper.MapDtoToEntity).ToList() ?? [], //UNTRACKED

                AssignedCategories = transaction.Categories?.Select(categoryMapper.MapDtoToEntity).ToList() ?? [], //UNTRACKED
                AssignedProperties = transaction.Properties?.Select(propertyMapper.MapDtoToEntity).ToList() ?? [], //UNTRACKED

                CreatedOn = transaction.CreatedOn,
                UpdatedOn = transaction.UpdatedOn,
                IsDeleted = transaction.IsDeleted
            };
        }

        public TransactionDto MapEntityToDto(Transaction transaction)
        {
            return new TransactionDto
            {
                Id = transaction.Id,
                Reference = transaction.Reference,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Sequence = transaction.Sequence,
                EvidencePending = transaction.EvidencePending,
                Evidence = transaction.Evidence?.Select(transactionEvidenceMapper.MapEntityToDto).ToList() ?? [],

                Categories = transaction.AssignedCategories?.Select(categoryMapper.MapEntityToDto).ToList() ?? [],
                Properties = transaction.AssignedProperties?.Select(propertyMapper.MapEntityToDto).ToList() ?? [],

                CreatedOn = transaction.CreatedOn,
                UpdatedOn = transaction.UpdatedOn,
                IsDeleted = transaction.IsDeleted
            };
        }

        public Transaction ProjectDtoToEntity(TransactionDto incoming, Transaction entity)
        {
            entity.Amount = incoming.Amount;
            entity.Date = incoming.Date;
            entity.Reference = incoming.Reference;
            entity.Sequence = incoming.Sequence;
            entity.EvidencePending = incoming.EvidencePending;

            entity.AssignedCategories = incoming.Categories.Select(categoryMapper.MapDtoToEntity).ToList(); //untracked
            entity.AssignedProperties = incoming.Properties.Select(propertyMapper.MapDtoToEntity).ToList(); //untracked

            return entity;
        }
    }
}
