using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Filters
{
    public record TransactionEntityFilter : DateFilter
    {
        public int PageNumber { get; init; }
        public int PageLength { get; init; }
        public List<int> CategoryIds { get; init; }
        public List<int> PropertyIds { get; init; }
        public string? SearchText { get; init; }
    }
}