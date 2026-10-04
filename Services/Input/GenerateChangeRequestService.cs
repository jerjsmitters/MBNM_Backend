using Common.Dto.ChangeRequest;
using Common.Dto.ChangeRequest.Line;
using Common.Dto.Criteria;
using Common.Dto.Input;
using Common.Dto.Transactions;
using Common.Enums;
using Services.Criteria;
using Services.Interfaces.Input;
using Services.Interfaces.Transactions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Services.Input
{
    public class GenerateChangeRequestService(IMatchingCriteriaService matchingCriteriaService) : IGenerateChangeRequestService
    {
        public ChangeRequestDto GenerateChangeRequest(List<InputTransactionDto> inputTransactions, List<TransactionDto> persistedTransactionsForDateRange)
        {
            var changeRequest = new ChangeRequestDto
            {
                CreatedOn = DateTime.UtcNow,
                CreateLines = [],
                DeleteLines = []
            };

            // map the values by date
            var inputTransactionsByDateMap = GetInputTransactionByDateMap(inputTransactions);
            var persistedTransactionForDateRangeByDateMap = GetPersistedTransactionByDateMap(persistedTransactionsForDateRange);

            var startDate = inputTransactions.Min(x => x.Date);
            var endDate = inputTransactions.Max(x => x.Date);
            var dateRange = GetDateRange(startDate, endDate);

            //cache properties and categories for later
            var allInputReferences = inputTransactions.Select(it => it.Reference);
            var propertiesMap = matchingCriteriaService.GetMatchingPropertiesMapByReferences(allInputReferences);
            var categoriesMap = matchingCriteriaService.GetMatchingCategoriesMapByReferences(allInputReferences);

            foreach(var date in dateRange)
            {
                var inputTransactionsForDate = inputTransactionsByDateMap.TryGetValue(date, out List<InputTransactionDto>? value) ? value : [];
                var persistedTransactionForDate = persistedTransactionForDateRangeByDateMap.TryGetValue(date, out List<TransactionDto>? value1) ? value1 : [];
                var results = GenerateChangeRequestForDate(date, inputTransactionsForDate, persistedTransactionForDate, categoriesMap, propertiesMap);

                changeRequest.CreateLines.AddRange(results.CreateLines);
                changeRequest.DeleteLines.AddRange(results.DeleteLines);
            }

            return changeRequest;
        }


        private ChangeRequestForDate GenerateChangeRequestForDate(
             DateOnly date, List<InputTransactionDto> inputTransactions,
             List<TransactionDto> persistedTransactions,
             Dictionary<string, List<TransactionCategoryDto>> cachedCategoryReferenceMap,
             Dictionary<string, List<TransactionPropertyDto>> cachedPropertyReferenceMap
         )
        {
            List<ChangeRequestCreateLineDto> toBeAdded = [];
            List<ChangeRequestDeleteLineDto> toBeRemoved = [];

            var sequenceForDate = 0;

            //3) 
            ChangeRequestCreateLineDto addLine;
            foreach (var transaction in inputTransactions)
            {
                //1) input transactions that can't possibly be in persisted transactions (and thus no equality check or delete operation required)
                // as we have reached the end of the transactions for the date range that already exist in the DB
                var needToAdd = inputTransactions.IndexOf(transaction) + 1 > persistedTransactions.Count;
                if (needToAdd)
                {
                    addLine = new ChangeRequestCreateLineDto
                    {
                        ProposedAmount = transaction.Amount,
                        ProposedDate = transaction.Date,
                        ProposedReference = transaction.Reference,
                        ProposedSequence = sequenceForDate,
                        RequestState = ChangeRequestState.PENDING,
                        ProposedCategories = cachedCategoryReferenceMap[transaction.Reference],
                        ProposedProperties = cachedPropertyReferenceMap[transaction.Reference]
                    };

                    toBeAdded.Add(addLine);

                    sequenceForDate++;

                    continue;
                }

                //2) input transactions that could be the same or different to persisted and thus need equality checking plus potential delete operation
                // as the input transaction we are on
                var matchingPersistedEntity = persistedTransactions[sequenceForDate];

                var entitiesMatch = matchingPersistedEntity.GetIdentifier().Equals(transaction.GetIdentifier());

                if (!entitiesMatch)
                {
                    addLine = new ChangeRequestCreateLineDto
                    {
                        ProposedAmount = transaction.Amount,
                        ProposedDate = transaction.Date,
                        ProposedReference = transaction.Reference,
                        ProposedSequence = sequenceForDate,
                        RequestState = ChangeRequestState.PENDING,
                        ProposedCategories = cachedCategoryReferenceMap[transaction.Reference],
                        ProposedProperties = cachedPropertyReferenceMap[transaction.Reference]
                    };

                    toBeAdded.Add(addLine);

                    var deleteLine = new ChangeRequestDeleteLineDto
                    {
                        TransactionToDelete = matchingPersistedEntity,
                        RequestState = ChangeRequestState.PENDING
                    };

                    toBeRemoved.Add(deleteLine);
                }

                sequenceForDate++;
            }

            //3) persistedd transactions that can't possibly be in inputTransactions, and thus don't need equality check or create operation
            toBeRemoved = RemoveSurplusPersistedIfApplicable(sequenceForDate, persistedTransactions, toBeRemoved);

            return new(date, toBeAdded, toBeRemoved);
        }

        private ImmutableArray<DateOnly> GetDateRange(DateOnly startDate, DateOnly endDate)
        {
            DateOnly date = startDate;
            List <DateOnly> dateRange = [];

            while(date <= endDate)
            {
                dateRange.Add(date);
                date = date.AddDays(1);
            }

            return dateRange.ToImmutableArray();
        }



        private Dictionary<DateOnly, List<InputTransactionDto>> GetInputTransactionByDateMap(List<InputTransactionDto> inputTransactions)
        {
            var returnValue = new Dictionary<DateOnly, List<InputTransactionDto>>();

            foreach (var transaction in inputTransactions)
            {
                if (returnValue.TryGetValue(transaction.Date, out List<InputTransactionDto>? value))
                {
                    value.Add(transaction);
                }
                else
                {
                    returnValue.Add(transaction.Date, [ transaction ]);
                }
            }

            return returnValue;
        }

        private Dictionary<DateOnly, List<TransactionDto>> GetPersistedTransactionByDateMap(List<TransactionDto> persistedTransactions)
        {
            var returnValue = new Dictionary<DateOnly, List<TransactionDto>>();

            foreach (var transaction in persistedTransactions)
            {
                if (returnValue.TryGetValue(transaction.Date, out List<TransactionDto>? value))
                {
                    value.Add(transaction);
                }
                else
                {
                    returnValue.Add(transaction.Date, [transaction]);
                }
            }

            return returnValue;
        }


        private List<ChangeRequestDeleteLineDto> RemoveSurplusPersistedIfApplicable(int finalSequence, List<TransactionDto> persistedTransactions, List<ChangeRequestDeleteLineDto> generatedDeleteLines)
        {
            var oneBasedFinalSequence = finalSequence + 1;
            if (oneBasedFinalSequence >= persistedTransactions.Count)
            {
                return generatedDeleteLines;
            }

            var toDelete = persistedTransactions.Skip(finalSequence).ToList();
            toDelete.ForEach(td =>
            {
                var deleteLine = new ChangeRequestDeleteLineDto
                {
                    TransactionToDelete = td,
                    RequestState = ChangeRequestState.PENDING
                };

                generatedDeleteLines.Add(deleteLine);
            });

            return generatedDeleteLines;
        }



        // ================== CLASS ONLY DTOS =======================//
        private record ChangeRequestForDate(DateOnly Date, List<ChangeRequestCreateLineDto> CreateLines, List<ChangeRequestDeleteLineDto> DeleteLines);

    }
}
