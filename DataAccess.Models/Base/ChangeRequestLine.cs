using Common.Enums;
using DataAccess.Models.ChangeRequestEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Base
{
    public abstract class ChangeRequestLine : EntityBase
    {
        public ChangeRequestState RequestState { get; set; }
        public ChangeRequest ChangeRequest { get; set; }

    }
}
