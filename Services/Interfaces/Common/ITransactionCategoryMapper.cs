using Common.Dto.Criteria;
using DataAccess.Models.Criteria;
using Services.Interfaces.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Common
{
    public interface ITransactionCategoryMapper : IMapperWithProjection<TransactionCategory, TransactionCategoryDto> {}
}
