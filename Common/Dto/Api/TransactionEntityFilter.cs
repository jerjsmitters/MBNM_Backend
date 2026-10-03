using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Api
{
    public record TransactionEntityFilter(int PageNumber, int PageLength, List<int> CategoryIds, List<int> PropertyIds, string? SearchText = null, DateOnly? StartDate = null, DateOnly? EndDate = null); 
}
