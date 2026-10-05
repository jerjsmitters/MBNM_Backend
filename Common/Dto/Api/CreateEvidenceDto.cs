using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Api
{
    public record class CreateEvidenceDto(int TransactionId, IFormFile File);
}
