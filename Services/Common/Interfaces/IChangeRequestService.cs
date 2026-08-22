using Common.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common.Interfaces
{
    public interface IChangeRequestService
    {
        ChangeRequestDto GenerateChangeRequest(List<TransactionDto> inputTransactions, List<TransactionDto> persistedTransactions);
        Task<ChangeRequestDto> SaveChangeRequest(ChangeRequestDto proposedChanges);

        Task CommitChangeRequest(int requestId);
        
        Task DenyChangeRequest(int requestId);
    }
}
