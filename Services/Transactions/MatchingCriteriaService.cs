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
    public class MatchingCriteriaService(IMappingService mappingService, EfDbContext dbContext) : IMatchingCriteriaService
    {
        public List<TransactionCategoryDto> GetMatchingCategoriesByReference(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                throw new ArgumentNullException();
            }

            var matchingCriteriaWithCategories = dbContext
                .MatchingCriteria
                .Where(x => x.TransactionCategories.Any());

            return matchingCriteriaWithCategories
                .Where(x => x.MatchingCriteriaMode == MatchingCriteriaMode.CONTAINS && reference.Contains(x.SearchText))
                .SelectMany(x => x.TransactionCategories)
                .Select(x => mappingService.MapCategoryToDto(x))
                .Distinct()
                .ToList();

        }

        public List<TransactionPropertyDto> GetMatchingPropertiesByReference(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                throw new ArgumentNullException();
            }

            var matchingCriteriaWithCategories = dbContext
                .MatchingCriteria
                .Where(x => x.TransactionCategories.Any());

            return matchingCriteriaWithCategories
                .Where(x => x.MatchingCriteriaMode == MatchingCriteriaMode.CONTAINS && reference.Contains(x.SearchText))
                .SelectMany(x => x.TransactionProperties)
                .Select(x => mappingService.MapPropertyToDto(x))
                .Distinct()
                .ToList();

        }


    }
}
