using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.ChangeRequest.Line.Base
{
    public abstract class ChangeRequestLine
    {
        public ChangeRequestState RequestState { get; set; }

    }
}
