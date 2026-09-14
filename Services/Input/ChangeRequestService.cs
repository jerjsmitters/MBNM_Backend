using Common.Dto.Api;
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
using Services.Transactions;
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
        ITransactionPropertyService propertyService,
        ITransactionCategoryService categoryService,
        IChangeRequestMapper changeRequestMapper,
        ITransactionMapper transactionMapper) : IChangeRequestService
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
        public async Task<List<ChangeRequestCommitResultDto>> ApplyChangeRequest(ApplyChangeRequestDto applyChangeRequestInfo)
        {
            var results = new List<ChangeRequestCommitResultDto>();
            ChangeRequestCommitResultDto result;

            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            var changeRequest = await dbContext.ChangeRequests.FindAsync(applyChangeRequestInfo.ChangeRequestId)
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

                result = new ChangeRequestCommitResultDto
                {
                    OperationPerformed = ChangeRequestCommitOperation.ADD,
                    TransactionBefore = null,
                    TransactionAfter = transactionMapper.MapTransactionToDto(transactionToAdd)
                };

                results.Add(result);
            }

            await dbContext.Transactions.AddRangeAsync(addRequests);


            //update requests
            TransactionDto transactionBeforeUpdate;
            foreach (ChangeRequestUpdateLine updateRequest in changeRequest.UpdateLines ?? [])
            {
                if (NeedToIgnoreLine(applyChangeRequestInfo.CreateLines, updateRequest))
                {
                    updateRequest.RequestState = ChangeRequestState.DENIED;
                    continue;
                }

                transactionBeforeUpdate = transactionMapper.MapTransactionToDto(updateRequest.Transaction);
                changeRequestMapper.ProjectChangeRequestLineToTransaction(updateRequest, updateRequest.Transaction);
                updateRequest.RequestState = ChangeRequestState.COMMITED;

                result = new ChangeRequestCommitResultDto
                {
                    OperationPerformed = ChangeRequestCommitOperation.UPDATE,
                    TransactionBefore = transactionBeforeUpdate,
                    TransactionAfter = transactionMapper.MapTransactionToDto(updateRequest.Transaction)
                };

                results.Add(result);
            }


            //delete requests 
            foreach (var deleteRequest in changeRequest.DeleteLines ?? [])
            {
                if (NeedToIgnoreLine(applyChangeRequestInfo.CreateLines, deleteRequest))
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
            await transaction.CommitAsync();

            return results;
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
                        ProposedCategories = categoryService.GetTransactionCategoriesByReference(t.Reference),
                        ProposedProperties = propertyService.GetTransactionPropertiesByReference(t.Reference)

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
                        ProposedCategories = categoryService.GetTransactionCategoriesByReference(t.Reference),
                        ProposedProperties = propertyService.GetTransactionPropertiesByReference(t.Reference)
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

            var changeRequestEntity = changeRequestMapper.MapDtoToChangeRequest(changeRequest);

            var savedRequest = await dbContext.AddAsync(changeRequestEntity);
            await dbContext.SaveChangesAsync();

            return changeRequestMapper.MapChangeRequestToDto(savedRequest.Entity);
        }



        // =========== DATA ACCESS  ===============//
        private List<TransactionDto> GetTransactionsForDateRange(DateOnly startDate, DateOnly endDate)
        {
            return dbContext
                .Transactions
                .Where(t => t.Date <= startDate && t.Date >= endDate)
                .OrderBy(t => t.Date)
                .ThenBy(t => t.Sequence)
                .Select(t => transactionMapper.MapTransactionToDto(t))
                .ToList();
        }

        private Transaction ProjectChangeRequestLineToTransaction(TransactionDataChangeRequestLine changeRequestLine) => changeRequestMapper.ProjectChangeRequestLineToTransaction(changeRequestLine, new Transaction());


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

        private static bool NeedToIgnoreLine(List<ApplyChangeRequestLineDto> applyChangeRequestLines, ChangeRequestLine changeRequestLine)
        {
            return applyChangeRequestLines.Any(cl => cl.LineId == changeRequestLine.Id && cl.DesiredState == ChangeRequestState.DENIED);

        }


        private static Tuple<DateOnly, DateOnly> GetDateRangeOfInputTransactions(List<InputTransactionDto> inputs)
        {
            var startDate = inputs.Max(i => i.Date);
            var endDate = inputs.Min(i => i.Date);

            return Tuple.Create(startDate, endDate);
        }
    }
}
