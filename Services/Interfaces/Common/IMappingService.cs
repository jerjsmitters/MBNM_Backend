using Common.Dto.ChangeRequest;
using Common.Dto.Transactions;
using DataAccess.Models.Base;
using DataAccess.Models.ChangeRequestEntities;
using DataAccess.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Common
{
    public interface IMappingService
    {
        ChangeRequest MapDtoToChangeRequest(ChangeRequestDto changeRequest);
        ChangeRequestDto MapChangeRequestToDto(ChangeRequest changeRequest);
        Transaction ProjectChangeRequestLineToTransaction(TransactionDataChangeRequestLine changeRequestLine, Transaction transaction);


        TransactionDto MapTransactionToDto(Transaction transaction);


        TransactionPropertyDto MapPropertyToDto(TransactionProperty property);
        TransactionProperty MapDtoToProperty(TransactionPropertyDto property);
        TransactionProperty ProjectDtoToProperty(TransactionPropertyDto incoming, TransactionProperty entity);


        TransactionCategoryDto MapCategoryToDto(TransactionCategory category);
        TransactionCategory MapDtoToCategory(TransactionCategoryDto category);
        TransactionCategory ProjectDtoToCategory(TransactionCategoryDto incoming, TransactionCategory entity);
    }
}
