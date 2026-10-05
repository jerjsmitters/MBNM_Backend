using Common.Dto.ChangeRequest;
using DataAccess.Models.Base;
using DataAccess.Models.ChangeRequestEntities;
using DataAccess.Models.Transactions;
using Services.Interfaces.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Common
{
    public interface IChangeRequestMapper : IMapper<ChangeRequest, ChangeRequestDto>
    {
        Transaction ProjectTransactionChangeRequestLineToEntity(TransactionDataChangeRequestLine changeRequestLine, Transaction transaction);
    }
}
