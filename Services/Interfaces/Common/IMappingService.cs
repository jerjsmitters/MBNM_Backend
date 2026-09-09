using Common.Dto.ChangeRequest;
using Common.Dto.Transactions;
using DataAccess.Models.ChangeRequestEntities;
using DataAccess.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Common
{
    public interface IMappingService
    {
        ChangeRequest MapChangeRequestDtoToEntity(ChangeRequestDto changeRequest);
        ChangeRequestDto MapChangeRequestToDto(ChangeRequest changeRequest);
        TransactionDto MapTransactionToDto(Transaction transaction);
        TransactionCategoryDto MapCategoryToDto(TransactionCategory category);
        TransactionPropertyDto MapPropertyToDto(TransactionProperty property);
    }
}
