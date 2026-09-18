using Common.Dto.Criteria;
using DataAccess.Models.Criteria;
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
