using Common.Dto.Transactions;
using DataAccess.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Common
{
    public interface ITransactionCategoryMapper
    {
        TransactionCategoryDto MapCategoryToDto(TransactionCategory category);
        TransactionCategory MapDtoToCategory(TransactionCategoryDto category);
        TransactionCategory ProjectDtoToCategory(TransactionCategoryDto incoming, TransactionCategory entity);
    }
}
