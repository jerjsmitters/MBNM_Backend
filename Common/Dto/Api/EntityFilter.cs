using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Api
{
    public record EntityFilter(int PageNumber, int PageLength, string? SearchText = null, DateOnly? StartDate = null, DateOnly? EndDate = null); 
}
