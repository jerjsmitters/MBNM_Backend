using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.Api
{
    public record class CreateEvidenceDto(int TransactionId, byte[] Bytes);
}
