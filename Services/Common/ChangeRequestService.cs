using Common.Dto;
using Common.Dto.Input;
using Common.Enums;
using Common.Extensions;
using DataAccess.EF.DbContextImpl;
using DataAccess.Models;
using DataAccess.Models.ChangeRequest;
using DataAccess.Models.ChangeRequest.Line;
using DataAccess.Models.ChangeRequest.Line.Base;
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
    internal class ChangeRequestService(EfDbContext dbContext, ITransactionMatchingService transactionMatchingService) : IChangeRequestService
    {

        //pre-approval process
        public ChangeRequestDto GenerateChangeRequest(List<TransactionDto> inputTransactions, List<TransactionDto> persistedTransactions)
        {
            var proposedChanges = new List<ChangeRequestTransactionDto>();
            //TBI 

            //1) missing from persisted
            ////var missingFromInputTrans = persistedTransactions
            ////    .Where(pt => inputTransactions.Any(it => transactionMatchingService.InputTransactionMatchesPersisted(pt, it)))
            ////    .Select(t => new ChangeRequestTransactionDto(t, ChangeRequestOperation.ADD));

            ////proposedChanges.AddRange(missingFromInputTrans);
            
            //2) missing from input
            ////var missingFromPersistedTrans = inputTransactions
            ////    .Where(it => persistedTransactions.Any(pt => transactionMatchingService.InputTransactionMatchesPersisted(pt, it)))
            ////    .Select(t => new ChangeRequestTransactionDto(t, ChangeRequestOperation.DELETE));

            ////proposedChanges.AddRange(missingFromPersistedTrans);

            //3) persisted but values changed



            return new ChangeRequestDto
            {
                ProposedChanges = proposedChanges
            };
        }

        public async Task<ChangeRequestDto> SaveChangeRequest(ChangeRequestDto changeRequest)
        {
            if (changeRequest?.ProposedChanges == null || changeRequest.ProposedChanges.Count == 0)
            {
                var illegalArgument = changeRequest == null ? nameof(ChangeRequestDto) : nameof(ChangeRequestDto.ProposedChanges);
                throw new ArgumentException(illegalArgument);
            }

            var changeRequestEntity = MapToChangeRequestEntity(changeRequest);

            var savedRequest = await dbContext.AddAsync(changeRequestEntity);
            await dbContext.SaveChangesAsync();

            return MapToChangeRequestDto(savedRequest.Entity);
        }


        //approval process
        public async Task CommitChangeRequest(int changeRequestId)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            var changeRequest = await dbContext.ChangeRequests.FindAsync(changeRequestId) 
                ?? throw new InvalidOperationException($"Change Request with Id = {changeRequestId} doesn't exist.");

            var changeRequestLines = GetChangeRequestLines(changeRequest);
            if (NothingToProcess(changeRequestLines))
            {
                return;
            }

            //add requests
            var addRequests = new List<Transaction>();
            foreach (ChangeRequestCreateLine addRequest in changeRequest.CreateLines ?? [])
            {
                var transactionToAdd = ProjectChangeRequestLineToTransaction(addRequest);
                addRequests.Add(transactionToAdd);
            }

            await dbContext.Transactions.AddRangeAsync(addRequests);


            //update requests
            foreach (ChangeRequestUpdateLine updateRequest in changeRequest.UpdateLines ?? [])
            {
                ProjectChangeRequestLineToTransaction(updateRequest, updateRequest.Transaction);
            }


            //delete requests 
            foreach (var deleteRequest in changeRequest.DeleteLines ?? [])
            {
                deleteRequest.Transaction.Deleted = true;
            }

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
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




        private ChangeRequest MapToChangeRequestEntity(ChangeRequestDto changeRequest)
        {
            return null;
        }

        private ChangeRequestDto MapToChangeRequestDto(ChangeRequest changeRequest)
        {
            return null;
        }

        private Transaction ProjectChangeRequestLineToTransaction(TransactionDataChangeRequestLine changeRequestLine)
        {
            return ProjectChangeRequestLineToTransaction(changeRequestLine, new Transaction());
        }


        private Transaction ProjectChangeRequestLineToTransaction(TransactionDataChangeRequestLine changeRequestLine, Transaction transaction)
        {

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
    }
}
