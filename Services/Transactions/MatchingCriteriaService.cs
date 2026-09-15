using Common.Dto.Transactions;
using Common.Enums;
using DataAccess.EF.DbContextImpl;
using Services.Interfaces.Common;
using Services.Interfaces.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Transactions
{
    public class MatchingCriteriaService(ITransactionPropertyMapper propertyMapper, ITransactionCategoryMapper categoryMapper, EfDbContext dbContext) : IMatchingCriteriaService
    {
        public List<TransactionCategoryDto> GetMatchingCategoriesByReference(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                throw new ArgumentNullException();
            }

            return dbContext
                .MatchingCriteria
                .Where(x => x.TransactionCategories.Any())
                .Where(x => x.MatchingCriteriaMode == MatchingCriteriaMode.CONTAINS && reference.Contains(x.SearchText))
                .SelectMany(x => x.TransactionCategories)
                .Distinct()
                .Select(x => categoryMapper.MapCategoryToDto(x))
                .ToList();
        }

        public List<TransactionPropertyDto> GetMatchingPropertiesByReference(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                throw new ArgumentNullException();
            }

            return dbContext
                .MatchingCriteria
                .Where(x => x.TransactionProperties.Any())
                .Where(x => x.MatchingCriteriaMode == MatchingCriteriaMode.CONTAINS && reference.Contains(x.SearchText))
                .SelectMany(x => x.TransactionProperties)
                .Distinct()
                .Select(x => propertyMapper.MapPropertyToDto(x))
                .ToList();
        }
    }
}
