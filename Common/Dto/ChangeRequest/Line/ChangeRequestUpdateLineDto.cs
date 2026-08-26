using Common.Dto.ChangeRequest.Line.Base;

namespace Common.Dto.ChangeRequest.Line
{
    public class ChangeRequestUpdateLineDto : TransactionDataChangeRequestLineDto
    {
        public TransactionDto Transaction { get; set; }
    }
}
