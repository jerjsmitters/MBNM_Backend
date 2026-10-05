using Common.Dto.Transactions;
using DataAccess.Models.Transactions;
using Services.Interfaces.Common;
using Services.Interfaces.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    public class TransactionEvidenceMapper : ITransactionEvidenceMapper
    {
        public TransactionEvidence MapDtoToEntity(TransactionEvidenceDto evidenceDto)
        {
            return new()
            {
                Id = evidenceDto.Id,
                FileLocation = evidenceDto.FileLocation,

                CreatedOn = evidenceDto.CreatedOn,
                UpdatedOn = evidenceDto.UpdatedOn,
                IsDeleted = evidenceDto.IsDeleted

                //no need to map transaction as it owns the relationship
            };
        }

        public TransactionEvidenceDto MapEntityToDto(TransactionEvidence evidence)
        {
            return new()
            {
                Id = evidence.Id,
                FileLocation = evidence.FileLocation,

                CreatedOn = evidence.CreatedOn,
                UpdatedOn = evidence.UpdatedOn,
                IsDeleted = evidence.IsDeleted
                //no need to map transaction as it owns the relationship

            };
        }
    }
}
