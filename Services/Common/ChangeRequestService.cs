using Common.Dto;
using Common.Dto.ChangeRequest;
using Common.Dto.ChangeRequest.Line;
using Common.Dto.ChangeRequest.Line.Base;
using Common.Dto.Input;
using Common.Enums;
using Common.Extensions;
using DataAccess.EF.DbContextImpl;
using DataAccess.Models.Base;
using DataAccess.Models.ChangeRequestEntities;
using DataAccess.Models.ChangeRequestEntities.Line;
using DataAccess.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Services.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Services.Common
{
    //TBI
    internal class ChangeRequestService(EfDbContext dbContext, ITransactionMatchingService transactionMatchingService, IMappingService mappingService) : IChangeRequestService
    {

        //pre-approval process
        public ChangeRequestDto GenerateChangeRequest(List<TransactionDto> inputTransactions, List<TransactionDto> persistedTransactions)
        {

            //1) Add operations
            var needToBeAdded = inputTransactions
                .Where(it => !persistedTransactions.Any(pt => transactionMatchingService.InputTransactionMatchesPersisted(it, pt)))
                .Select(
                    t => new ChangeRequestCreateLineDto 
                    {
                        Amount = t.Amount,
                        Date = t.Date,
                        Reference = t.Reference,
                        Sequence = t.Sequence,
                        RequestState = ChangeRequestState.PENDING
                    }
                )
                .ToList();


            //2) Update Operations
            var needToBeUpdate = inputTransactions
                .Where(it => persistedTransactions.Any(pt => transactionMatchingService.NeedsUpdating(it, pt))) 
                .Select(
                    t => new ChangeRequestUpdateLineDto
                    {
                        Amount = t.Amount,
                        Date = t.Date,
                        Reference = t.Reference,
                        Sequence = t.Sequence,
                        RequestState = ChangeRequestState.PENDING
                    }
                )
                .ToList();

            //3) persisted but values changed
            var needToDelete = persistedTransactions
                .Where(pt => !inputTransactions.Any(it => transactionMatchingService.InputTransactionMatchesPersisted(pt, it)))
                .Select(
                    t => new ChangeRequestDeleteLineDto
                    {
                        Transaction = t,
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

        public async Task<ChangeRequestDto> SaveChangeRequest(ChangeRequestDto changeRequest)
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





        private Transaction ProjectChangeRequestLineToTransaction(TransactionDataChangeRequestLine changeRequestLine)
        {
            return ProjectChangeRequestLineToTransaction(changeRequestLine, new Transaction());
        }


        private static Transaction ProjectChangeRequestLineToTransaction(TransactionDataChangeRequestLine changeRequestLine, Transaction transaction)
        {
            transaction.Sequence = changeRequestLine.Sequence;
            transaction.Reference = changeRequestLine.Reference;
            transaction.Amount = changeRequestLine.Amount;
            return transaction;
        }


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
    }
}
