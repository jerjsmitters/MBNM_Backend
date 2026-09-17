using Common.Dto.Base;
using Common.Dto.Transactions;

namespace Common.Dto.ChangeRequest.Line
{
    public record ChangeRequestDeleteLineDto : ChangeRequestLineDto
    {
        public TransactionDto TransactionToDelete { get; init; }
    }
}
