using Common.Dto.ChangeRequest;
using DataAccess.Models.Base;
using DataAccess.Models.ChangeRequestEntities;
using DataAccess.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Common
{
    public interface IChangeRequestMapper
    {
        ChangeRequest MapDtoToChangeRequest(ChangeRequestDto changeRequest);
        ChangeRequestDto MapChangeRequestToDto(ChangeRequest changeRequest);
        Transaction ProjectChangeRequestLineToTransaction(TransactionDataChangeRequestLine changeRequestLine, Transaction transaction);
    }
}
