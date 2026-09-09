using Common.Dto.Input;
using Common.Dto.Transactions;
using DataAccess.Models.Transactions;
using Services.Interfaces.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Transactions
{
    //TBI
    internal class TransactionCategoryService : ITransactionCategoryService
    {
        public List<TransactionCategoryDto> GetAllByReference(string reference)
        {
            throw new NotImplementedException();
        }
    }
}
