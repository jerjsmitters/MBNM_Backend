using Common.Enums;
using DataAccess.Abstractions.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstractions.Domain
{
    public class ChangeRequestTransaction : EntityBase
    {
        public Transaction Transaction { get; set; }
        public ChangeRequestOperation ProposedChangeOperation { get; set; }
        public ChangeRequestState RequestState { get; set; }
    }
}
