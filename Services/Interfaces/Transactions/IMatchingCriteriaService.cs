using Common.Dto.Criteria;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Transactions
{
    public interface IMatchingCriteriaService
    {
        List<TransactionCategoryDto> GetMatchingCategoriesByReference(string reference);
        Dictionary<string, List<TransactionCategoryDto>> GetMatchingCategoriesMapByReferences(IEnumerable<string> references);
        List<TransactionPropertyDto> GetMatchingPropertiesByReference(string reference);
        Dictionary<string, List<TransactionPropertyDto>> GetMatchingPropertiesMapByReferences(IEnumerable<string> references);
    }
}
