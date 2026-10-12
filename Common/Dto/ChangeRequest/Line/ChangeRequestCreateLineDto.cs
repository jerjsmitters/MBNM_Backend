using Common.Dto.Base;
using Common.Dto.Criteria;
using Common.Enums;

namespace Common.Dto.ChangeRequest.Line
{
    public record ChangeRequestCreateLineDto : ChangeRequestLineDto
    {
        public decimal ProposedAmount { get; init; }
        public DateOnly ProposedDate { get; init; }
        public int ProposedSequence { get; init; }
        public string ProposedReference { get; init; }
        public List<TransactionCategoryDto> ProposedCategories { get; init; }
        public List<TransactionPropertyDto> ProposedProperties { get; init; }
    }
}
