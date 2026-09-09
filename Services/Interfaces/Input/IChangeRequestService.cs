using Common.Dto;
using Common.Dto.ChangeRequest;
using Common.Dto.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Input
{
    public interface IChangeRequestService
    {
        Task<ChangeRequestDto> GenerateChangeRequestFromInputTransactions(InputTransactionsDto dataInput);

        Task<List<ChangeRequestCommitResultDto>> CommitChangeRequest(int changeRequestId);

        Task DenyChangeRequest(int requestId);
    }
}
