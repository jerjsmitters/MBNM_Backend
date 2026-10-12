using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Filters
{
    public abstract record DateFilter
    {
        public DateOnly? StartDate { get; init; }
        public DateOnly? EndDate { get; init; }
    }
}
