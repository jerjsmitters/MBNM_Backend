using Common.Dto.Api;
using Common.Dto.Base;
using Common.Dto.ChangeRequest;
using Common.Dto.ChangeRequest.Line;
using Common.Dto.Input;
using Common.Dto.Transactions;
using Common.Enums;
using Common.Extensions;
using DataAccess.EF.DbContextImpl;
using DataAccess.Models.Base;
using DataAccess.Models.ChangeRequestEntities;
using DataAccess.Models.ChangeRequestEntities.Line;
using DataAccess.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Services.Interfaces.Common;
using Services.Interfaces.Input;
using Services.Interfaces.Transactions;
using Services.Transactions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Services.Input
{
    public class ChangeRequestService(
        EfDbContext dbContext, 
        IMatchingCriteriaService matchingCriteriaService,
        IChangeRequestMapper changeRequestMapper,
        ITransactionService transactionService,
        ITransactionMapper transactionMapper) : IChangeRequestService
    {

        public async Task<ChangeRequestDto?> GetForApproval(int id)
        {
            var changeRequest = await GetInternal(id);

            if (changeRequest == null)
            {
                return null;
            }

            return changeRequestMapper.MapChangeRequestToDto(changeRequest);
        }

        public async Task<List<ChangeRequestDto>> GetAll()
        {

            var changeRequest = await dbContext
                .ChangeRequests
                .Include(cr => cr.CreateLines)
                .Include(cr => cr.DeleteLines)
                .ThenInclude(cr => cr.Transaction)
                .ToListAsync();

            if (!changeRequest.Any())
            {
                return [];
            }

            return changeRequest
                .Select(changeRequestMapper.MapChangeRequestToDto)
                .ToList();
        }

        public async Task<ChangeRequestDto> GenerateChangeRequestFromInputTransactions(InputTransactionsDto dataInput)
        {
            if (dataInput == null || (dataInput.Transactions?.Count ?? 0) == 0)
            {
                return new ChangeRequestDto();
            }

            var dateRange = GetDateRangeOfInputTransactions(dataInput.Transactions);

            List<TransactionDto> persistedTransactionsForDateRange = GetTransactionsForDateRange(dateRange.Item1, dateRange.Item2);

            var proposedTransactionChangeLog = GenerateChangeRequest(dataInput.Transactions, persistedTransactionsForDateRange);

            if (proposedTransactionChangeLog.NothingToDo)
            {
                return proposedTransactionChangeLog;
            }

            proposedTransactionChangeLog = await SaveChangeRequest(proposedTransactionChangeLog);

            return proposedTransactionChangeLog;
        }

        //approval process
        public async Task<List<ChangeRequestCommitResultDto>> ApplyChangeRequest(CommitChangeRequestDto applyChangeRequestInfo)
        {
            var results = new List<ChangeRequestCommitResultDto>();
            ChangeRequestCommitResultDto result;

            await using var efDbTransaction = await dbContext.Database.BeginTransactionAsync();

            var changeRequest = await GetInternal(applyChangeRequestInfo.ChangeRequestId)
                ?? throw new InvalidOperationException($"Change Request with Id = {applyChangeRequestInfo.ChangeRequestId} doesn't exist.");

            var changeRequestLines = GetChangeRequestLines(changeRequest);
            if (NothingToProcess(changeRequestLines))
            {
                return [];
            }

            //add requests
            var addRequests = new List<Transaction>();
            foreach (ChangeRequestCreateLine addRequest in changeRequest.CreateLines ?? [])
            {
                if (NeedToIgnoreLine(applyChangeRequestInfo.CreateLines, addRequest))
                {
                    addRequest.RequestState = ChangeRequestState.DENIED;
                    continue;
                }

                var transactionToAdd = ProjectChangeRequestLineToTransaction(addRequest);
                addRequests.Add(transactionToAdd);
                addRequest.RequestState = ChangeRequestState.COMMITED;

                result = new ChangeRequestCommitResultDto
                {
                    OperationPerformed = ChangeRequestCommitOperation.ADD,
                    TransactionBefore = null,
                    TransactionAfter = transactionMapper.MapTransactionToDto(transactionToAdd)
                };

                results.Add(result);
            }

            await dbContext.Transactions.AddRangeAsync(addRequests);

            //delete requests 
            foreach (var deleteRequest in changeRequest.DeleteLines ?? [])
            {
                if (NeedToIgnoreLine(applyChangeRequestInfo.DeleteLines, deleteRequest))
                {
                    deleteRequest.RequestState = ChangeRequestState.DENIED;
                    continue;
                }

                deleteRequest.Transaction.IsDeleted = true;
                deleteRequest.RequestState = ChangeRequestState.COMMITED;

                result = new ChangeRequestCommitResultDto
                {
                    OperationPerformed = ChangeRequestCommitOperation.DELETE,
                    TransactionBefore = transactionMapper.MapTransactionToDto(deleteRequest.Transaction),
                    TransactionAfter = null
                };

                results.Add(result);
            }


            await dbContext.SaveChangesAsync();

            //adjust sequences
            changeRequest = await RecalcPersistedTransactionSequences(changeRequest);
            await dbContext.SaveChangesAsync();

            await efDbTransaction.CommitAsync();

            return results;
        }






        // ========================== PRIVATE FNS ===========================//

        ///FOR ONE DAY ONLY
        private ChangeRequestDto GenerateChangeRequest(List<InputTransactionDto> inputTransactions, List<TransactionDto> persistedTransactions)
        {
            //cache properties and categories for later
            var allInputReferences = inputTransactions.Select(it => it.Reference);
            var propertiesMap = matchingCriteriaService.GetMatchingPropertiesMapByReferences(allInputReferences);
            var categoriesMap = matchingCriteriaService.GetMatchingCategoriesMapByReferences(allInputReferences);

            List<ChangeRequestCreateLineDto> toBeAdded = [];
            List<ChangeRequestDeleteLineDto> toBeRemoved = [];
            var sequence = 0;

            //3) 

            ChangeRequestCreateLineDto addLine;
            foreach (var transaction in inputTransactions) 
            {

                //1) input transactions that can't possibly be in persisted transactions (and thus no equality check or delete operation required)
                var needToAdd = inputTransactions.IndexOf(transaction) + 1 > persistedTransactions.Count;
                if (needToAdd)
                {
                    addLine = new ChangeRequestCreateLineDto
                    {
                        ProposedAmount = transaction.Amount,
                        ProposedDate = transaction.Date,
                        ProposedReference = transaction.Reference,
                        ProposedSequence = sequence,
                        RequestState = ChangeRequestState.PENDING,
                        ProposedCategories = categoriesMap[transaction.Reference],
                        ProposedProperties = propertiesMap[transaction.Reference]
                    };

                    toBeAdded.Add(addLine);
                    sequence++;
                    continue; 
                }

                //2) input transactions that could be the same or different to persisted and thus need equality checking plus potential delete operation
                var matchingPersistedEntity = persistedTransactions[sequence];

                var entitiesMatch = matchingPersistedEntity.GetIdentifier().Equals(transaction.GetIdentifier());
                
                if (!entitiesMatch)
                {
                    addLine = new ChangeRequestCreateLineDto
                    {
                        ProposedAmount = transaction.Amount,
                        ProposedDate = transaction.Date,
                        ProposedReference = transaction.Reference,
                        ProposedSequence = sequence,
                        RequestState = ChangeRequestState.PENDING,
                        ProposedCategories = categoriesMap[transaction.Reference],
                        ProposedProperties = propertiesMap[transaction.Reference]
                    };

                    toBeAdded.Add(addLine);

                    var deleteLine = new ChangeRequestDeleteLineDto
                    {
                        TransactionToDelete = matchingPersistedEntity,
                        RequestState = ChangeRequestState.PENDING
                    };

                    toBeRemoved.Add(deleteLine);
                }

                sequence++;              
            }

            //3) persistedd transactions that can't possibly be in inputTransactions, and thus don't need equality check or create operation
            toBeRemoved = RemoveSurplusPersistedIfApplicable(sequence, persistedTransactions, toBeRemoved);

            return new ChangeRequestDto
            {
                CreatedOn = DateTime.UtcNow,
                CreateLines = toBeAdded,
                DeleteLines = toBeRemoved
            };
        }

        private async Task<ChangeRequestDto> SaveChangeRequest(ChangeRequestDto changeRequest)
        {
            if (changeRequest == null || GetChangeRequestLineDtos(changeRequest).Count == 0)
            {
                throw new ArgumentException("No change request lines" ,nameof(changeRequest));
            }

            var changeRequestEntity = changeRequestMapper.MapDtoToChangeRequest(changeRequest);

            //attach the untracked many-to-many properties
            changeRequestEntity = AttachManyToNavigationProperties(changeRequestEntity);

            var savedRequest = await dbContext.AddAsync(changeRequestEntity);
            await dbContext.SaveChangesAsync();

            return changeRequestMapper.MapChangeRequestToDto(savedRequest.Entity);
        }


        private async Task<ChangeRequest> RecalcPersistedTransactionSequences(ChangeRequest changeRequest)
        {
            var createLinesEarliestDate = changeRequest.CreateLines.Any() 
                ? (DateOnly?)changeRequest.CreateLines.Min(x => x.Date) 
                : null;

            var createLinesLatestDate = changeRequest.CreateLines.Any()
                ? (DateOnly?)changeRequest.CreateLines.Max(x => x.Date)
                : null;

            var deleteLineEarliestDate = changeRequest.DeleteLines.Any()
                ? (DateOnly?)changeRequest.DeleteLines.Min(x => x.Transaction.Date)
                : null;

            var deleteLineLatestDate = changeRequest.DeleteLines.Any()
                ? (DateOnly?)changeRequest.DeleteLines.Max(x => x.Transaction.Date)
                : null;

            var earliestDate = new List<DateOnly?> { createLinesEarliestDate, deleteLineEarliestDate }.Where(x => x != null).Min();
            var latestDate = new List<DateOnly?> { createLinesLatestDate, deleteLineLatestDate }.Where(x => x != null).Max();

            await transactionService.AdjustSequenceOfTransactionsByDate(earliestDate.Value, latestDate.Value);
            await dbContext.SaveChangesAsync();

            return changeRequest;
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

        // =========== DATA ACCESS  ===============//
        private Task<ChangeRequest?> GetInternal(int id)
        {
            return dbContext
                .ChangeRequests
                .Include(cr => cr.CreateLines)
                .ThenInclude(cl => cl.TransactionProperties)
                .Include(cr => cr.CreateLines)
                .ThenInclude(cl => cl.TransactionCategories)
                .Include(cr => cr.DeleteLines)
                .ThenInclude(dl => dl.Transaction)
                .FirstOrDefaultAsync(cr => cr.Id == id);

        }

        private List<TransactionDto> GetTransactionsForDateRange(DateOnly startDate, DateOnly endDate)
        {
            return dbContext
                .Transactions
                .Where(t => !t.IsDeleted)
                .Where(t => t.Date <= startDate && t.Date >= endDate)
                .OrderBy(t => t.Date)
                .ThenBy(t => t.Sequence)
                .AsNoTracking()
                .Select(t => transactionMapper.MapTransactionToDto(t))
                .ToList();
        }

        private ChangeRequest AttachManyToNavigationProperties(ChangeRequest changeRequest)
        {
            //properties
            var propertyIds = changeRequest
                .CreateLines
                .SelectMany(x => x.TransactionProperties)
                .Select(x => x.Id)
                .Distinct();

            var properties = dbContext.TransactionProperties.Where(x => propertyIds.Contains(x.Id)).ToList();


            //categories
            var categoryIds = changeRequest
                .CreateLines
                .SelectMany(x => x.TransactionCategories)
                .Select(x => x.Id);

            var categories = dbContext.TransactionCategories.Where(x => categoryIds.Distinct().Contains(x.Id)).ToList();

            foreach (var createLine in changeRequest.CreateLines)
            {
                createLine.TransactionProperties = properties.Where(ap => createLine.TransactionProperties.Any(cltp => ap.Id == cltp.Id)).ToList();
                createLine.TransactionCategories = categories.Where(ac => createLine.TransactionCategories.Any(cltc => ac.Id == cltc.Id)).ToList();
            }

            var transactionIds = changeRequest.DeleteLines.Select(x => x.Transaction.Id).Distinct();
            var transactions = dbContext.Transactions.Where(x => transactionIds.Contains(x.Id));

            foreach (var deleteLine in changeRequest.DeleteLines)
            {
                deleteLine.Transaction = transactions.FirstOrDefault(ac => deleteLine.Transaction.Id == ac.Id) ?? throw new InvalidOperationException("cant find transaction to attach to delete line");
            }

            return changeRequest;
        }


        // ============ HELPER FNS ================//
        private Transaction ProjectChangeRequestLineToTransaction(TransactionDataChangeRequestLine changeRequestLine) => changeRequestMapper.ProjectChangeRequestLineToTransaction(changeRequestLine, new Transaction());

        private static bool NothingToProcess(List<ChangeRequestLine> requestLines)
        {
            if (requestLines.Count == 0)
            {
                return true;
            }

            return !requestLines.Any(crl => crl.RequestState == ChangeRequestState.PENDING);
        }

        private static List<ChangeRequestLine> GetChangeRequestLines(ChangeRequest changeRequest)
        {
            return [
                .. changeRequest?.CreateLines ?? [],
                .. changeRequest?.DeleteLines ?? []
            ];
        }

        private static List<ChangeRequestLineDto> GetChangeRequestLineDtos(ChangeRequestDto changeRequest)
        {
            return [
                .. changeRequest?.CreateLines ?? [],
                .. changeRequest?.DeleteLines ?? []
            ];
        }

        private static bool NeedToIgnoreLine(List<ApplyChangeRequestLineDto> applyChangeRequestLines, ChangeRequestLine changeRequestLine)
        {
            var matchingLine = applyChangeRequestLines.FirstOrDefault(x => x.LineId == changeRequestLine.Id) ?? throw new Exception("Cannot find line");
            return matchingLine.DesiredState == ChangeRequestState.DENIED;

        }


        private static Tuple<DateOnly, DateOnly> GetDateRangeOfInputTransactions(List<InputTransactionDto> inputs)
        {
            var startDate = inputs.Max(i => i.Date);
            var endDate = inputs.Min(i => i.Date);

            return Tuple.Create(startDate, endDate);
        }

        public bool PersistedTransactionNeedsUpdatingByInput(InputTransactionDto inputTranscation, TransactionDto persistedTransaction)
        {
            if (inputTranscation == null || persistedTransaction == null)
            {
                return false;
            }

            if (!inputTranscation.GetIdentifier().Equals(persistedTransaction.GetIdentifier()))
            {
                return false;
            }

            return inputTranscation.Reference != persistedTransaction.Reference
                    || inputTranscation.Amount != persistedTransaction.Amount;

        }

    }
}
