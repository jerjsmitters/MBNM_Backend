using Common.Dto.Criteria;
using Common.Dto.Transactions;
using DataAccess.Models.Criteria;
using DataAccess.Models.Transactions;
using Services.Interfaces.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Common
{
    public interface ITransactionMapper : IMapperWithProjection<Transaction, TransactionDto> {}
}
