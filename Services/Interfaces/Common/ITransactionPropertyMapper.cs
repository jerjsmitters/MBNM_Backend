using Common.Dto.Transactions;
using DataAccess.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Common
{
    public interface ITransactionPropertyMapper
    {
        TransactionPropertyDto MapPropertyToDto(TransactionProperty property);
        TransactionProperty MapDtoToProperty(TransactionPropertyDto property);
        TransactionProperty ProjectDtoToProperty(TransactionPropertyDto incoming, TransactionProperty entity);
    }
}
