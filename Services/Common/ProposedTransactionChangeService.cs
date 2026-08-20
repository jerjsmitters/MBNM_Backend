using Common.Dto;
using Common.Dto.Input;
using Services.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    //TBI
    internal class ProposedTransactionChangeService : IProposedTransactionChangeService
    {
        public void CommitProposedChanges(int requestId)
        {
            throw new NotImplementedException();
        }

        public void DenyProposedChanges(int requestId)
        {
            throw new NotImplementedException();
        }

        public ProposedTransactionChangesDto GenerateProposedChanges(List<InputTransactionDto> inputTransactions, List<TransactionDto> persistedTransactions)
        {
            throw new NotImplementedException();
        }

        public ProposedTransactionChangesDto SaveProposedChanges(ProposedTransactionChangesDto proposedChanges)
        {
            throw new NotImplementedException();
        }
    }
}
