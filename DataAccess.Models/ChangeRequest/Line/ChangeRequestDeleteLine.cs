using Common.Enums;
using DataAccess.Models.Base;
using DataAccess.Models.ChangeRequest.Line.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.ChangeRequest.Line
{
    public class ChangeRequestDeleteLine : ChangeRequestLine
    {
        public Transaction Transaction { get; set; }
    }
}
