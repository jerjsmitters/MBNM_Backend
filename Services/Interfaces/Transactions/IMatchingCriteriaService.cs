using Common.Dto.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Transactions
{
    public interface IMatchingCriteriaService
    {
        List<TransactionCategoryDto> GetMatchingCategoriesByReference(string reference);
        List<TransactionPropertyDto> GetMatchingPropertiesByReference(string reference);

    }
}
