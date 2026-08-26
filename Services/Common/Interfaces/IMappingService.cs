using Common.Dto;
using Common.Dto.ChangeRequest;
using DataAccess.Models;
using DataAccess.Models.ChangeRequest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common.Interfaces
{
    public interface IMappingService
    {
        ChangeRequest MapChangeRequestDtoToEntity(ChangeRequestDto changeRequest);
        ChangeRequestDto MapChangeRequestToDto(ChangeRequest changeRequest);
        TransactionDto MapTransactionToDto(Transaction transaction);
    }
}
