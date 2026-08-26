using Common.Dto;
using Common.Dto.ChangeRequest;
using DataAccess.Models;
using DataAccess.Models.ChangeRequest;
using Services.Common.Interfaces;
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
    }
}
