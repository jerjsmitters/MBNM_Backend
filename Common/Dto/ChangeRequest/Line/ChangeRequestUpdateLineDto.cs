using Common.Dto.Base;
using Common.Dto.Transactions;

namespace Common.Dto.ChangeRequest.Line
{
    public record ChangeRequestUpdateLineDto : TransactionDataChangeRequestLineDto
    {
        public TransactionDto TransactionToUpdate { get; set; }
    }
}
