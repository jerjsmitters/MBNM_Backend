using Common.Dto.Base;
using Common.Dto.Transactions;
using Common.Enums;

namespace Common.Dto.ChangeRequest.Line
{
    public record ChangeRequestDeleteLineDto : ChangeRequestLineDto
    {
        public TransactionDto TransactionToDelete { get; init; }
    }
}
