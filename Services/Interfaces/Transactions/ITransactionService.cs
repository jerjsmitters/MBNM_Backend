using Common.Dto.Api;
using Common.Dto.ChangeRequest;
using Common.Dto.Input;
using Common.Dto.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Transactions
{
    public interface ITransactionService
    {
        Task<TransactionDto?> Get(int id);

        Task<List<TransactionDto>> GetAll();

        Task<List<TransactionDto>> GetAllFiltered(EntityFilter entityFilter);

        Task Delete(int id);
    }
}
