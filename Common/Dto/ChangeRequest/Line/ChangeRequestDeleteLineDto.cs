using Common.Dto.ChangeRequest.Line.Base;

namespace Common.Dto.ChangeRequest.Line
{
    public class ChangeRequestDeleteLineDto : ChangeRequestLineDto
    {
        public TransactionDto Transaction { get; set; }
    }
}
