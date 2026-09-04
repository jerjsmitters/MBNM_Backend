using Common.Dto;
using Common.Dto.ChangeRequest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common.Interfaces
{
    public interface IChangeRequestService
    {
        ChangeRequestDto GenerateChangeRequest(List<TransactionDto> inputTransactions, List<TransactionDto> persistedTransactions);
        Task<ChangeRequestDto> SaveChangeRequest(ChangeRequestDto proposedChanges);

        Task<List<ChangeRequestCommitResultDto>> CommitChangeRequest(int changeRequestId);

        Task DenyChangeRequest(int requestId);
    }
}
