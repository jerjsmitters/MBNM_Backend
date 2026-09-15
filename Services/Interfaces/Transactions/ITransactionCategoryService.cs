using Common.Dto.Transactions;
using DataAccess.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Transactions
{
    public interface ITransactionCategoryService
    {
        Task<TransactionCategoryDto> Add(TransactionCategoryDto transactionDto);
        Task Delete(int id);
        Task<TransactionCategoryDto?> Get(int id);
        Task<List<TransactionCategoryDto>> GetAll();        
        Task<TransactionCategoryDto?> Update(TransactionCategoryDto transactionDto);
    }
}
