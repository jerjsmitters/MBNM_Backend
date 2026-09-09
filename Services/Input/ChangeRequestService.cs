using Common.Dto.ChangeRequest;
using Common.Dto.ChangeRequest.Line;
using Common.Dto.ChangeRequest.Line.Base;
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
using Services.Interfaces.Common;
using Services.Interfaces.Input;
using Services.Interfaces.Transactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Services.Input
{
    //TBI
    public class ChangeRequestService(
        EfDbContext dbContext, 
        ITransactionMatchingService transactionMatchingService, 
        IMappingService mappingService,
        IMatchingCriteriaService matchingCriteriaService) : IChangeRequestService
    {

        public async Task<ChangeRequestDto> GenerateChangeRequestFromInputTransactions(InputTransactionsDto dataInput)
        {
            if (dataInput == null || (dataInput.Transactions?.Count ?? 0) == 0)
            {
                return new ChangeRequestDto();
            }

            var dateRange = GetDateRangeOfInputTransactions(dataInput.Transactions);

            List<TransactionDto> persistedTransactionsForDateRange = GetTransactionsForDateRange(dateRange.Item1, dateRange.Item2);

            var proposedTransactionChangeLog = GenerateChangeRequest(dataInput.Transactions, persistedTransactionsForDateRange);
            await SaveChangeRequest(proposedTransactionChangeLog);

            return proposedTransactionChangeLog;
        }

        //approval process
        public async Task<List<ChangeRequestCommitResultDto>> CommitChangeRequest(int changeRequestId)
        {
            var results = new List<ChangeRequestCommitResultDto>();
            ChangeRequestCommitResultDto result;

            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            var changeRequest = await dbContext.ChangeRequests.FindAsync(changeRequestId)
                ?? throw new InvalidOperationException($"Change Request with Id = {changeRequestId} doesn't exist.");

            var changeRequestLines = GetChangeRequestLines(changeRequest);
            if (NothingToProcess(changeRequestLines))
            {
                return [];
            }

            //add requests
            var addRequests = new List<Transaction>();
            foreach (ChangeRequestCreateLine addRequest in changeRequest.CreateLines ?? [])
            {
                var transactionToAdd = ProjectChangeRequestLineToTransaction(addRequest);
                addRequests.Add(transactionToAdd);

                result = new ChangeRequestCommitResultDto
                {
                    OperationPerformed = ChangeRequestCommitOperation.ADD,
                    TransactionBefore = null,
                    TransactionAfter = mappingService.MapTransactionToDto(transactionToAdd)
                };

                results.Add(result);
            }

            await dbContext.Transactions.AddRangeAsync(addRequests);


            //update requests
            TransactionDto transactionBeforeUpdate;
            foreach (ChangeRequestUpdateLine updateRequest in changeRequest.UpdateLines ?? [])
            {
                transactionBeforeUpdate = mappingService.MapTransactionToDto(updateRequest.Transaction);
                ProjectChangeRequestLineToTransaction(updateRequest, updateRequest.Transaction);
                updateRequest.RequestState = ChangeRequestState.COMMITED;

                result = new ChangeRequestCommitResultDto
                {
                    OperationPerformed = ChangeRequestCommitOperation.UPDATE,
                    TransactionBefore = transactionBeforeUpdate,
                    TransactionAfter = mappingService.MapTransactionToDto(updateRequest.Transaction)
                };

                results.Add(result);
            }


            //delete requests 
            foreach (var deleteRequest in changeRequest.DeleteLines ?? [])
            {
                deleteRequest.Transaction.Deleted = true;
                deleteRequest.RequestState = ChangeRequestState.COMMITED;

                result = new ChangeRequestCommitResultDto
                {
                    OperationPerformed = ChangeRequestCommitOperation.DELETE,
                    TransactionBefore = mappingService.MapTransactionToDto(deleteRequest.Transaction),
                    TransactionAfter = null
                };

                results.Add(result);
            }

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return results;
        }

        public async Task DenyChangeRequest(int changeRequestId)
        {
            var changeRequest = await dbContext.ChangeRequests.FindAsync(changeRequestId)
                ?? throw new InvalidOperationException($"Change Request with Id = {changeRequestId} doesn't exist.");

            var changeRequestLines = GetChangeRequestLines(changeRequest);
            if (NothingToProcess(changeRequestLines))
            {
                return;
            }


            foreach (var changeRequestLine in changeRequestLines)
            {
                changeRequestLine.RequestState = ChangeRequestState.DENIED;
            }

            await dbContext.SaveChangesAsync();
        }




        // ========================== PRIVATE FNS ===========================//
        private ChangeRequestDto GenerateChangeRequest(List<InputTransactionDto> inputTransactions, List<TransactionDto> persistedTransactions)
        {

            //1) Add operations
            var needToBeAdded = inputTransactions
                .Where(it => !persistedTransactions.Any(pt => transactionMatchingService.InputTransactionMatchesPersisted(it, pt)))
                .Select(
                    t => new ChangeRequestCreateLineDto 
                    {
                        ProposedAmount = t.Amount,
                        ProposedDate = t.Date,
                        ProposedReference = t.Reference,
                        ProposedSequence = t.Sequence,
                        RequestState = ChangeRequestState.PENDING,
                        ProposedCategories = matchingCriteriaService.GetMatchingCategoriesByReference(t.Reference),
                        ProposedProperties = matchingCriteriaService.GetMatchingPropertiesByReference(t.Reference)

                    }
                )
                .ToList();


            //2) Update Operations
            var needToBeUpdate = inputTransactions
                .Where(it => persistedTransactions.Any(pt => transactionMatchingService.PersistedTransactionNeedsUpdatingByInput(it, pt))) 
                .Select(
                    t => new ChangeRequestUpdateLineDto
                    {
                        ProposedAmount = t.Amount,
                        ProposedDate = t.Date,
                        ProposedReference = t.Reference,
                        ProposedSequence = t.Sequence,
                        RequestState = ChangeRequestState.PENDING,
                        ProposedCategories = matchingCriteriaService.GetMatchingCategoriesByReference(t.Reference),
                        ProposedProperties = matchingCriteriaService.GetMatchingPropertiesByReference(t.Reference)
                    }
                )
                .ToList();

            //3) persisted but values changed
            var needToDelete = persistedTransactions
                .Where(pt => !inputTransactions.Any(it => transactionMatchingService.InputTransactionMatchesPersisted(it, pt)))
                .Select(
                    t => new ChangeRequestDeleteLineDto
                    {
                        TransactionToDelete = t,
                        RequestState = ChangeRequestState.PENDING
                    }
                )
                .ToList();



            return new ChangeRequestDto
            {
                CreatedOn = DateTime.UtcNow,
                CreateLines = needToBeAdded,
                DeleteLines = needToDelete,
                UpdateLines = needToBeUpdate
            };
        }

        private async Task<ChangeRequestDto> SaveChangeRequest(ChangeRequestDto changeRequest)
        {
            if (changeRequest == null || GetChangeRequestLineDtos(changeRequest).Count == 0)
            {
                throw new ArgumentException("No change request lines" ,nameof(changeRequest));
            }

            var changeRequestEntity = mappingService.MapChangeRequestDtoToEntity(changeRequest);

            var savedRequest = await dbContext.AddAsync(changeRequestEntity);
            await dbContext.SaveChangesAsync();

            return mappingService.MapChangeRequestToDto(savedRequest.Entity);
        }



        // =========== DATA ACCESS  ===============//
        private List<TransactionDto> GetTransactionsForDateRange(DateOnly startDate, DateOnly endDate)
        {
            return dbContext
                .Transactions
                .Where(t => t.Date <= startDate && t.Date >= endDate)
                .OrderBy(t => t.Date)
                .ThenBy(t => t.Sequence)
                .Select(t => mappingService.MapTransactionToDto(t))
                .ToList();
        }

        private Transaction ProjectChangeRequestLineToTransaction(TransactionDataChangeRequestLine changeRequestLine) => ProjectChangeRequestLineToTransaction(changeRequestLine, new Transaction());


        // ============ HELPER FNS ================//
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
                .. changeRequest?.UpdateLines ?? [],
                .. changeRequest?.DeleteLines ?? []
            ];
        }

        private static List<ChangeRequestLineDto> GetChangeRequestLineDtos(ChangeRequestDto changeRequest)
        {
            return [
                .. changeRequest?.CreateLines ?? [],
                .. changeRequest?.UpdateLines ?? [],
                .. changeRequest?.DeleteLines ?? []
            ];
        }

        private static Transaction ProjectChangeRequestLineToTransaction(TransactionDataChangeRequestLine changeRequestLine, Transaction transaction)
        {
            transaction.Sequence = changeRequestLine.Sequence;
            transaction.Reference = changeRequestLine.Reference;
            transaction.Amount = changeRequestLine.Amount;
            return transaction;
        }


        private static Tuple<DateOnly, DateOnly> GetDateRangeOfInputTransactions(List<InputTransactionDto> inputs)
        {
            var startDate = inputs.Max(i => i.Date);
            var endDate = inputs.Min(i => i.Date);

            return Tuple.Create(startDate, endDate);
        }
    }
}
