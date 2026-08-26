using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Dto.ChangeRequest.Line.Base
{
    public abstract class TransactionDataChangeRequestLineDto : ChangeRequestLineDto
    {
        public decimal Amount { get; set; }
        public DateOnly Date { get; set; }
        public short Sequence { get; set; }
        public string Reference { get; set; }
    }
}
