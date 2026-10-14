using Common.Dto.ChangeRequest;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces.Input
{
    public interface IInputService
    {
        Task<ChangeRequestDto> AddTransactions(IFormFile file);
    }
}
