using Common.Dto.ChangeRequest;
using Common.Dto.Transactions;
using DataAccess.Models.ChangeRequestEntities;
using DataAccess.Models.Transactions;
using Services.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    public class MappingService : IMappingService
    {
        public TransactionDto MapTransactionToDto(Transaction transaction)
        {
            return new TransactionDto
            {
                Id = transaction.Id,
                CreatedOn = transaction.CreatedOn,
                Date = transaction.Date,
                Amount = transaction.Amount
            };
        }


        public ChangeRequest MapChangeRequestDtoToEntity(ChangeRequestDto changeRequest)
        {
            return null;
        }

        public ChangeRequestDto MapChangeRequestToDto(ChangeRequest changeRequest)
        {
            return null;
        }

        public TransactionCategoryDto MapCategoryToDto(TransactionCategory category)
        {
            throw new NotImplementedException();
        }

        public TransactionPropertyDto MapPropertyToDto(TransactionProperty property)
        {
            throw new NotImplementedException();
        }
    }
}
