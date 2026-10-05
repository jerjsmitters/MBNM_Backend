using Common.Dto.Criteria;
using Common.Enums;
using DataAccess.EF.DbContextImpl;
using DataAccess.Models.Criteria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Services.Interfaces.Common;
using Services.Interfaces.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Criteria
{
    public class MatchingCriteriaService(EfDbContext dbContext, ITransactionPropertyMapper propertyMapper, ITransactionCategoryMapper categoryMapper) : IMatchingCriteriaService
    {
        public List<TransactionCategoryDto> GetMatchingCategoriesByReference(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                throw new ArgumentNullException();
            }

            var entities = GetMatchingTransactionCategoriesByReferenceQuery(reference).AsEnumerable();

            return entities
                .Select(categoryMapper.MapEntityToDto)
                .ToList();
                
        }

        public Dictionary<string, List<TransactionCategoryDto>> GetMatchingCategoriesMapByReferences(IEnumerable<string> references)
        {
            var returnVal = new Dictionary<string, List<TransactionCategoryDto>>();

            List<TransactionCategoryDto> matched;
            foreach (var reference in references) 
            {
                if (returnVal.ContainsKey(reference))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(reference))
                {
                    returnVal.Add(reference, []);
                }

                matched = GetMatchingTransactionCategoriesByReferenceQuery(reference)
                    .Select(categoryMapper.MapEntityToDto)
                    .ToList();

                returnVal.Add(reference, matched);
            }

            return returnVal;
        }

        public List<TransactionPropertyDto> GetMatchingPropertiesByReference(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                throw new ArgumentNullException();
            }

            var entities = GetMatchingTransactionPropertiesByReferenceQuery(reference).AsEnumerable();

            return entities
                .Select(propertyMapper.MapEntityToDto)
                .ToList();
        }

        public Dictionary<string, List<TransactionPropertyDto>> GetMatchingPropertiesMapByReferences(IEnumerable<string> references)
        {
            var returnVal = new Dictionary<string, List<TransactionPropertyDto>>();

            List<TransactionPropertyDto> matched;
            foreach (var reference in references)
            {
                if (returnVal.ContainsKey(reference))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(reference))
                {
                    returnVal.Add(reference, []);
                }

                matched = GetMatchingTransactionPropertiesByReferenceQuery(reference)
                    .Select(propertyMapper.MapEntityToDto)
                    .ToList();

                returnVal.Add(reference, matched);
            }

            return returnVal;
        }

        private IQueryable<TransactionCategory> GetMatchingTransactionCategoriesByReferenceQuery(string reference)
        {
            return dbContext
                .MatchingCriteria
                .Include(x => x.TransactionCategories)
                .Where(x => x.MatchingCriteriaMode == MatchingCriteriaMode.CONTAINS && reference.Contains(x.SearchText))
                .SelectMany(x => x.TransactionCategories)
                .Distinct()
                .Include(x => x.CriteriaToMatchOn)
                .AsNoTracking();
        }

        private IQueryable<TransactionProperty> GetMatchingTransactionPropertiesByReferenceQuery(string reference)
        {
            return dbContext
                .MatchingCriteria
                .Include(x => x.TransactionProperties)
                .Where(x => x.MatchingCriteriaMode == MatchingCriteriaMode.CONTAINS && reference.Contains(x.SearchText))
                .SelectMany(x => x.TransactionProperties)
                .Distinct()
                .Include(x => x.CriteriaToMatchOn)
                .AsNoTracking();
        }
    }
}
