using Common.Dto;
using Common.Dto.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common.Interfaces
{
    public interface IProposedTransactionChangeService
    {
        ProposedTransactionChangesDto GenerateProposedChanges(List<InputTransactionDto> inputTransactions, List<TransactionDto> persistedTransactions);
        ProposedTransactionChangesDto SaveProposedChanges(ProposedTransactionChangesDto proposedChanges);

        void CommitProposedChanges(int requestId);
        
        void DenyProposedChanges(int requestId);
    }
}
