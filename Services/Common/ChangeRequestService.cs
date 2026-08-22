using Common.Dto;
using Common.Enums;
using DataAccess.Abstractions.Domain;
using DataAccess.Abstractions.Repositories;
using Services.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Services.Common
{
    //TBI
    internal class ChangeRequestService(ITransactionMatchingService transactionMatchingService, IChangeRequestRepository changeRequestRepository) : IChangeRequestService
    {

        //pre-approval process
        public ChangeRequestDto GenerateChangeRequest(List<TransactionDto> inputTransactions, List<TransactionDto> persistedTransactions)
        {
            var proposedChanges = new List<ChangeRequestTransactionDto>();

            //find what's missing from each dataset + map to proposed change
            var missingFromInputTrans = persistedTransactions
                .Where(pt => inputTransactions.Any(it => transactionMatchingService.InputTransactionMatchesPersisted(pt, it)))
                .Select(t => new ChangeRequestTransactionDto(t, ChangeRequestOperation.ADD));

            proposedChanges.AddRange(missingFromInputTrans);
            

            var missingFromPersistedTrans = inputTransactions
                .Where(it => persistedTransactions.Any(pt => transactionMatchingService.InputTransactionMatchesPersisted(pt, it)))
                .Select(t => new ChangeRequestTransactionDto(t, ChangeRequestOperation.DELETE));

            proposedChanges.AddRange(missingFromPersistedTrans);


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

            var savedRequest = await changeRequestRepository.AddAsync(changeRequestEntity);

            return MapToChangeRequestDto(savedRequest);
        }


        //approval process
        public async Task CommitChangeRequest(int changeRequestId)
        {
            var changeRequest = await changeRequestRepository.GetByIdAsync(changeRequestId);
            if (changeRequest == null)
            {
                throw new InvalidOperationException($"Change Request with Id = {changeRequestId} does not exist");
            }

            var deleteRequests = changeRequest.ProposedChanges.Where(pc => pc.ProposedChangeOperation == ChangeRequestOperation.DELETE);
            foreach (var deleteRequest in deleteRequests) 
            {
                //delete op 
            }


            var addRequests = changeRequest.ProposedChanges.Where(pc => pc.ProposedChangeOperation == ChangeRequestOperation.ADD);
            foreach (var deleteRequest in deleteRequests)
            {
                //add op 
            }
        }

        public async Task DenyChangeRequest(int changeRequestId)
        {
            var changeRequest = await changeRequestRepository.GetByIdAsync(changeRequestId);
            if (changeRequest == null)
            {
                throw new InvalidOperationException($"Change Request with Id = {changeRequestId} does not exist");
            }

            changeRequest.ProposedChanges.ForEach(pc => pc.RequestState = ChangeRequestState.DENIED);

            await changeRequestRepository.UpdateAsync(changeRequest);
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
