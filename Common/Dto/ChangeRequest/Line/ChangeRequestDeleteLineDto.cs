using Common.Dto.ChangeRequest.Line.Base;
using Common.Dto.Transactions;

namespace Common.Dto.ChangeRequest.Line
{
    public record ChangeRequestDeleteLineDto : ChangeRequestLineDto
    {
        public TransactionDto TransactionToDelete { get; init; }
    }
}
