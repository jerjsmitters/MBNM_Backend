using Common.Dto.Transactions;
using DataAccess.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Transactions
{
    public interface ITransactionPropertyService
    {
        Task<TransactionPropertyDto> Add(TransactionPropertyDto transactionDto);
        Task Delete(int id);
        Task<TransactionPropertyDto?> Get(int id);
        Task<List<TransactionPropertyDto>> GetAll();
        List<TransactionPropertyDto> GetTransactionPropertiesByReference(string reference);
        Task<TransactionPropertyDto?> Update(TransactionPropertyDto transactionDto);
    }
}
