using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Input
{
    public class ProposedTransactionChangesDto
    {
        public ProposedTransactionChangesDto()
        {
            proposedChanges = [];
        }

        //other metadata
        public List<ProposedTransactionChangeDto> proposedChanges;
    }
}
