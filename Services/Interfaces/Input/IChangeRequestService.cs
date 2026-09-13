using Common.Dto;
using Common.Dto.Api;
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

        Task<List<ChangeRequestCommitResultDto>> ApplyChangeRequest(ApplyChangeRequestDto changeRequest);
    }
}
