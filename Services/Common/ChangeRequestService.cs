using Common.Dto;
using Common.Enums;
using Common.Extensions;
using DataAccess.EF.DbContextImpl;
using DataAccess.Models.ChangeRequest;
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
            await using var dbTransaction = await dbContext.Database.BeginTransactionAsync();

            var changeRequest = await dbContext.ChangeRequests.FindAsync(changeRequestId);

            //TBI 

            //if (changeRequest == null || ListHelpers.IsNullOrEmpty(changeRequest.ProposedAddUpdateChanges))
            //{
            //    throw new InvalidOperationException($"Change Request with Id = {changeRequestId} either doesn't exist or have any changes to commit");
            //}

            //var addRequests = changeRequest.ProposedAddUpdateChanges.Where(pc => pc.ProposedChangeOperation == ChangeRequestOperation.ADD);
            //foreach (var deleteRequest in deleteRequests)
            //{
            //    //add op 
            //}

            //var updatedRequests = changeRequest.ProposedAddUpdateChanges.Where(pc => pc.ProposedChangeOperation)

            //var deleteRequests = changeRequest.ProposedAddUpdateChanges.Where(pc => pc.ProposedChangeOperation == ChangeRequestOperation.DELETE);
            //foreach (var deleteRequest in deleteRequests) 
            //{
            //    deleteRequest.Transaction.Deleted = true;
            //}

            

            dbTransaction.Commit();
        }

        public async Task DenyChangeRequest(int changeRequestId)
        {
            await using var dbTransaction = await dbContext.Database.BeginTransactionAsync();

            var changeRequest = await dbContext.ChangeRequests.FindAsync(changeRequestId);
            if (changeRequest == null)
            {
                throw new InvalidOperationException($"Change Request with Id = {changeRequestId} does not exist");
            }

            //TBI
            //changeRequest.ProposedChanges.ForEach(pc => pc.RequestState = ChangeRequestState.DENIED);

            dbTransaction.Commit();
        }


        private ChangeRequest MapToChangeRequestEntity(ChangeRequestDto changeRequest)
        {
            return null;
        }

        private ChangeRequestDto MapToChangeRequestDto(ChangeRequest changeRequest)
        {
            return null;

        }
    }
}
